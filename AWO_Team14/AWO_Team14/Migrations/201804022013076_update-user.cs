namespace AWO_Team14.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateuser : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Users", "CreditCardNumber");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Users", "CreditCardNumber", c => c.String());
        }
    }
}
