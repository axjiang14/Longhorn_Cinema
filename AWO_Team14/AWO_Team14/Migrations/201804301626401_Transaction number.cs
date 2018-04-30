namespace AWO_Team14.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Transactionnumber : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Transactions", "TransactionNumber", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Transactions", "TransactionNumber");
        }
    }
}
