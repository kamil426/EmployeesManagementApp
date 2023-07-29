using EmployeesManagementApp.Models.Domains;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeesManagementApp.Models.Configurations
{
    public class EmployeeConfiguration : EntityTypeConfiguration<Employee>
    {
        public EmployeeConfiguration()
        {
            ToTable("dbo.Employees");
            HasKey(x => x.Id);

            Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(100);

            Property(x => x.LastName)
                .IsRequired()
                .HasMaxLength(100);

            Property(x => x.Earnings)
                .HasPrecision(18, 2);

            Property(x => x.DateOfEmployment)
                .IsRequired();
        }
    }
}
