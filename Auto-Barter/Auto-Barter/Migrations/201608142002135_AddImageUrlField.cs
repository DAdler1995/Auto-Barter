namespace Auto_Barter.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddImageUrlField : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.CarPosts", "ImageUrl", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.CarPosts", "ImageUrl");
        }
    }
}
