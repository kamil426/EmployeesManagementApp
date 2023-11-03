using EmployeesManagementApp.Models.Domains;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeesManagementApp.Models.Configurations
{
    public class RoleConfiguration : EntityTypeConfiguration<Role>
    {
        public RoleConfiguration()
        {
            ToTable("dbo.Roles");
            HasKey(x => x.Id);

            Property(x => x.RoleName)
                .IsRequired()
                .HasMaxLength(50);

            Property(x => x.UserId)
                .IsRequired();
        }
    }
}
