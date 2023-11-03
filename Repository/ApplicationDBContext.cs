using EmployeesManagementApp.Models.Configurations;
using EmployeesManagementApp.Models.Domains;
using EmployeesManagementApp.Properties;
using System;
using System.Data.Entity;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Media;

namespace EmployeesManagementApp.Repository
{
    public class ApplicationDBContext : DbContext
    {
        private static string _connectionString =
                $"Server = {Settings.Default.serverAdress}" +
                $@"\{Settings.Default.serverName};" +
                $" Database = {Settings.Default.dateBaseName};" +
                $" User Id = {Settings.Default.userName};" +
                $" Password = {Settings.Default.userPassword};";

        public ApplicationDBContext()
            : base(_connectionString)
        {
        }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new EmployeeConfiguration());
            modelBuilder.Configurations.Add(new UserConfiguration());
            modelBuilder.Configurations.Add(new RoleConfiguration());
        }
    }
}