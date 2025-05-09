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

namespace gamelauncher.ViewModels
{
    public class GameViewModel : INotifyPropertyChanged
    {
        private Game _game;
        private int _id;
        private string _title;
        private decimal _price;
        private string _description;
        private DateTime? _created;
        private double? _size;
        private string _imagePath;
        private bool _isActive;
        private ObservableCollection<GenreViewModel> _allGenres;
        private ObservableCollection<PlatformViewModel> _allPlatforms;

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
                _price = value;
                OnPropertyChanged();
            }
        }

        public string Description
        {
            get => _description;
            set
            {
                _description = value;
                OnPropertyChanged();
            }
        }

        public DateTime? Created
        {
            get => _created;
            set
            {
                _created = value;
                OnPropertyChanged();
            }
        }

        public double? Size
        {
            get => _size;
            set
            {
                _size = value;
                OnPropertyChanged();
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

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            CommandManager.InvalidateRequerySuggested();
        }
    }
}
