using EmployeesManagementApp.Commands;
using EmployeesManagementApp.Models;
using EmployeesManagementApp.Properties;
using EmployeesManagementApp.Views;
using MahApps.Metro.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace EmployeesManagementApp.ViewModels
{
    public class ConfigConnectionWithDbViewModel : ViewModelBase
    {
        private bool _openAppSettingsFromButton;
        public ConfigConnectionWithDbViewModel(bool openAppSettingsFromButton = false)
        {
            ConnectCommand = new RelayCommand(Connect);
            CancelCommand = new RelayCommand(Cancel);
            _openAppSettingsFromButton = openAppSettingsFromButton;
        }
        public RelayCommand ConnectCommand { get; set; }
        public RelayCommand CancelCommand { get; set; }

        public string ServerAdress
        {
            get { return Settings.Default.serverAdress; }
            set 
            { 
                Settings.Default.serverAdress = value;
                OnPropertyChanged();
            }
        }

        public string ServerName
        {
            get { return Settings.Default.serverName; }
            set 
            {
                Settings.Default.serverName = value;
                OnPropertyChanged();
            }
        }

        public string DateBaseName
        {
            get { return Settings.Default.dateBaseName; }
            set 
            {
                Settings.Default.dateBaseName = value;
                OnPropertyChanged();
            }
        }

        public string UserName
        {
            get { return Settings.Default.userName; }
            set { 
                Settings.Default.userName = value;
                OnPropertyChanged();
            }
        }

        public string UserPassword
        {
            get { return Settings.Default.userPassword; }
            set
            { 
                Settings.Default.userPassword = value;
                OnPropertyChanged();
            }
        }

        private void Cancel(object obj)
        {
            var window = obj as Window;
            if (_openAppSettingsFromButton)
                window.Close();
            else
                Application.Current.Shutdown(0);
        }

        private void Connect(object obj)
        {
            var loginParams = obj as LoginParams;
            UserPassword = loginParams.PasswordBox.Password;
            UserName = loginParams.LoginTextBox.Text;
            Settings.Default.Save();
            System.Diagnostics.Process.Start(Application.ResourceAssembly.Location);
            Application.Current.Shutdown(0);
        }
    }
}
