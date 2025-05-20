using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using gamelauncher.Model;
using gamelauncher.MVVM;
using gamelauncher.Views;
using System.Windows.Input;
using System.ComponentModel;
using System.Data.Entity;
using System.Windows;

namespace gamelauncher.ViewModels
{
    public class LibraryViewModel : INotifyPropertyChanged
    {
        private bool _isNull;
        private ObservableCollection<GameCardViewModel> _cards;
        private ObservableCollection<GameCardViewModel> _allCards;
        private ObservableCollection<GenreViewModel> _genres = new ObservableCollection<GenreViewModel>();
        private ObservableCollection<GenreViewModel> _selectedGenres = new ObservableCollection<GenreViewModel>();
        private ObservableCollection<PlatformViewModel> _platforms = new ObservableCollection<PlatformViewModel>();
        private ObservableCollection<PlatformViewModel> _selectedPlatforms = new ObservableCollection<PlatformViewModel>();
        private string _searchText;
        private bool _isSearchEmpty;
        private decimal _balance;
        private decimal _maxPrice;
        private decimal _minPrice;
        private decimal _lowerLimit;
        private decimal _upperLimit;
        private bool _PriceFilter;
        private bool _isList;
        private ObservableCollection<string> _groups;
        private List<UserGameGroup> _allGroups = new List<UserGameGroup>();

        private string _selectedGroup;
        private GameCardViewModel _selectedGameForGroup;
        public GameCardViewModel SelectedGameForGroup
        {
            get => _selectedGameForGroup;
            set
            {
                _selectedGameForGroup = value;
                OnPropertyChanged();
            }
        }
        public string SelectedGroup
        {
            get => _selectedGroup;
            set
            {
                _selectedGroup = value;
                OnPropertyChanged();
                FilterGames();
            }
        }

        private UserGameGroup _selectedGroupObject;
        public UserGameGroup SelectedGroupObject
        {
            get => _selectedGroupObject;
            private set
            {
                _selectedGroupObject = value;
                OnPropertyChanged();
            }
        }
        public ObservableCollection<GameCardViewModel> Cards
        {
            get => _cards;
            set
            {
                _cards = value;
                OnPropertyChanged();
            }
        }

        public bool IsList
        {
            get => _isList;
            set
            {
                _isList = value;
                OnPropertyChanged();
            }
        }
        public ObservableCollection<string> Groups
        {
            get => _groups;
            set
            {
                _groups = value;
                OnPropertyChanged();
            }
        }

        public bool PriceFilter
        {
            get => _PriceFilter;
            set
            {
                if (_PriceFilter != value)
                {
                    _PriceFilter = value;
                    OnPropertyChanged();
                }
            }
        }

        public decimal MaxPrice
        {
            get => _maxPrice;
            set
            {
                _maxPrice = value;
                OnPropertyChanged();
            }
        }

        public decimal MinPrice
        {
            get => _minPrice;
            set
            {
                _minPrice = value;
                OnPropertyChanged();
            }
        }

        public decimal LowerLimit
        {
            get => _lowerLimit;
            set
            {
                if (value < 0)
                    _lowerLimit = 0;
                else
                    _lowerLimit = value;

                OnPropertyChanged();
                FilterGames();
            }
        }

        public decimal UpperLimit
        {
            get => _upperLimit;
            set
            {
                if (value < LowerLimit)
                    _upperLimit = 0;
                else
                    _upperLimit = value;
                OnPropertyChanged();
                FilterGames();
            }
        }

        public ObservableCollection<GenreViewModel> AllGenres
        {
            get => _genres;
            set
            {
                _genres = value;
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
                FilterGames();
            }
        }
        public ObservableCollection<PlatformViewModel> AllPlatforms
        {
            get => _platforms;
            set
            {
                _platforms = value;
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
                FilterGames();
            }
        }

        public decimal Balance
        {
            get => _balance;
            set
            {
                _balance = value;
                OnPropertyChanged();
            }
        }

        public string SearchText
        {
            get => _searchText;
            set
            {
                _searchText = value;
                OnPropertyChanged();
                FilterGames();
            }
        }

        public bool IsSearchEmpty
        {
            get => _isSearchEmpty;
            set
            {
                _isSearchEmpty = value;
                OnPropertyChanged();
            }
        }

        public bool IsNull
        {
            get => _isNull;
            set
            {
                _isNull = value;
                OnPropertyChanged();
            }
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is double number)
            {
                return number.ToString("#,##0.00", culture);
            }
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string strValue && double.TryParse(strValue, NumberStyles.Any, culture, out double number))
            {
                return number;
            }
            return 0;
        }

        private void LoadGames()
        {
            using (ApplicationContext db = new ApplicationContext())
            {
                var boughtGameIds = db.Library
                    .Where(w => w.UserId == CurrentUser.Instance.Id)
                    .Select(w => w.GameId)
                    .ToList();

                var games = db.Games
                   .Where(g => g.IsActive && boughtGameIds.Contains(g.Id))
                   .Include(g => g.GameGenres.Select(gg => gg.Genre))
                   .Include(g => g.GamePlatforms.Select(gp => gp.Platform))
                   .ToList();

                _allCards = new ObservableCollection<GameCardViewModel>();
                foreach (var game in games)
                {
                    var card = new GameCardViewModel(game);
                    card.GameBought += OnGameBought;
                    card.NavigateToGame += OnGameNavigate;
                    _allCards.Add(card);
                }

                Cards = new ObservableCollection<GameCardViewModel>(_allCards);
                IsNull = Cards.Count == 0;
            }
        }


        private void OnGameBought()
        {
            UpdateBalance();
        }

        private void OnGameNavigate(Game game)
        {
            GameNavigate?.Invoke(game);
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

        private void SearchMaxAndMin()
        {
            if (_allCards == null || !_allCards.Any()) return;

            var priceCards = _allCards.Where(c => c.Price >= 0).ToList();

            if (!priceCards.Any())
            {
                MaxPrice = 0;
                MinPrice = 0;
                LowerLimit = 0;
                UpperLimit = 0;
                return;
            }

            MaxPrice = priceCards.Max(c => c.Price);
            MinPrice = priceCards.Min(c => c.Price);

            LowerLimit = MinPrice;
            UpperLimit = MaxPrice;
        }

        private void LoadGroups()
        {
            var userGroups = DataWorker.GetAllUserGroups();
            var groupNames = userGroups.Select(g => g.Name).ToList();
            if(LanguageManager.CurrentLanguage == "ru-RU")
            {
                groupNames.Insert(0, "Все");
            }
            else
            {
                groupNames.Insert(0, "All");
            }

            Groups = new ObservableCollection<string>(groupNames);
            if (LanguageManager.CurrentLanguage == "ru-RU")
            {
                SelectedGroup = "Все";
            }
            else
            {
                SelectedGroup = "All";
            }

            _allGroups = userGroups.ToList();
        }


        private void FilterGames()
        {
            if (_allCards == null) return;

            var filtered = _allCards.AsEnumerable();

            if (!string.IsNullOrEmpty(SelectedGroup) && SelectedGroup != "Все" && SelectedGroup != "All")
            {
                var group = _allGroups.FirstOrDefault(g => g.Name == SelectedGroup);
                if (group != null)
                {
                    filtered = filtered.Where(c => c.Groups.Any(g => g.Id == group.Id));
                }
            }

            if (!string.IsNullOrWhiteSpace(SearchText))
            {
                var searchLower = SearchText.ToLower();
                filtered = filtered.Where(c =>
                    c.Title.ToLower().Contains(searchLower));
            }

            if (SelectedGenres != null && SelectedGenres.Any())
            {
                var selectedIds = SelectedGenres.Select(g => g.Id).ToList();
                filtered = filtered.Where(game =>
                    selectedIds.All(selectedId =>
                        game.GenreList.Any(gameGenre => gameGenre.Id == selectedId)));
            }

            if (SelectedPlatforms != null && SelectedPlatforms.Any())
            {
                var selectedPlatformIds = SelectedPlatforms.Select(p => p.Id).ToList();
                filtered = filtered.Where(game =>
                    selectedPlatformIds.All(platformId =>
                        game.PlatformList.Any(gamePlatform => gamePlatform.Id == platformId)));
            }

            filtered = filtered.Where(c => (c.Price >= LowerLimit && c.Price <= UpperLimit));

            Cards = new ObservableCollection<GameCardViewModel>(filtered.ToList());
            bool hasAnyFilter =
            !string.IsNullOrWhiteSpace(SearchText)
            || SelectedGenres.Count > 0
            || SelectedPlatforms.Count > 0
            || LowerLimit > MinPrice
            || UpperLimit < MaxPrice;

            IsSearchEmpty = hasAnyFilter && !Cards.Any();
            
        }

        public ICommand ResetCommand { get; }
        public ICommand AddGroupCommand { get; }
        public ICommand NavigateToGame { get; }

        public ICommand ListCommand { get; }
        public ICommand CardsCommand { get; }
        public ICommand SelectGroupCommand { get; }

        private void SelectGroup(string groupName)
        {
            SelectedGroup = groupName;
        }

        public Action<Game> GameNavigate;

        public LibraryViewModel(Action<Game> gameNavigate)
        {
            IsList = false;
            _allCards = new ObservableCollection<GameCardViewModel>();
            Cards = new ObservableCollection<GameCardViewModel>();
            SelectedGenres = new ObservableCollection<GenreViewModel>();
            Balance = CurrentUser.Instance.Balance;
            GameNavigate += gameNavigate;
            LoadGames();
            LoadAllGenres();
            LoadAllPlatforms();
            SearchMaxAndMin();
            LoadGroups();

            ResetCommand = new RelayCommand(_ => ResetFilters());
            AddGroupCommand = new RelayCommand(_ => AddGroup());
            ListCommand = new RelayCommand(_ => ChangeState(), _ => CanDoList());
            CardsCommand = new RelayCommand(_ => ChangeState(), _ => CanDoCards());
            SelectGroupCommand = new RelayCommand(group => SelectGroup(group as string));

            SelectedGenres.CollectionChanged += (s, e) => FilterGames();
            SelectedPlatforms.CollectionChanged += (s, e) => FilterGames();
        }

        private bool CanDoList()
        {
            return !IsList;
        }
        private bool CanDoCards()
        {
            return IsList;
        }

        private void ChangeState()
        {
            IsList = !IsList;
        }

        private void UpdateBalance()
        {
            Balance = CurrentUser.Instance.Balance;
            OnPropertyChanged(nameof(Balance));
        }

        private void AddGroup()
        {
            AddGroup adding = new AddGroup(() =>
            {
                RefreshGamesAndGroups();
            });
            adding.ShowDialog();
        }


        private void RefreshGamesAndGroups()
        {
            LoadGroups();
            LoadGames(); 
            FilterGames(); 
        }

        private void ResetFilters()
        {
            UpperLimit = MaxPrice;
            LowerLimit = MinPrice;

            SelectedGenres.Clear();
            SelectedPlatforms.Clear();

            LoadAllGenres();
            LoadAllPlatforms();

            foreach (var genre in AllGenres)
                genre.IsSelected = false;

            foreach (var platform in AllPlatforms)
                platform.IsSelected = false;

            FilterGames();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            CommandManager.InvalidateRequerySuggested();
        }
    }
}
