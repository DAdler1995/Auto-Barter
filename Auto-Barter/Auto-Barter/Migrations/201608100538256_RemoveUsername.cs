namespace Auto_Barter.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemoveEmailAddress : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.UserAccounts", "EmailAddress");
        }
        
        public override void Down()
        {
            AddColumn("dbo.UserAccounts", "EmailAddress", c => c.String(nullable: false));
        }
    }
}
