namespace AWO_Team14.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updatetransactionmodel : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Transactions", "Giftee_Id", c => c.String(maxLength: 128));
            CreateIndex("dbo.Transactions", "Giftee_Id");
            AddForeignKey("dbo.Transactions", "Giftee_Id", "dbo.AspNetUsers", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Transactions", "Giftee_Id", "dbo.AspNetUsers");
            DropIndex("dbo.Transactions", new[] { "Giftee_Id" });
            DropColumn("dbo.Transactions", "Giftee_Id");
        }
    }
}
