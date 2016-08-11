namespace Auto_Barter.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ReaddedEmailAddress : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.UserAccounts", "EmailAddress", c => c.String(nullable: false, defaultValue: ""));
        }
        
        public override void Down()
        {
        }
    }
}
