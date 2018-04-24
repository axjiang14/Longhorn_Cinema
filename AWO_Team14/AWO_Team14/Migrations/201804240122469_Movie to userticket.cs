namespace AWO_Team14.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Movietouserticket : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.UserTickets", "MovieID", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.UserTickets", "MovieID");
        }
    }
}
