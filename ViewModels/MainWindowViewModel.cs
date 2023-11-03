using EmployeesManagementApp.Commands;
using EmployeesManagementApp.Models;
using EmployeesManagementApp.Models.Converters;
using EmployeesManagementApp.Models.Domains;
using EmployeesManagementApp.Models.Identity;
using EmployeesManagementApp.Models.Wrappers;
using EmployeesManagementApp.Properties;
using EmployeesManagementApp.Repository;
using EmployeesManagementApp.Views;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace EmployeesManagementApp.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        private EmployeeRepository _employeeRepository = new EmployeeRepository();
        private UserRepository _userRepository = new UserRepository();
        public MainWindowViewModel()
        {
            AddEmployeeCommand = new RelayCommand(AddEditEmployee);
            EditEmployeeCommand = new RelayCommand(AddEditEmployee, CanEditEmployee);
            DissmissEmployeeCommand = new AsyncRelayCommand(DissmissEmployee, CanEditEmployee);
            AppSettingsCommand = new RelayCommand(OpenAppSettings);
            FilterChangedCommand = new RelayCommand(FilterChanged);
            LoginCommand = new RelayCommand(Login);
            
            LoginAndCheckConnection();
        }

        public RelayCommand AddEmployeeCommand { get; set; }
        public RelayCommand EditEmployeeCommand { get; set; }
        public AsyncRelayCommand DissmissEmployeeCommand { get; set; }
        public RelayCommand AppSettingsCommand { get; set; }
        public RelayCommand FilterChangedCommand { get; set; }
        public RelayCommand LoginCommand { get; set; }

        private int _filterIsStillEmployed;

        public int FilterIsStillEmployed
        {
            get { return _filterIsStillEmployed; }
            set
            {
                _filterIsStillEmployed = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<Filter> _filters;

        public ObservableCollection<Filter> Filters
        {
            get { return _filters; }
            set 
            {
                _filters = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<EmployeeWrapper> _employees;

        public ObservableCollection<EmployeeWrapper> Employees
        {
            get { return _employees; }
            set 
            {
                _employees = value;
                OnPropertyChanged();
            }
        }

        private EmployeeWrapper _selectedEmployee;

        public EmployeeWrapper SelectedEmployee
        {
            get { return _selectedEmployee; }
            set
            {
                _selectedEmployee = value;
                OnPropertyChanged();
            }
        }

        public bool IsAuthenticated
        {
            get { return Thread.CurrentPrincipal.Identity.IsAuthenticated; }
        }

        private void Login(object obj)
        {
            var openLoginWindowFromButton = true;
            var loginWindow = new LoginView(openLoginWindowFromButton);
            loginWindow.ShowDialog();
            RefreshDiary();
            OnPropertyChanged("IsAuthenticated");
        }

        private void FilterChanged(object obj)
        {
            var userName = Thread.CurrentPrincipal.Identity.Name;

            Employees = new ObservableCollection<EmployeeWrapper>(_employeeRepository.GetEmployees(FilterIsStillEmployed, userName));
        }

        public async void LoginAndCheckConnection()
        {
            using (var context = new ApplicationDBContext())
            {
                try
                {
                    context.Database.Connection.Open();
                    context.Database.Connection.Close();
                }
                catch (Exception ex)
                {
                    var mainWindow = Application.Current.MainWindow as MetroWindow;
                    var messageBoxResult = await mainWindow.ShowMessageAsync(
                        "Nieprawidłowe dane",
                        "Nie można połączyć się z bazą danych." +
                        $" Czy chcesz wprowadzić dane do nawiązania połączenia z bazą danych? " +
                        $"Kliknięcie 'Anuluj' spowoduje zamknięcie aplikacji.\n\n" +
                        $"Kod błędu:\n{ex.Message}", MessageDialogStyle.AffirmativeAndNegative);

                    if (messageBoxResult == MessageDialogResult.Affirmative)
                    {
                        var configConnectionWithDB = new ConfigConnectionWithDBView();
                        configConnectionWithDB.ShowDialog();
                    }
                    else if (messageBoxResult == MessageDialogResult.Negative)
                        Application.Current.Shutdown(0);
                }
            }

            if(!string.IsNullOrWhiteSpace(Settings.Default.rememberUserName)
                && !string.IsNullOrWhiteSpace(Settings.Default.rememberUserPassword))
                LoginRememberUser(Settings.Default.rememberUserName, Settings.Default.rememberUserPassword);
            else
            {
                var loginWindow = new LoginView();
                loginWindow.ShowDialog();
            }
            RefreshDiary();
            InitializeFilters();
        }

        private void OpenAppSettings(object obj)
        {
            var openAppSettingsFromButton = true;
            var window = new ConfigConnectionWithDBView(openAppSettingsFromButton);
            window.ShowDialog();
        }

        private async Task DissmissEmployee(object obj)
        {
            var metroWindow = obj as MetroWindow;
            MessageDialogResult dialog;

            if (!SelectedEmployee.IsStillEmployed)
            {
                dialog = await metroWindow.ShowMessageAsync("Zwalnianie pracownika", "Dany pracownik został już zwolniony!");
                return;
            }

            dialog = await metroWindow.ShowMessageAsync("Zwalnianie pracownika", $"Czy na pewno chcesz zwolnić pracownika " +
                $"{SelectedEmployee.Name} {SelectedEmployee.LastName}?", MessageDialogStyle.AffirmativeAndNegative);

            if (dialog != MessageDialogResult.Affirmative)
                return;

            _employeeRepository.DissmissEmployee(_selectedEmployee.Id);

            RefreshDiary();
        }

        private void AddEditEmployee(object obj)
        {
            var addEditEmployeeWindow = new AddEditEmployeeView(obj as EmployeeWrapper);
            addEditEmployeeWindow.ShowDialog();
            RefreshDiary();
        }

        private void InitializeFilters()
        {
            _filters = new ObservableCollection<Filter>()
            {
                new Filter() { Id = 0, SelectedFilter = "-- Wszyscy --"},
                new Filter() { Id = 1, SelectedFilter = "Zatrudnieni"},
                new Filter() { Id = 2, SelectedFilter = "Zwolnieni"}
            };
        }

        private void RefreshDiary()
        {
            var userName = Thread.CurrentPrincipal.Identity.Name;
            if (!string.IsNullOrWhiteSpace(userName))
                Employees = new ObservableCollection<EmployeeWrapper>(_employeeRepository.GetEmployees(userName));
            else
                Employees = new ObservableCollection<EmployeeWrapper>();
        }

        public void LoginRememberUser(string rememberUserName, string rememberUserPassword)
        {
            var user = _userRepository.GetRememberUser(rememberUserName, rememberUserPassword);

            CustomPrincipal customPrincipal = Thread.CurrentPrincipal as CustomPrincipal;
            if (customPrincipal == null)
                throw new ArgumentException("The application's default thread principal must be set to a CustomPrincipal object on startup.");

            customPrincipal.Identity = new CustomIdentity(user.UserName, user.Email, user.Roles.ToArrayRole());
        }

        private bool CanEditEmployee(object obj)
        {
            return SelectedEmployee != null;
        }

    }
}
