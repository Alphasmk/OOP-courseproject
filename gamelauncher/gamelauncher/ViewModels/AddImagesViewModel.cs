using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using gamelauncher.Model;
using gamelauncher.MVVM;
using Microsoft.Win32;

namespace gamelauncher.ViewModels
{
    public class AddImagesViewModel : INotifyPropertyChanged
    {
        private Game _game;
        private ObservableCollection<ImageItemViewModel> _images;

        public ObservableCollection<ImageItemViewModel> Images
        {
            get => _images;
            set
            {
                _images = value;
                OnPropertyChanged();
            }
        }
        private readonly Action _closeAction;
        public ICommand AddCommand { get; }
        public ICommand CloseCommand { get; }

        public AddImagesViewModel(Game game, Action closeAction)
        {
            _game = game;
            _closeAction = closeAction;
            _images = new ObservableCollection<ImageItemViewModel>();
            _images = DataWorker.GetAllImages(game.Id, RemoveImageFromCollection);
            AddCommand = new RelayCommand(_ => AddImage());
            CloseCommand = new RelayCommand(_ => _closeAction?.Invoke());
        }

        private void AddImage()
        {
            var openFileDialog = new OpenFileDialog
            {
                Filter = "Image files (*.jpg, *.jpeg, *.png)|*.jpg;*.jpeg;*.png",
                Multiselect = false
            };

            if (openFileDialog.ShowDialog() == true)
            {
                string selectedPath = openFileDialog.FileName;

                var newGameImage = new GameImage
                {
                    GameId = _game.Id,
                    ImagePath = selectedPath
                };

                using (var db = new ApplicationContext())
                {
                    db.GameImages.Add(newGameImage);
                    db.SaveChanges();
                }

                var newItem = new ImageItemViewModel(RemoveImageFromCollection)
                {
                    GameImage = newGameImage,
                    ImagePath = selectedPath
                };

                Images.Add(newItem);
            }
        }

        private void RemoveImageFromCollection(ImageItemViewModel imageItem)
        {
            Images.Remove(imageItem);
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
