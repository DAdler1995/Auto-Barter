namespace Auto_Barter.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangedTitleToRepresentCarTitleStatus : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.CarPosts", "Title", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.CarPosts", "Title", c => c.String(nullable: false));
        }
    }
}
