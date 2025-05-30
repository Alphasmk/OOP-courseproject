﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using gamelauncher.Model;
using gamelauncher.MVVM;
using gamelauncher.Views;

namespace gamelauncher.ViewModels
{
    enum Platforms
    {
        Windows, Apple, Linux
    }
    public class GamePageViewModel : INotifyPropertyChanged
    {
        private Game _game;
        private ObservableCollection<GameImage> _gameImages;
        private int _id;
        private string _title;
        private decimal _price;
        private string _logoPath;
        private bool _isLiked;
        private bool _isBought;
        private string _description;
        private DateTime? _date;
        private int _currentImageIndex;
        private ObservableCollection<string> _platforms;
        private ObservableCollection<string> _genres;
        public ObservableCollection<GameImage> GameImages
        {
            get => _gameImages;
            set
            {
                _gameImages = value;
                OnPropertyChanged();
            }
        }

        public int CurrentImageIndex
        {
            get => _currentImageIndex;
            set
            {
                _currentImageIndex = value;
                OnPropertyChanged();
            }
        }

        public bool IsBought
        {
            get => _isBought;
            set
            {
                _isBought = value;
                OnPropertyChanged();
            }
        }

        public bool IsLiked
        {
            get => _isLiked;
            set
            {
                _isLiked = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<string> Platforms
        {
            get => _platforms;
            set
            {
                _platforms = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<string> Genres
        {
            get => _genres;
            set
            {
                _genres = value;
                OnPropertyChanged();
            }
        }

        public string Title { get => _title; }
        public string LogoPath { get => _logoPath; }
        public decimal Price { get => _price; }
        public int Id { get => _id; }
        public string Description { get => _description; }
        public bool IsFree => Price == 0;

        public DateTime? Date { get => _date; }

        private GameImage _selectedImage;
        public GameImage SelectedImage
        {
            get => _selectedImage;
            set
            {
                _selectedImage = value;
                OnPropertyChanged();
                if (_selectedImage != null)
                    CurrentImageIndex = GameImages.IndexOf(_selectedImage);
            }
        }

        public ICommand PreviousImageCommand { get; }
        public ICommand NextImageCommand { get; }
        public ICommand BuyGameCommand { get; }

        public ICommand LikeCommand { get; }

        public GamePageViewModel(Game game)
        {
            _game = game;
            _title = game.Title;
            _id = game.Id;
            _logoPath = game.LogoImagePath;
            _price = game.Price;
            _date = game.ReleaseDate;
            _description = game.Description;
            IsBought = DataWorker.isGameBought(Id);
            IsLiked = DataWorker.isGameLiked(Id);
            LoadImages();
            PreviousImageCommand = new RelayCommand(_ => PreviousImage(), _ => CanPreviousImage());
            NextImageCommand = new RelayCommand(_ => NextImage(), _ => CanNextImage());
            BuyGameCommand = new RelayCommand(async _ => await BuyGame(), _ => CanBuyGame());
            LikeCommand = new RelayCommand(_ => Like());

            if (GameImages.Count > 0)
                SelectedImage = GameImages[0];
            GetPlatformNames();
            GetGenreNames();
        }

        private async void PreviousImage()
        {
            if (CanPreviousImage())
            {
                CurrentImageIndex--;
                await Task.Delay(300);
                SelectedImage = GameImages[CurrentImageIndex];
            }
        }

        private void GetPlatformNames()
        {
            Platforms = new ObservableCollection<string>();
            var platformsList = DataWorker.GetPlatformsByGameId(Id);
            foreach (var platform in platformsList)
            {
                switch (platform.Name)
                {
                    case "Windows":
                        Platforms.Add("/img/windowslogo.png");
                        break;
                    case "Linux":
                        Platforms.Add("/img/linuxlogo.png");
                        break;
                    case "MacOS":
                        Platforms.Add("/img/applelogo.png");
                        break;
                }
            }
        }

        private void GetGenreNames()
        {
            Genres = new ObservableCollection<string>();
            var genresList = DataWorker.GetGenresByGameId(Id);
            foreach (var genre in genresList)
            {
                Genres.Add(genre.Name);
            }
        }

        private async Task BuyGame()
        {
            if (CurrentUser.Instance.Balance < Price)
            {
                if (LanguageManager.CurrentLanguage == "ru-RU")
                {
                    RegisterError error = new RegisterError("Недостаточно средств на балансе");
                    error.ShowDialog();
                }
                else
                {
                    RegisterError error = new RegisterError("Insufficient funds on balance");
                    error.ShowDialog();
                }
            }
            else
            {
                await DataWorker.BuyGame(Id);
                CurrentUser.Instance.Balance -= Price;
                DataWorker.UpdateUser(CurrentUser.Instance);
                if (LanguageManager.CurrentLanguage == "ru-RU")
                {
                    RegisterError error = new RegisterError("Успешная покупка");
                    error.ShowDialog();
                }
                else
                {
                    RegisterError error = new RegisterError("Successful purchase");
                    error.ShowDialog();
                }
                IsBought = true;
            }
        }

        private void Like()
        {
            DataWorker.ChangeLikeState(Id);
            IsLiked = DataWorker.isGameLiked(Id);
        }

        private bool CanBuyGame()
        {
            return !IsBought;
        }

        private bool CanPreviousImage() => CurrentImageIndex > 0;

        private async void NextImage()
        {
            if (CanNextImage())
            {
                CurrentImageIndex++;
                await Task.Delay(300);
                SelectedImage = GameImages[CurrentImageIndex];
            }
        }

        private bool CanNextImage() => CurrentImageIndex < GameImages.Count - 1;

        private void LoadImages()
        {
            GameImages = DataWorker.LoadImages(_game.Id);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
