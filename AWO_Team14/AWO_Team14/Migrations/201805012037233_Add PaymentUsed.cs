namespace AWO_Team14.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddPaymentUsed : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Transactions", "PaymentUsed", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Transactions", "PaymentUsed");
        }
    }
}
