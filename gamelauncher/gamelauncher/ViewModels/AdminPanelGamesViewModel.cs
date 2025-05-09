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
using gamelauncher.Views;

namespace gamelauncher.ViewModels
{
    public class AdminPanelGamesViewModel : INotifyPropertyChanged
    {
        private List<Game> _games;
        private Game _selectedGame;
        private bool _isSelected;
        private bool isEditing;

        public List<Game> Games
        {
            get => _games;
            set
            {
                _games = value;
                OnPropertyChanged();
            }
        }

        public Game SelectedGame
        {
            get => _selectedGame;
            set
            {
                _selectedGame = value;
                OnPropertyChanged();
                if (_selectedGame != null)
                {
                    IsSelected = true;
                }
                else
                {
                    IsSelected = false;
                }
                CommandManager.InvalidateRequerySuggested();
            }
        }

        public bool IsSelected
        {
            get => _isSelected;
            set
            {
                _isSelected = value;
                OnPropertyChanged();
            }
        }

        public ICommand RefreshCommand { get; }
        public ICommand DeleteCommand { get; }
        public ICommand EditOrCreateCommand { get; }
        public ICommand ClearSelectionCommand { get; }

        public AdminPanelGamesViewModel()
        {
            RefreshCommand = new RelayCommand(_ => LoadGames());
            DeleteCommand = new RelayCommand(_ => DeleteGame(), _ => CanDelete());
            EditOrCreateCommand = new RelayCommand(_ => EditOrCreateGame());
            ClearSelectionCommand = new RelayCommand(_ => ClearSelection());
            LoadGames();
        }

        private bool CanDelete()
        {
            return SelectedGame != null;
        }

        private void ClearSelection()
        {
            SelectedGame = null;
        }

        private void LoadGames()
        {
            Games = DataWorker.GetAllGames().ToList();
        }

        private void DeleteGame()
        {
            if (SelectedGame != null)
            {
                if (DataWorker.DeleteGame(SelectedGame))
                {
                    LoadGames();
                }
            }
        }

        private void EditOrCreateGame()
        {
            isEditing = SelectedGame != null;
            var editPage = new EditGame(SelectedGame, isEditing);
            editPage.ShowDialog();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
