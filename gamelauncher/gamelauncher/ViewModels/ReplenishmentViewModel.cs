using System;
using System.Collections.Generic;
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
    public class ReplenishmentViewModel : INotifyPropertyChanged
    {
        private decimal _inputReplenishment;
        private decimal _balance;
        private decimal _additionalSum;
        private bool _canReplenish;
        public decimal InputReplenishment
        {
            get => _inputReplenishment;
            set
            {
                _inputReplenishment = value;
                OnPropertyChanged();
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

        public decimal AdditionalSum
        {
            get => _additionalSum;
            set
            {
                _additionalSum = value < 0 ? 0 : value;
                OnPropertyChanged();
            }
        }

        public bool CanReplenish
        {
            get => _canReplenish;
            set
            {
                _canReplenish = value;
                OnPropertyChanged();
            }
        }

        public ICommand CloseCommand { get; }
        public ICommand ReplenishCommand { get; }

        public event Action<decimal> BalanceUpdated;
        public ReplenishmentViewModel(Action closeAction, Action<decimal> UpdateBalance)
        {
            Balance = CurrentUser.Instance.Balance;
            CloseCommand = new RelayCommand(_ => closeAction());
            ReplenishCommand = new RelayCommand(_ => ReplenishBalance(), _ => CanReplenishBalance());
            BalanceUpdated += UpdateBalance;
        }

        private bool CanReplenishBalance()
        {
            if(AdditionalSum != 0)
            {
                CanReplenish = true;
                return true;
            }
            else
            {
                CanReplenish = false;
                return false;
            }
        }

        private void ReplenishBalance()
        {
            if (AdditionalSum <= 0) return;

            CurrentUser.Instance.Balance += AdditionalSum;
            Balance = CurrentUser.Instance.Balance;

            AdditionalSum = 0;

            DataWorker.UpdateUser(CurrentUser.Instance);
            BalanceUpdated?.Invoke(Balance);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
