using EmployeesManagementApp.Models.Domains;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace EmployeesManagementApp.Repository
{
    public class UserRepository
    {
        public User GetUser(string userName, string userPassword)
        {
            using (var context = new ApplicationDBContext())
            {
                var user = context.Users.ToList().SingleOrDefault(x => x.UserName.Equals(userName)
                    && x.HashedPassword.Equals(CalculateHash(userPassword, "142C35C5-37BC-4EFE-B425-B8F590AB9E36")));

                if(user != null)
                    user.Roles = context.Roles.Where(x => x.UserId == user.Id).ToList();

                return user;
            }
        }

        private string CalculateHash(string userPassword, string salt)
        {
            byte[] saltedHashBytes = Encoding.UTF8.GetBytes(userPassword + salt);
            HashAlgorithm algorithm = new SHA256Managed();
            byte[] hash = algorithm.ComputeHash(saltedHashBytes);
            return Convert.ToBase64String(hash);
        }

        public User GetRememberUser(string rememberUserName, string rememberUserPassword)
        {
            using (var context = new ApplicationDBContext())
            {
                var user = context.Users.
                    SingleOrDefault(x => x.UserName == rememberUserName && x.HashedPassword == rememberUserPassword);

                if (user != null)
                    user.Roles = context.Roles.Where(x => x.UserId == user.Id).ToList();

                return user;
            }
        }

        public void Register2Users()
        {
            using (var context = new ApplicationDBContext())
            {
                var users = context.Users.ToList();
                if (!users.Any())
                {
                    var user1 = new User()
                    {
                        UserName = "Mark",
                        Email = "mark@company.com",
                        HashedPassword = CalculateHash("Mark", "142C35C5-37BC-4EFE-B425-B8F590AB9E36")
                    };
                    var user2 = new User()
                    {
                        UserName = "John",
                        Email = "john@company.com",
                        HashedPassword = CalculateHash("John", "142C35C5-37BC-4EFE-B425-B8F590AB9E36")
                    };
                    context.Users.Add(user1);
                    context.Users.Add(user2);
                    context.SaveChanges();
                    context.Roles.Add(new Role()
                    {
                        RoleName = "Administrator",
                        UserId = user1.Id,
                    });
                    context.SaveChanges();
                }
            }
        }

        //new InternalUserData("Mark", "mark@company.com",
        //"MB5PYIsbI2YzCUe34Q5ZU2VferIoI4Ttd+ydolWV0OE=", new string[] { "Administrators" }),//hasło: Mark
        //new InternalUserData("John", "c",
        //"hMaLizwzOQ5LeOnMuj+C6W75Zl5CXXYbwDSHWW9ZOXc=", new string[] { })//hasło: John
    }
}
