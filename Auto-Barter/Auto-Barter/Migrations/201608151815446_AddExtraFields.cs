namespace Auto_Barter.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddExtraFields : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Cars",
                c => new
                    {
                        CarId = c.Int(nullable: false, identity: true),
                        Make = c.String(),
                        Model = c.String(),
                        Year = c.DateTime(nullable: false),
                        Vin = c.String(),
                        EnteredDateTime = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.CarId);
            
            AddColumn("dbo.CarPosts", "UserCar_CarId", c => c.Int());
            CreateIndex("dbo.CarPosts", "UserCar_CarId");
            AddForeignKey("dbo.CarPosts", "UserCar_CarId", "dbo.Cars", "CarId");
            DropColumn("dbo.CarPosts", "VIN");
            DropColumn("dbo.CarPosts", "Make");
            DropColumn("dbo.CarPosts", "Model");
            DropColumn("dbo.CarPosts", "Year");
        }
        
        public override void Down()
        {
            AddColumn("dbo.CarPosts", "Year", c => c.DateTime(nullable: false));
            AddColumn("dbo.CarPosts", "Model", c => c.String(nullable: false));
            AddColumn("dbo.CarPosts", "Make", c => c.String(nullable: false));
            AddColumn("dbo.CarPosts", "VIN", c => c.String());
            DropForeignKey("dbo.CarPosts", "UserCar_CarId", "dbo.Cars");
            DropIndex("dbo.CarPosts", new[] { "UserCar_CarId" });
            DropColumn("dbo.CarPosts", "UserCar_CarId");
            DropTable("dbo.Cars");
        }
    }
}
