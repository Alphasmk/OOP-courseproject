using System;
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

namespace gamelauncher.ViewModels
{
    public class ImageItemViewModel : INotifyPropertyChanged
    {
        private GameImage _gameimage;
        private string _imagePath;
        private Action<ImageItemViewModel> _onDeleteAction;
        public string ImagePath
        {
            get => _imagePath;
            set
            {
                _imagePath = value;
                OnPropertyChanged();
            }
        }

        public GameImage GameImage
        {
            get => _gameimage;
            set
            {
                _gameimage = value;
                OnPropertyChanged();
            }
        }

        public ICommand DeleteCommand { get; }
        public ICommand ChangeCommand { get; }
        

        public ImageItemViewModel(Action<ImageItemViewModel> onDeleteAction)
        {
            _onDeleteAction = onDeleteAction;
            DeleteCommand = new RelayCommand(_ => DeleteImage());
            ChangeCommand = new RelayCommand(_ => ChangeImage());
        }

        private void ChangeImage()
        {
            var openFileDialog = new Microsoft.Win32.OpenFileDialog
            {
                Filter = "Image files (*.jpg, *.jpeg, *.png)|*.jpg;*.jpeg;*.png",
                Multiselect = false
            };

            if (openFileDialog.ShowDialog() == true)
            {
                ImagePath = openFileDialog.FileName;
                GameImage.ImagePath = ImagePath;

                using (var db = new ApplicationContext())
                {
                    db.GameImages.Update(GameImage);
                    db.SaveChanges();
                }
            }
        }

        private void DeleteImage()
        {
            using (var db = new ApplicationContext())
            {
                db.GameImages.Remove(GameImage);
                _onDeleteAction?.Invoke(this);
                db.SaveChanges();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
