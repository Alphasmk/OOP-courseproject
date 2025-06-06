﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using gamelauncher.ViewModels;

namespace gamelauncher.Views
{
    /// <summary>
    /// Логика взаимодействия для AdminPanelGames.xaml
    /// </summary>
    public partial class AdminPanelGames : Page
    {
        public AdminPanelGamesViewModel ViewModel { get; }
        public AdminPanelGames()
        {
            InitializeComponent();
            ViewModel = new AdminPanelGamesViewModel();
            DataContext = ViewModel;
            ViewModel.RefreshCommand.Execute(null);
        }
    }
}
