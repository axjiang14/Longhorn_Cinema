namespace AWO_Team14.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Changedtransactionusersmodel : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Transactions", "Total");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Transactions", "Total", c => c.Decimal(nullable: false, precision: 18, scale: 2));
        }
    }
}
