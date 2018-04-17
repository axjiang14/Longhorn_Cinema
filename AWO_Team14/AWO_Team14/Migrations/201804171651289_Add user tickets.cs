namespace AWO_Team14.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Addusertickets : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.UserTickets", "SeatNumber", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.UserTickets", "SeatNumber");
        }
    }
}
