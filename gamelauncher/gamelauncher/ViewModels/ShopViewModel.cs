using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data.Entity;
using System.Linq;
using System.Net.Http.Headers;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Input;
using gamelauncher.Model;

namespace gamelauncher.ViewModels
{
    public class ShopViewModel : INotifyPropertyChanged
    {
        private ObservableCollection<Game> _games;
        public ObservableCollection<Game> Games
        {
            get => _games;
            set
            {
                _games = value;
                OnPropertyChanged();
                FilteredGames.Refresh();
            }
        }

        private string _searchText;
        public string SearchText
        {
            get => _searchText;
            set
            {
                _searchText = value;
                OnPropertyChanged();
                FilteredGames.Refresh();
            }
        }

        public ICollectionView FilteredGames { get; }

        public ShopViewModel()
        {
            Games = new ObservableCollection<Game>();
            FilteredGames = CollectionViewSource.GetDefaultView(Games);
            FilteredGames.Filter = FilterGames;
        }

        public bool FilterGames(object obj)
        {
            if (obj is Game game)
            {
                return string.IsNullOrEmpty(SearchText) ||
                game.Title.ToLower().Contains(SearchText.ToLower()) ||
                game.GameGenres.Any(gm => gm.Genre.Name.ToLower().Contains(SearchText.ToLower()));
            }
            else
            {
                return false;
            }
        }

        public async Task LoadProducts()
        {
            using (ApplicationContext db = new ApplicationContext())
            {
                var games = await db.Games.ToListAsync();
                Games = new ObservableCollection<Game>(games);
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
