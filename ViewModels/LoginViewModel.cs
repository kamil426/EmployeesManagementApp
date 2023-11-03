using EmployeesManagementApp.Commands;
using EmployeesManagementApp.Models;
using EmployeesManagementApp.Models.Converters;
using EmployeesManagementApp.Models.Domains;
using EmployeesManagementApp.Models.Identity;
using EmployeesManagementApp.Properties;
using EmployeesManagementApp.Repository;
using EmployeesManagementApp.Views;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.ApplicationServices;
using System.Web.UI.WebControls;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace EmployeesManagementApp.ViewModels
{
    public class LoginViewModel : ViewModelBase
    {
        private bool _openLoginWindowFromButton;
        private UserRepository _userRepository = new UserRepository();

        public LoginViewModel(bool openLoginWindowFromButton)
        {
            _openLoginWindowFromButton = openLoginWindowFromButton;
            LoginCommand = new AsyncRelayCommand(Login, CanLogin);
            LogoutCommand = new RelayCommand(Logout, CanLogout);
            CancelCommand = new RelayCommand(Cancel);
            _userRepository.Register2Users();
        }

        public AsyncRelayCommand LoginCommand { get; set; }
        public RelayCommand CancelCommand { get; set; }
        public RelayCommand LogoutCommand { get; set; }

        public string AuthenticatedUser
        {
            get
            {
                if (IsAuthenticated)
                    return string.Format("Zalogowany jako {0}. {1}",
                          Thread.CurrentPrincipal.Identity.Name,
                          Thread.CurrentPrincipal.IsInRole("Administrator") ? "Jesteś administratorem!"
                              : "Nie jesteś administratorem.");

                return "Jesteś nie zalogowany!";
            }
        }

        public bool IsAuthenticated
        {
            get { return Thread.CurrentPrincipal.Identity.IsAuthenticated; }
        }

        public bool IsNotAuthenticated
        {
            get { return !Thread.CurrentPrincipal.Identity.IsAuthenticated; }
        }

        private bool _rememberMe;

        public bool RememberMe
        {
            get { return _rememberMe; }
            set
            {
                _rememberMe = value;
                OnPropertyChanged();
            }
        }

        private async Task Login(object obj)
        {
            var loginParams = obj as LoginParams;
            var userName = loginParams.LoginTextBox.Text;
            var userPassword = loginParams.PasswordBox.Password;

            try
            {
                User user = AuthenticateUser(userName, userPassword);

                CustomPrincipal customPrincipal = Thread.CurrentPrincipal as CustomPrincipal;
                if (customPrincipal == null)
                    throw new ArgumentException("The application's default thread principal must be set to a CustomPrincipal object on startup.");

                customPrincipal.Identity = new CustomIdentity(user.UserName, user.Email, user.Roles.ToArrayRole());

                UpdateUi(loginParams);

                if (RememberMe)
                {
                    Settings.Default.rememberUserName = user.UserName;
                    Settings.Default.rememberUserPassword = user.HashedPassword;
                    Settings.Default.Save();
                }

                var window = loginParams.Window;
                if (!_openLoginWindowFromButton)
                    window.Close();
            }
            catch (UnauthorizedAccessException)
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

        private void Logout(object obj)
        {
            var loginParams = obj as LoginParams;

            CustomPrincipal customPrincipal = Thread.CurrentPrincipal as CustomPrincipal;
            if (customPrincipal != null)
            {
                Settings.Default.rememberUserName = "";
                Settings.Default.rememberUserPassword = "";
                Settings.Default.Save();
                customPrincipal.Identity = new AnonymousIdentity();
                UpdateUi(loginParams);
            }
        }

        private void Cancel(object obj)
        {
            var window = obj as Window;
            if (_openLoginWindowFromButton)
                window.Close();
            else
                Application.Current.Shutdown(0);
        }

        private bool CanLogin(object parameter)
        {
            return IsNotAuthenticated;
        }

        private bool CanLogout(object parameter)
        {
            return IsAuthenticated;
        }

        private void UpdateUi(LoginParams loginParams)
        {
            OnPropertyChanged("AuthenticatedUser");
            OnPropertyChanged("IsAuthenticated");
            OnPropertyChanged("IsNotAuthenticated");

            loginParams.LoginTextBox.Text = "";
            loginParams.PasswordBox.Password = "";
        }

        private User AuthenticateUser(string userName, string userPassword)
        {
            var user = _userRepository.GetUser(userName, userPassword);

            if (user == null)
                throw new UnauthorizedAccessException("Access denied. Please provide some valid credentials.");

            return user;
        }
    }
}
