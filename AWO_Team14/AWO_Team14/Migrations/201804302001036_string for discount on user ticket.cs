namespace AWO_Team14.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class stringfordiscountonuserticket : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.UserTickets", "AppliedDiscounts", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.UserTickets", "AppliedDiscounts");
        }
    }
}
