namespace AWO_Team14.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class seedusers : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        UserID = c.Int(nullable: false, identity: true),
                        UserType = c.Int(nullable: false),
                        FirstName = c.String(nullable: false),
                        MiddleInitial = c.String(),
                        LastName = c.String(nullable: false),
                        Email = c.String(nullable: false),
                        Street = c.String(),
                        City = c.String(),
                        State = c.Int(nullable: false),
                        Zip = c.String(maxLength: 5),
                        Birthday = c.DateTime(nullable: false),
                        PhoneNumer = c.String(nullable: false),
                        Password = c.String(nullable: false),
                        SSN = c.String(),
                        CreditCardNumber = c.String(),
                        PopcornPoints = c.Int(nullable: false),
                        Archived = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.UserID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Users");
        }
    }
}
