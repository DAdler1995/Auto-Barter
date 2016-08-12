namespace Auto_Barter.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedUserRoles : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.UserAccounts", "UserRole", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.UserAccounts", "UserRole");
        }
    }
}
