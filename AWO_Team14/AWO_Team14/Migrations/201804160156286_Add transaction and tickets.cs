namespace AWO_Team14.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Addtransactionandtickets : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.UserTickets", name: "Transactions_TransactionID", newName: "Transaction_TransactionID");
            RenameIndex(table: "dbo.UserTickets", name: "IX_Transactions_TransactionID", newName: "IX_Transaction_TransactionID");
        }
        
        public override void Down()
        {
            RenameIndex(table: "dbo.UserTickets", name: "IX_Transaction_TransactionID", newName: "IX_Transactions_TransactionID");
            RenameColumn(table: "dbo.UserTickets", name: "Transaction_TransactionID", newName: "Transactions_TransactionID");
        }
    }
}
