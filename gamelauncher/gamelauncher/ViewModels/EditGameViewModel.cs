using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data.Entity;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Input;
using gamelauncher.Model;
using gamelauncher.MVVM;
using gamelauncher.Views;

namespace gamelauncher.ViewModels
{
    public class EditGameViewModel : INotifyPropertyChanged
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
        private bool _isEditing;
        private ObservableCollection<GenreViewModel> _allGenres;
        private ObservableCollection<GenreViewModel> _selectedGenres;
        private ObservableCollection<PlatformViewModel> _allPlatforms;
        private ObservableCollection<PlatformViewModel> _selectedPlatforms;

        public ObservableCollection<GenreViewModel> AllGenres
        {
            get => _allGenres;
            set
            {
                _allGenres = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<GenreViewModel> SelectedGenres
        {
            get => _selectedGenres;
            set
            {
                _selectedGenres = value;
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

        public ObservableCollection<PlatformViewModel> SelectedPlatforms
        {
            get => _selectedPlatforms;
            set
            {
                _selectedPlatforms = value;
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

        public bool IsEdit
        {
            get => _isEditing;
            set
            {
                _isEditing = value;
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

        private readonly Action _closeAction;
        public ICommand CloseCommand { get; }
        public ICommand SaveOrCreateCommand { get; }
        public ICommand ChangeActivityState { get; }
        public ICommand BrowseImageCommand { get; }
        public ICommand AddImagesCommand { get; }
        public EditGameViewModel(Game game, Action closeAction, bool IsEditing = false)
        {
            IsEdit = IsEditing;
            LoadAllGenres();
            LoadAllPlatforms();
            if (IsEdit)
            {
                _game = game;
                Id = game.Id;
                Title = game.Title;
                Description = game.Description;
                Price = game.Price;
                Created = game.ReleaseDate;
                Size = game.SizeGB;
                ImagePath = game.CoverImagePath;
                IsActive = game.IsActive;
            }
            SelectedGenres = new ObservableCollection<GenreViewModel>();
            SelectedPlatforms = new ObservableCollection<PlatformViewModel>();
            SaveOrCreateCommand = new RelayCommand(_ => SaveOrCreateGame(), _ => Validate());
            BrowseImageCommand = new RelayCommand(_ => BrowseImage());
            AddImagesCommand = new RelayCommand(_ => AddImages());
            _closeAction = closeAction;
            CloseCommand = new RelayCommand(_ => ExecuteClose());
            ChangeActivityState = new RelayCommand(_ => ChangeActivity());
            if (IsEditing)
            {
                LoadSelectedGenres();
                LoadSelectedPlatforms();
            }
        }

        private void AddImages()
        {
            AddImagesPage add = new AddImagesPage(_game);
            add.ShowDialog();
        }

        private void BrowseImage()
        {
            var openFileDialog = new Microsoft.Win32.OpenFileDialog
            {
                Filter = "Image files (*.png;*.jpeg;*.jpg;*.bmp)|*.png;*.jpeg;*.jpg;*.bmp|All files (*.*)|*.*"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                ImagePath = openFileDialog.FileName;
            }
        }

        private void ChangeActivity()
        {
            IsActive = !IsActive;
        }

        private void LoadSelectedGenres()
        {
            using (var db = new ApplicationContext())
            {
                var gameGenres = db.GameGenres
                    .Where(gg => gg.GameId == _game.Id)
                    .Include(gg => gg.Genre)
                    .ToList();

                foreach (var gameGenre in gameGenres)
                {
                    var genre = AllGenres.FirstOrDefault(g => g.Id == gameGenre.GenreId);
                    if (genre != null)
                    {
                        genre.IsSelected = true;
                        SelectedGenres.Add(genre);
                    }
                }
            }
        }

        private void LoadSelectedPlatforms()
        {
            using (var db = new ApplicationContext())
            {
                var gamePlatforms = db.GamePlatforms
                    .Where(gg => gg.GameId == _game.Id)
                    .Include(gg => gg.Platform)
                    .ToList();

                foreach (var gamePlatform in gamePlatforms)
                {
                    var platform = AllPlatforms.FirstOrDefault(g => g.Id == gamePlatform.PlatformId);
                    if (platform != null)
                    {
                        platform.IsSelected = true;
                        SelectedPlatforms.Add(platform);
                    }
                }
            }
        }

        private void SaveOrCreateGame()
        {
            using (var db = new ApplicationContext())
            {
                if (_game == null)
                {
                    _game = new Game
                    {
                        Title = Title,
                        Description = Description,
                        Price = Price,
                        ReleaseDate = Created ?? DateTime.Now,
                        SizeGB = Size ?? 0,
                        CoverImagePath = ImagePath,
                        IsActive = IsActive
                    };

                    db.Games.Add(_game);
                    db.SaveChanges(); 

                    UpdateGameGenres(db);
                    UpdateGamePlatforms(db);
                    db.SaveChanges();
                }
                else
                {
                    db.Games.Attach(_game);
                    _game.Title = Title;
                    _game.Description = Description;
                    _game.Price = Price;
                    _game.ReleaseDate = Created ?? _game.ReleaseDate;
                    _game.SizeGB = Size ?? _game.SizeGB;
                    _game.CoverImagePath = ImagePath;
                    _game.IsActive = IsActive;

                    UpdateGameGenres(db);
                    UpdateGamePlatforms(db);
                    db.SaveChanges();
                }

                _closeAction?.Invoke();
            }
        }

        private void UpdateGamePlatforms(ApplicationContext db)
        {
            var existingPlatforms = db.GamePlatforms.Where(gg => gg.GameId == _game.Id).ToList();
            db.GamePlatforms.RemoveRange(existingPlatforms);

            foreach (var platform in AllPlatforms.Where(g => g.IsSelected))
            {
                db.GamePlatforms.Add(new GamePlatform
                {
                    GameId = _game.Id,
                    PlatformId = platform.Id
                });
            }
        }

        private void UpdateGameGenres(ApplicationContext db)
        {
            var existingGenres = db.GameGenres.Where(gg => gg.GameId == _game.Id).ToList();
            db.GameGenres.RemoveRange(existingGenres);

            foreach (var genre in AllGenres.Where(g => g.IsSelected))
            {
                db.GameGenres.Add(new GameGenre
                {
                    GameId = _game.Id,
                    GenreId = genre.Id
                });
            }
        }

        private void ExecuteClose()
        {
            _closeAction?.Invoke();
        }
        private void LoadAllGenres()
        {
            AllGenres = DataWorker.GetAllGenres();
            foreach (var genre in AllGenres)
            {
                genre.PropertyChanged += (s, e) =>
                {
                    if (e.PropertyName == nameof(GenreViewModel.IsSelected))
                    {
                        var genreVm = (GenreViewModel)s;
                        if (genreVm.IsSelected)
                        {
                            if (!SelectedGenres.Contains(genreVm))
                                SelectedGenres.Add(genreVm);
                        }
                        else
                        {
                            SelectedGenres.Remove(genreVm);
                        }
                    }
                };
            }
        }

        private void LoadAllPlatforms()
        {
            AllPlatforms = DataWorker.GetAllPlatforms();
            foreach (var platform in AllPlatforms)
            {
                platform.PropertyChanged += (s, e) =>
                {
                    if (e.PropertyName == nameof(PlatformViewModel.IsSelected))
                    {
                        var platformVm = (PlatformViewModel)s;
                        if (platformVm.IsSelected)
                        {
                            if (!SelectedPlatforms.Contains(platformVm))
                                SelectedPlatforms.Add(platformVm);
                        }
                        else
                        {
                            SelectedPlatforms.Remove(platformVm);
                        }
                    }
                };
            }
        }

        private bool Validate()
        {
            string title_pattern = @"^[A-Za-zА-Яа-я0-9_]{1,20}$";

            string path_pattern = @"^(?:[a-zA-Z]:)?(\\[A-Za-zА-Яа-я0-9._ -]+)+\\?[A-Za-zА-Яа-я0-9._ -]+\.[a-zA-Z0-9]{1,5}$";

            bool isTitleValid = !string.IsNullOrEmpty(Title) && Regex.IsMatch(Title, title_pattern);
            bool isPathValid = !string.IsNullOrEmpty(ImagePath) && Regex.IsMatch(ImagePath, path_pattern);

            if(SelectedGenres.Count() == 0 || SelectedPlatforms.Count() == 0)
            {
                return false;
            }

            return isTitleValid && isPathValid;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
