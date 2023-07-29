using EmployeesManagementApp.Models.Domains;
using EmployeesManagementApp.Models.Wrappers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace EmployeesManagementApp.Models.Converters
{
    public static class EmployeeConverter
    {
        public static EmployeeWrapper ToWrapper(this Employee model)
        {
            return new EmployeeWrapper
            {
                Id = model.Id,
                Name = model.Name,
                LastName = model.LastName,
                Earnings = model.Earnings,
                Comments = model.Comments,
                Position = model.Position,
                DateOfDissmissed = model.DateOfDissmissed,
                DateOfEmployment = model.DateOfEmployment,
                IsStillEmployed = model.IsStillEmployed
            };
        }
        public static Employee ToDao(this EmployeeWrapper model)
        {
            return new Employee
            {
                Id = model.Id,
                Name = model.Name,
                LastName = model.LastName,
                Earnings = model.Earnings,
                Comments = model.Comments,
                Position = model.Position,
                DateOfEmployment = model.DateOfEmployment,
                DateOfDissmissed = model.DateOfDissmissed,
                IsStillEmployed = model.IsStillEmployed
            };
        }
    }
}
