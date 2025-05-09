using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data.Entity;
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
    public class GameCardViewModel : INotifyPropertyChanged
    {
        private Game _game;
        private int _id;
        private string _title;
        private decimal _price;
        private string _imagePath;
        private bool _isActive;
        private bool _isLiked;
        private bool _isBought;
        private ObservableCollection<GenreViewModel> _allGenres;
        private ObservableCollection<PlatformViewModel> _allPlatforms;
        private ObservableCollection<GenreViewModel> _genreList;
        private ObservableCollection<PlatformViewModel> _platformList;
        public Action<Game> NavigateToGame;


        public ObservableCollection<GenreViewModel> AllGenres
        {
            get => _allGenres;
            set
            {
                _allGenres = value;
                OnPropertyChanged();
            }
        }
        public ObservableCollection<PlatformViewModel> AllPlatforms
        {
            get => _allPlatforms;
            set
            {
                _allPlatforms = value;
                OnPropertyChanged();
            }
        }

        public int Id
        {
            get => _id;
            set
            {
                _id = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<GenreViewModel> GenreList
        {
            get => _genreList;
            set
            {
                _genreList = value;
                OnPropertyChanged();
            }
        }
        public ObservableCollection<PlatformViewModel> PlatformList
        {
            get => _platformList;
            set
            {
                _platformList = value;
                OnPropertyChanged();
            }
        }

        public string Title
        {
            get => _title;
            set
            {
                _title = value;
                OnPropertyChanged();
            }
        }

        public decimal Price
        {
            get => _price;
            set
            {
                if (_price != value)
                {
                    _price = value;
                    OnPropertyChanged();
                }
            }
        }

        public string ImagePath
        {
            get => _imagePath;
            set
            {
                _imagePath = value;
                OnPropertyChanged();
            }
        }

        public bool IsActive
        {
            get => _isActive;
            set
            {
                _isActive = value;
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

        private bool _isHovered;
        public bool IsHovered
        {
            get => _isHovered;
            set
            {
                _isHovered = value;
                OnPropertyChanged();
            }
        }

        public Game Game => _game;

        public bool IsBought
        {
            get => _isBought;
            set
            {
                _isBought = value;
                OnPropertyChanged();
            }
        }

        public bool IsFree => Price == 0;

        public ICommand LikeCommand { get; }
        public ICommand BuyCommand { get; }
        public ICommand SetHoverTrueCommand { get; }
        public ICommand SetHoverFalseCommand { get; }
        public ICommand GameNavigateCommand { get; }

        public event Action GameBought;

        public GameCardViewModel(Game game)
        {
            _game = game;
            Id = game.Id;
            Title = game.Title;
            Price = game.Price;
            ImagePath = game.CoverImagePath;
            IsActive = game.IsActive;
            GenreList = new ObservableCollection<GenreViewModel>();
            PlatformList = new ObservableCollection<PlatformViewModel>();
            IsLiked = DataWorker.isGameLiked(Id);
            IsBought = DataWorker.isGameBought(Id);
            LikeCommand = new RelayCommand(_ => Like());
            BuyCommand = new RelayCommand(_ => BuyGame());
            SetHoverTrueCommand = new RelayCommand(_ => IsHovered = true);
            SetHoverFalseCommand = new RelayCommand(_ => IsHovered = false);
            GameNavigateCommand = new RelayCommand(_ => NavigateToGame.Invoke(game));
            LoadAllGenres();
            LoadGameGenres();
            LoadAllPlatforms();
            LoadGamePlatforms();
        }

        private void Like()
        {
            DataWorker.ChangeLikeState(Id);
            IsLiked = DataWorker.isGameLiked(Id);
        }

        private void LoadGameGenres()
        {
            if (AllGenres == null || !AllGenres.Any()) return;

            using (var db = new ApplicationContext())
            {
                var gameGenreIds = db.GameGenres
                    .Where(gg => gg.GameId == Id)
                    .Select(gg => gg.GenreId)
                    .ToList();

                foreach (var genreId in gameGenreIds)
                {
                    var genre = AllGenres.FirstOrDefault(g => g.Id == genreId);
                    if (genre != null && !GenreList.Contains(genre))
                    {
                        GenreList.Add(genre);
                    }
                }
            }
        }

        private void LoadGamePlatforms()
        {
            if (AllPlatforms == null || !AllPlatforms.Any()) return;

            using (var db = new ApplicationContext())
            {
                var gamePlatformIds = db.GamePlatforms
                    .Where(gg => gg.GameId == Id)
                    .Select(gg => gg.PlatformId)
                    .ToList();

                foreach (var platformId in gamePlatformIds)
                {
                    var platform = AllPlatforms.FirstOrDefault(g => g.Id == platformId);
                    if (platform != null && !PlatformList.Contains(platform))
                    {
                        PlatformList.Add(platform);
                    }
                }
            }
        }

        private void BuyGame()
        {
            if (CurrentUser.Instance.Balance < Price)
            {
                RegisterError error = new RegisterError("Недостаточно средств на балансе");
                error.ShowDialog();
            }
            else
            {
                DataWorker.BuyGame(Id);
                CurrentUser.Instance.Balance -= Price;
                DataWorker.UpdateUser(CurrentUser.Instance);
                RegisterError error = new RegisterError("Успешная покупка");
                error.ShowDialog();
                IsBought = true;
                GameBought?.Invoke();
            }
        }

        private void LoadAllGenres()
        {
            AllGenres = DataWorker.GetAllGenres();
        }

        private void LoadAllPlatforms()
        {
            AllPlatforms = DataWorker.GetAllPlatforms();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            CommandManager.InvalidateRequerySuggested();
        }
    }
}
