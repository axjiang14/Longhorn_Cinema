namespace AWO_Team14.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Addtransactiontotal : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Transactions", "Total", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            DropColumn("dbo.AspNetUsers", "MiddleInitial");
            DropColumn("dbo.AspNetUsers", "Street");
            DropColumn("dbo.AspNetUsers", "City");
            DropColumn("dbo.AspNetUsers", "State");
            DropColumn("dbo.AspNetUsers", "Zip");
            DropColumn("dbo.AspNetUsers", "Birthday");
            DropColumn("dbo.AspNetUsers", "CreditCardNumber");
            DropColumn("dbo.AspNetUsers", "PopcornPoints");
            DropColumn("dbo.AspNetUsers", "Archived");
        }
        
        public override void Down()
        {
            AddColumn("dbo.AspNetUsers", "Archived", c => c.Boolean(nullable: false));
            AddColumn("dbo.AspNetUsers", "PopcornPoints", c => c.Int(nullable: false));
            AddColumn("dbo.AspNetUsers", "CreditCardNumber", c => c.String());
            AddColumn("dbo.AspNetUsers", "Birthday", c => c.DateTime(nullable: false));
            AddColumn("dbo.AspNetUsers", "Zip", c => c.String(maxLength: 5));
            AddColumn("dbo.AspNetUsers", "State", c => c.Int(nullable: false));
            AddColumn("dbo.AspNetUsers", "City", c => c.String());
            AddColumn("dbo.AspNetUsers", "Street", c => c.String());
            AddColumn("dbo.AspNetUsers", "MiddleInitial", c => c.String());
            DropColumn("dbo.Transactions", "Total");
        }
    }
}
