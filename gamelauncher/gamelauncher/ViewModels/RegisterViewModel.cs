using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Input;
using gamelauncher.Model;
using gamelauncher.MVVM;
using gamelauncher.Validation;
using Newtonsoft.Json.Linq;

namespace gamelauncher.ViewModels
{
    public class RegisterViewModel : INotifyPropertyChanged
    {

        public EmailViewModel EmailViewModel { get; }
        public PasswordViewModel PasswordViewModel { get; }

        private string _repeatPassword;
        public string RepeatPassword
        {
            get => _repeatPassword;
            set
            {
                    _repeatPassword = value;
                    OnPropertyChanged();
            }
        }

        public ICommand CreateAccountCommand { get; }

        public RegisterViewModel()
        {
            EmailViewModel = new EmailViewModel();
            PasswordViewModel = new PasswordViewModel();

            EmailViewModel.PropertyChanged += (s, e) =>
            {
                if (e.PropertyName == nameof(EmailViewModel.Email))
                    CommandManager.InvalidateRequerySuggested();
            };

            PasswordViewModel.PropertyChanged += (s, e) =>
            {
                if (e.PropertyName == nameof(PasswordViewModel.Password))
                    CommandManager.InvalidateRequerySuggested();
            };

            CreateAccountCommand = new RelayCommand(
                execute: _ => CreateAccount(),
                canExecute: _ => CanCreateAccount()
            );
        }

        public bool ValidateEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return false;
            string emailPattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";
            if (!Regex.IsMatch(email, emailPattern))
            {
                return false;
            }
            return true;
        }

        public bool ValidatePassword(string password)
        {
            if (string.IsNullOrWhiteSpace(password))
                return false;
            string passwordPattern = @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)[A-Za-z\d]{8,20}$";
            if (!Regex.IsMatch(password, passwordPattern))
            {
                return false;
            }
            return true;
        }
        private bool CanCreateAccount()
        {
            return ValidateEmail(EmailViewModel.Email) &&
                   ValidatePassword(PasswordViewModel.Password) &&
                   PasswordViewModel.Password == RepeatPassword;
        }

        private void CreateAccount()
        {
            DataWorker.CreateUser(EmailViewModel.Email, PasswordViewModel.Password);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            CommandManager.InvalidateRequerySuggested();
        }
    }
}
