using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace EmployeesManagementApp.Models.Wrappers
{
    public class EmployeeWrapper : IDataErrorInfo
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public DateTime DateOfEmployment { get; set; }
        public DateTime? DateOfDissmissed { get; set; }
        public string Position { get; set; }
        public decimal Earnings { get; set; }
        public bool IsStillEmployed { get; set; }
        public string Comments { get; set; }

        private bool _isFirstNameValid;
        private bool _isLastNameValid;
        public string this[string columnName]
        {
            get
            {
                switch (columnName)
                {
                    case nameof (Name):
                        if (string.IsNullOrWhiteSpace(Name))
                        {
                            Error = "Pole 'Imię' jest wymagane";
                            _isFirstNameValid = false;
                        }
                        else
                        {
                            Error = string.Empty;
                            _isFirstNameValid = true;
                        }
                        break;
                    case nameof (LastName):
                        if (string.IsNullOrWhiteSpace(LastName))
                        {
                            Error = "Pole 'Nazwisko' jest wymagane";
                            _isLastNameValid = false;
                        }
                        else
                        {
                            Error = string.Empty;
                            _isLastNameValid = true;
                        }
                        break;
                    default:
                        break;
                }
                return Error;
            }
        }
        public bool IsValid
        {
            get
            {
                return _isFirstNameValid && _isLastNameValid;
            }
        }
        public string Error { get; set; }
    }
}
