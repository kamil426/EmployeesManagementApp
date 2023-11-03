namespace EmployeesManagementApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddHashedPasswordToUser : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Users", "HashedPassword", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Users", "HashedPassword");
        }
    }
}
