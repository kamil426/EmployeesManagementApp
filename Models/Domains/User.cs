using EmployeesManagementApp.Models.Domains;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeesManagementApp.Models.Domains
{
    public class User
    {
        public User()
        {
            Employees = new Collection<Employee>();
            Roles = new Collection<Role>();
        }
        public int Id { get; set; }
        public string UserName {get; set;}
        public string HashedPassword { get;  set; }
        public string Email {get; set;}
        public ICollection<Role> Roles { get; set; }
        public ICollection<Employee> Employees { get; set; }
    }
}
