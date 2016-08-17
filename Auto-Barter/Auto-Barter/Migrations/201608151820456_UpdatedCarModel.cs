namespace Auto_Barter.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdatedCarModel : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Cars", "Mileage", c => c.Int(nullable: false));
            AddColumn("dbo.Cars", "Title", c => c.Int(nullable: false));
            AddColumn("dbo.Cars", "Transmission", c => c.Int(nullable: false));
            AddColumn("dbo.Cars", "Drivetype", c => c.Int(nullable: false));
            AddColumn("dbo.Cars", "ExteriorColor", c => c.String(nullable: false));
            AddColumn("dbo.Cars", "InteriorColor", c => c.String());
            DropColumn("dbo.CarPosts", "Title");
            DropColumn("dbo.CarPosts", "Mileage");
            DropColumn("dbo.CarPosts", "ExteriorColor");
            DropColumn("dbo.CarPosts", "InteriorColor");
            DropColumn("dbo.CarPosts", "Transmission");
            DropColumn("dbo.CarPosts", "Drivetype");
        }
        
        public override void Down()
        {
            AddColumn("dbo.CarPosts", "Drivetype", c => c.Int(nullable: false));
            AddColumn("dbo.CarPosts", "Transmission", c => c.Int(nullable: false));
            AddColumn("dbo.CarPosts", "InteriorColor", c => c.String());
            AddColumn("dbo.CarPosts", "ExteriorColor", c => c.String(nullable: false));
            AddColumn("dbo.CarPosts", "Mileage", c => c.Int(nullable: false));
            AddColumn("dbo.CarPosts", "Title", c => c.Int(nullable: false));
            DropColumn("dbo.Cars", "InteriorColor");
            DropColumn("dbo.Cars", "ExteriorColor");
            DropColumn("dbo.Cars", "Drivetype");
            DropColumn("dbo.Cars", "Transmission");
            DropColumn("dbo.Cars", "Title");
            DropColumn("dbo.Cars", "Mileage");
        }
    }
}
