namespace Auto_Barter.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CreateCarPostTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CarPosts",
                c => new
                    {
                        PostId = c.Int(nullable: false, identity: true),
                        Title = c.String(nullable: false),
                        Mileage = c.Int(nullable: false),
                        ExteriorColor = c.String(nullable: false),
                        InteriorColor = c.String(),
                        Transmission = c.Int(nullable: false),
                        Drivetype = c.Int(nullable: false),
                        VIN = c.String(),
                        Details = c.String(nullable: false),
                        Make = c.String(nullable: false),
                        Model = c.String(nullable: false),
                        Year = c.DateTime(nullable: false),
                        PostDate = c.DateTime(nullable: false),
                        UserDetails_UserDetailsId = c.Int(),
                    })
                .PrimaryKey(t => t.PostId)
                .ForeignKey("dbo.UserDetails", t => t.UserDetails_UserDetailsId)
                .Index(t => t.UserDetails_UserDetailsId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.CarPosts", "UserDetails_UserDetailsId", "dbo.UserDetails");
            DropIndex("dbo.CarPosts", new[] { "UserDetails_UserDetailsId" });
            DropTable("dbo.CarPosts");
        }
    }
}
