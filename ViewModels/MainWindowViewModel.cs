using EmployeesManagementApp.Commands;
using EmployeesManagementApp.Models;
using EmployeesManagementApp.Models.Domains;
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
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace EmployeesManagementApp.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        private EmployeeRepository _employeeRepository = new EmployeeRepository();
        public MainWindowViewModel()
        {
            AddEmployeeCommand = new RelayCommand(AddEditEmployee);
            EditEmployeeCommand = new RelayCommand(AddEditEmployee, CanEditEmployee);
            DissmissEmployeeCommand = new AsyncRelayCommand(DissmissEmployee, CanEditEmployee);
            AppSettingsCommand = new RelayCommand(OpenAppSettings);
            FilterChangedCommand = new RelayCommand(FilterChanged);
            LoginCommand = new RelayCommand(Login);
            MainWindowClosedCommand = new RelayCommand(MainWindowClosed);

            CheckConnection();
        }

        public ICommand AddEmployeeCommand { get; set; }
        public ICommand EditEmployeeCommand { get; set; }
        public ICommand DissmissEmployeeCommand { get; set; }
        public ICommand AppSettingsCommand { get; set; }
        public ICommand FilterChangedCommand { get; set; }
        public ICommand LoginCommand { get; set; }
        public ICommand MainWindowClosedCommand { get; set; }

        private string _contentButtonLogin;

        public string ContentButtonLogin
        {
            get { return _contentButtonLogin; }
            set 
            {
                _contentButtonLogin = value;
                OnPropertyChanged();
            }
        }

        private string _headerText;

        public string HeaderText
        {
            get { return _headerText; }
            set 
            {
                _headerText = value;
                OnPropertyChanged();
            }
        }

        private bool _canAddData;

        public bool CanAddData
        {
            get { return _canAddData; }
            set 
            {
                _canAddData = value;
                OnPropertyChanged();
            }
        }


        private bool _canDisplayData;

        public bool CanDisplayData
        {
            get { return _canDisplayData; }
            set 
            {
                _canDisplayData = value; 
                OnPropertyChanged();
            }
        }

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

        private void MainWindowClosed(object obj)
        {
            if(!Settings.Default.rememberMe)
            {
                Settings.Default.currentUser = null;
                Settings.Default.Save();
            }
        }

        private void CheckDataAndUser()
        {
            if (!string.IsNullOrWhiteSpace(Settings.Default.currentUser))
            {
                CanAddData = true;
                ContentButtonLogin = "Wyloguj się";

                if (!Employees.Any())
                {
                    HeaderText = "Brak danych!";
                    return;
                }
                CanDisplayData = true;
            }
            else
            {
                CanDisplayData= false;
                CanAddData= false;
                ContentButtonLogin = "Logowanie";
                HeaderText = "Zaloguj się aby móc uzyskać dostęp do danych";
            }
        }

        private void Login(object obj)
        {
            if(ContentButtonLogin.StartsWith("L"))
            {
                var loginWindow = new LoginView();
                loginWindow.ShowDialog();
            }
            else
            {
                Settings.Default.currentUser = null;
                Settings.Default.rememberMe = false;
                Settings.Default.Save();
            }
            CheckDataAndUser();
        }

        private void FilterChanged(object obj)
        {
            Employees = new ObservableCollection<EmployeeWrapper>(_employeeRepository.GetEmployees(FilterIsStillEmployed));
        }

        public async void CheckConnection()
        {
            using (var context = new ApplicationDBContext())
            {
                try
                {
                    context.Database.Connection.Open();
                    context.Database.Connection.Close();
                    RefreshDiary();
                    InitializeFilters();
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
                finally
                {
                    CheckDataAndUser();
                }
            }
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
            CheckDataAndUser();
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
            Employees = new ObservableCollection<EmployeeWrapper>(_employeeRepository.GetEmployees());
        }
        private bool CanEditEmployee(object obj)
        {
            return SelectedEmployee != null && !string.IsNullOrWhiteSpace(Settings.Default.currentUser);
        }

    }
}
