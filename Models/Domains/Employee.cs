using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace EmployeesManagementApp.Models.Domains
{
    public class Employee
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
    }
}
