﻿using EmployeesManagementApp.ViewModels;
using MahApps.Metro.Controls;

namespace EmployeesManagementApp.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new MainWindowViewModel();
        }
    }
}
