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

        public void Add(EmployeeWrapper employee)
        {
            using (var context = new ApplicationDBContext())
            {
                context.Employees.Add(employee.ToDao());
                context.SaveChanges();
            }
        }

        public void Edit(EmployeeWrapper employee)
        {
            using (var context = new ApplicationDBContext())
            {
                var employeeToUpdate = context.Employees.Single(x => x.Id == employee.Id);

                employeeToUpdate.Name = employee.Name;
                employeeToUpdate.LastName = employee.LastName;
                employeeToUpdate.DateOfEmployment = employee.DateOfEmployment;
                employeeToUpdate.Earnings = employee.Earnings;
                employeeToUpdate.Position = employee.Position;
                employeeToUpdate.Comments = employee.Comments;

                context.SaveChanges();
            }
        }

        public IEnumerable<EmployeeWrapper> GetEmployees()
        {
            using (var context = new ApplicationDBContext())
            {
                return context.Employees.ToList().Select(x => x.ToWrapper());
            }
        }

        public IEnumerable<EmployeeWrapper> GetEmployees(int filterIsStillEmployed)
        {
            using (var context = new ApplicationDBContext())
            {
                var employees = new List<Employee>();

                switch (filterIsStillEmployed)
                {
                    case 0:
                        employees = context.Employees.ToList();
                        break;
                    case 1:
                        employees = context.Employees.Where(x => x.IsStillEmployed == true).ToList();
                        break;
                    case 2:
                        employees = context.Employees.Where(x => x.IsStillEmployed == false).ToList();
                        break;
                }
                return employees.Select(x => x.ToWrapper());
            }
        }
    }
}
