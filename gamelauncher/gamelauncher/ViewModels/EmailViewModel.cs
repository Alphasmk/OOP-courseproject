using System;
using System.ComponentModel;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using static gamelauncher.Views.LoginPage;

namespace gamelauncher.ViewModels
{
    public class EmailViewModel : INotifyPropertyChanged
    {
        private string _email;

        public string Email
        {
            get => _email;
            set
            {
                    _email = value;
                    OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}
