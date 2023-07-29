using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows;

namespace EmployeesManagementApp.Models
{
    public class LoginParams
    {
        public Window Window { get; set; }
        public PasswordBox PasswordBox { get; set; }
        public TextBox LoginTextBox { get; internal set; }
    }
}
