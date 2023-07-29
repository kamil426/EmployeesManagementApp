using EmployeesManagementApp.Commands;
using EmployeesManagementApp.Models;
using EmployeesManagementApp.Properties;
using EmployeesManagementApp.Views;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace EmployeesManagementApp.ViewModels
{
    public class LoginViewModel : ViewModelBase
    {
        public LoginViewModel()
        {
            OkCommand = new AsyncRelayCommand(Ok);
            CancelCommand = new RelayCommand(Cancel);
        }

        public ICommand OkCommand { get; set; }
        public ICommand CancelCommand { get; set; }

        public bool RememberMe
        {
            get { return Settings.Default.rememberMe; }
            set
            {
                Settings.Default.rememberMe = value;
                OnPropertyChanged();
            }
        }

        private async Task Ok (object obj)
        {
            var loginParams = obj as LoginParams;
            var userName = loginParams.LoginTextBox.Text;
            var userPassword = loginParams.PasswordBox.Password;

            if (userName == "admin" && userPassword == "a")
            {
                Settings.Default.currentUser = userName;
                Settings.Default.Save();
                var window = loginParams.Window;
                window.Close();
            }
            else
            {
                var settings = new MetroDialogSettings()
                { 
                    DialogMessageFontSize = 12,
                    DialogTitleFontSize = 16,
                    DialogButtonFontSize = 16
                };
                var loginWindow = loginParams.Window as MetroWindow;
                _ = await loginWindow.ShowMessageAsync
                    ("Nieprawidłowe dane",
                    "Wpisane dane logowania są nieprawidłowe",
                    MessageDialogStyle.Affirmative, settings);
            }
        }
        private void Cancel(object obj)
        {
            var window = obj as Window;
            window.Close();
        }
    }
}
