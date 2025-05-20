using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using System.Net.Http.Headers;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using gamelauncher.Model;
using gamelauncher.MVVM;
using gamelauncher.Views;

namespace gamelauncher.ViewModels
{
    public class ShopViewModel : INotifyPropertyChanged
    {
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
        private string _balanceTitle;
        public string BalanceTitle
        {
            get => _balanceTitle;
            set
            {
                _balanceTitle = value;
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
                var games = db.Games
               .Where(g => g.IsActive)
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


        private void FilterGames()
        {
            if (_allCards == null) return;

            var filtered = _allCards.AsEnumerable();

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
        public ICommand ReplenishCommand { get; }
        public ICommand NavigateToGame {  get; }

        public Action<Game> GameNavigate;

        public ShopViewModel(Action<Game> gameNavigate)
        {
            _allCards = new ObservableCollection<GameCardViewModel>();
            Cards = new ObservableCollection<GameCardViewModel>();
            SelectedGenres = new ObservableCollection<GenreViewModel>();
            Balance = CurrentUser.Instance.Balance;
            BalanceTitle = $"{Application.Current.Resources["Balance"]}: ${Balance:N2}";
            GameNavigate += gameNavigate;
            LoadGames();
            LoadAllGenres();
            LoadAllPlatforms();
            SearchMaxAndMin();

            ResetCommand = new RelayCommand(_ => ResetFilters());
            ReplenishCommand = new RelayCommand(_ => ReplenishBalance());

            SelectedGenres.CollectionChanged += (s, e) => FilterGames();
            SelectedPlatforms.CollectionChanged += (s, e) => FilterGames();
        }

        private void UpdateBalance()
        {
            Balance = CurrentUser.Instance.Balance;
            OnPropertyChanged(nameof(Balance));
        }

        private void ReplenishBalance()
        {
            ReplenishmentPage replenishment = new ReplenishmentPage(newBalance =>
            {
                Balance = newBalance;
                BalanceTitle = $"{Application.Current.Resources["Balance"]}: ${Balance:N2}";
                OnPropertyChanged(nameof(Balance));
            });
            replenishment.ShowDialog();
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
