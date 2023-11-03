using EmployeesManagementApp.Models.Converters;
using EmployeesManagementApp.Models.Domains;
using EmployeesManagementApp.Models.Wrappers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeesManagementApp.Repository
{
    public class EmployeeRepository
    {
        public void DissmissEmployee(int id)
        {
            using (var context = new ApplicationDBContext())
            {
                var employeeToDissmiss = context.Employees.Single(x => x.Id == id);

                employeeToDissmiss.IsStillEmployed = false;
                employeeToDissmiss.DateOfDissmissed = DateTime.Now;

                context.SaveChanges();
            }
        }

        public void Add(EmployeeWrapper employee, string userName)
        {
            using (var context = new ApplicationDBContext())
            {
                var userId = context.Users.Single(x => x.UserName == userName).Id;
                context.Employees.Add(employee.ToDao(userId));
                context.SaveChanges();
            }
        }

        public void Edit(EmployeeWrapper employee, string userName)
        {
            using (var context = new ApplicationDBContext())
            {
                var userId = context.Users.Single(x => x.UserName == userName).Id;
                var employeeToUpdate = context.Employees.Single(x => x.Id == employee.Id && x.UserId == userId);

                employeeToUpdate.Name = employee.Name;
                employeeToUpdate.LastName = employee.LastName;
                employeeToUpdate.DateOfEmployment = employee.DateOfEmployment;
                employeeToUpdate.Earnings = employee.Earnings;
                employeeToUpdate.Position = employee.Position;
                employeeToUpdate.Comments = employee.Comments;

                context.SaveChanges();
            }
        }

        public IEnumerable<EmployeeWrapper> GetEmployees(string userName)
        {
            using (var context = new ApplicationDBContext())
            {
                var userId = context.Users.Single(x => x.UserName == userName).Id;
                return context.Employees.Where(x => x.UserId == userId)
                    .ToList().Select(x => x.ToWrapper());
            }
        }

        public IEnumerable<EmployeeWrapper> GetEmployees(int filterIsStillEmployed, string userName)
        {
            using (var context = new ApplicationDBContext())
            {
                var userId = context.Users.Single(x => x.UserName == userName).Id;
                var employees = new List<Employee>();

                switch (filterIsStillEmployed)
                {
                    case 0:
                        employees = context.Employees.Where(x => x.UserId == userId).ToList();
                        break;
                    case 1:
                        employees = context.Employees.Where(x => x.IsStillEmployed == true && x.UserId == userId).ToList();
                        break;
                    case 2:
                        employees = context.Employees.Where(x => x.IsStillEmployed == false && x.UserId == userId).ToList();
                        break;
                }
                return employees.Select(x => x.ToWrapper());
            }
        }
    }
}
