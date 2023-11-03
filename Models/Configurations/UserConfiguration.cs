using EmployeesManagementApp.Models.Domains;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeesManagementApp.Models.Configurations
{
    public class UserConfiguration : EntityTypeConfiguration<User>
    {
        public UserConfiguration()
        {
            ToTable("dbo.Users");
            HasKey(x => x.Id);

            Property(x => x.UserName)
                .IsRequired()
                .HasMaxLength(50);

            Property(x => x.Email)
                .IsRequired()
                .HasMaxLength(50);

            Property(x => x.HashedPassword)
                .IsRequired()
                .HasMaxLength(100);
        }
    }
}
