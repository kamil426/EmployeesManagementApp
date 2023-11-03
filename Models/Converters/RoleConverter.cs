using EmployeesManagementApp.Models.Domains;
using EmployeesManagementApp.Models.Wrappers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Security;

namespace EmployeesManagementApp.Models.Converters
{
    public static class RoleConverter
    {
        public static string[] ToArrayRole(this IEnumerable<Role> model)
        {
            var listOfRoles = new List<string>();
                
            foreach (var role in model)
                listOfRoles.Add(role.RoleName);

            return listOfRoles.ToArray();
        }
    }
}
