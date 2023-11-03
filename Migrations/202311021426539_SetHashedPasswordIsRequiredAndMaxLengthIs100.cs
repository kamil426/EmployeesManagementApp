namespace EmployeesManagementApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SetHashedPasswordIsRequiredAndMaxLengthIs100 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Users", "HashedPassword", c => c.String(nullable: false, maxLength: 100));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Users", "HashedPassword", c => c.String());
        }
    }
}
