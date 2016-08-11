namespace Auto_Barter.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CreatedAccountDetails : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Addresses",
                c => new
                    {
                        AddressId = c.Int(nullable: false, identity: true),
                        Street1 = c.String(),
                        Street2 = c.String(),
                        City = c.String(),
                        State = c.String(),
                        PostalCode = c.Int(nullable: false),
                        Country = c.String(),
                    })
                .PrimaryKey(t => t.AddressId);
            
            CreateTable(
                "dbo.UserDetails",
                c => new
                    {
                        UserDetailsId = c.Int(nullable: false, identity: true),
                        PhoneNumber = c.Int(nullable: false),
                        Address_AddressId = c.Int(),
                        UserAccount_UserId = c.Int(),
                    })
                .PrimaryKey(t => t.UserDetailsId)
                .ForeignKey("dbo.Addresses", t => t.Address_AddressId)
                .ForeignKey("dbo.UserAccounts", t => t.UserAccount_UserId)
                .Index(t => t.Address_AddressId)
                .Index(t => t.UserAccount_UserId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.UserDetails", "UserAccount_UserId", "dbo.UserAccounts");
            DropForeignKey("dbo.UserDetails", "Address_AddressId", "dbo.Addresses");
            DropIndex("dbo.UserDetails", new[] { "UserAccount_UserId" });
            DropIndex("dbo.UserDetails", new[] { "Address_AddressId" });
            DropTable("dbo.UserDetails");
            DropTable("dbo.Addresses");
        }
    }
}
