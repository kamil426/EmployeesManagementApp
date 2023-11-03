namespace EmployeesManagementApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddUserAndRoleToDataBase : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserName = c.String(nullable: false, maxLength: 50),
                        Email = c.String(nullable: false, maxLength: 50),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Roles",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        RoleName = c.String(nullable: false, maxLength: 50),
                        UserId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            AddColumn("dbo.Employees", "UserId", c => c.Int(nullable: false));
            AlterColumn("dbo.Employees", "Name", c => c.String(nullable: false, maxLength: 50));
            AlterColumn("dbo.Employees", "LastName", c => c.String(nullable: false, maxLength: 50));
            CreateIndex("dbo.Employees", "UserId");
            AddForeignKey("dbo.Employees", "UserId", "dbo.Users", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Roles", "UserId", "dbo.Users");
            DropForeignKey("dbo.Employees", "UserId", "dbo.Users");
            DropIndex("dbo.Roles", new[] { "UserId" });
            DropIndex("dbo.Employees", new[] { "UserId" });
            AlterColumn("dbo.Employees", "LastName", c => c.String(nullable: false, maxLength: 100));
            AlterColumn("dbo.Employees", "Name", c => c.String(nullable: false, maxLength: 100));
            DropColumn("dbo.Employees", "UserId");
            DropTable("dbo.Roles");
            DropTable("dbo.Users");
        }
    }
}
