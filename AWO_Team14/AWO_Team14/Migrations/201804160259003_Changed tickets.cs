namespace AWO_Team14.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Changedtickets : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.UserTickets", "SeatNumber");
        }
        
        public override void Down()
        {
            AddColumn("dbo.UserTickets", "SeatNumber", c => c.String(nullable: false));
        }
    }
}
