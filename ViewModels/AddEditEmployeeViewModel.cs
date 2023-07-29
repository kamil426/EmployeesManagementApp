using EmployeesManagementApp.Commands;
using EmployeesManagementApp.Models.Domains;
using EmployeesManagementApp.Models.Wrappers;
using EmployeesManagementApp.Repository;
using EmployeesManagementApp.Views.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace EmployeesManagementApp.ViewModels
{
    public class AddEditEmployeeViewModel : ViewModelBase
    {
        private EmployeeRepository _employeeRepository = new EmployeeRepository();
        public AddEditEmployeeViewModel(EmployeeWrapper employee = null)
        {
            CloseCommand = new RelayCommand(Close);
            ConfirmCommand = new RelayCommand(Confirm);

            if (employee == null)
            {
                Employee = new EmployeeWrapper()
                {
                    IsStillEmployed = true,
                    DateOfEmployment = DateTime.Now
                };
            }
            else
            {
                IsUpdate = true;
                Employee = employee;
            }
            SetValueIsStillEmployed();
        }

        private string _isStillEmployed;

        public string IsStillEmployed
        {
            get { return _isStillEmployed; }
            set 
            { 
                _isStillEmployed = value;
                OnPropertyChanged();

            }
        }

        private bool _isUpdate;

        public bool IsUpdate
        {
            get { return _isUpdate; }
            set
            {
                _isUpdate = value;
                OnPropertyChanged();
            }
        }

        private EmployeeWrapper _employee;

        public EmployeeWrapper Employee
        {
            get { return _employee; }
            set 
            {
                _employee = value;
                OnPropertyChanged();
            }
        }
        private void SetValueIsStillEmployed()
        {
            if (Employee.IsStillEmployed)
            {
                IsStillEmployed = "TAK";
            }
            else
            {
                IsStillEmployed = "NIE";
            }  
        }
        private void Confirm(object obj)
        {
            if (!Employee.IsValid)
                return;

            if (!IsUpdate)
                _employeeRepository.Add(Employee);
            else
                _employeeRepository.Edit(Employee);

            CloseWindow(obj);
        }

        private void Close(object obj)
        {
            CloseWindow(obj);
        }

        private void CloseWindow(object obj)
        {
            var window = obj as Window;
            window.Close();
        }

        public ICommand CloseCommand { get; set; }
        public ICommand ConfirmCommand { get; set; }
    }
}
