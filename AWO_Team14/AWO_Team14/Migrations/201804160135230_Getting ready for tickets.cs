namespace AWO_Team14.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Gettingreadyfortickets : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.UserTickets",
                c => new
                    {
                        UserTicketID = c.Int(nullable: false, identity: true),
                        CurrentPrice = c.Decimal(nullable: false, precision: 18, scale: 2),
                        SeatNumber = c.String(nullable: false),
                        Current = c.Boolean(nullable: false),
                        Showing_ShowingID = c.Int(),
                        Transactions_TransactionID = c.Int(),
                    })
                .PrimaryKey(t => t.UserTicketID)
                .ForeignKey("dbo.Showings", t => t.Showing_ShowingID)
                .ForeignKey("dbo.Transactions", t => t.Transactions_TransactionID)
                .Index(t => t.Showing_ShowingID)
                .Index(t => t.Transactions_TransactionID);
            
            CreateTable(
                "dbo.Transactions",
                c => new
                    {
                        TransactionID = c.Int(nullable: false, identity: true),
                        Payment = c.Int(nullable: false),
                        TransactionDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.TransactionID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.UserTickets", "Transactions_TransactionID", "dbo.Transactions");
            DropForeignKey("dbo.UserTickets", "Showing_ShowingID", "dbo.Showings");
            DropIndex("dbo.UserTickets", new[] { "Transactions_TransactionID" });
            DropIndex("dbo.UserTickets", new[] { "Showing_ShowingID" });
            DropTable("dbo.Transactions");
            DropTable("dbo.UserTickets");
        }
    }
}
