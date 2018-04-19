namespace AWO_Team14.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Changecurrenttostatus : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.UserTickets", "Status", c => c.Int(nullable: false));
            DropColumn("dbo.UserTickets", "Current");
        }
        
        public override void Down()
        {
            AddColumn("dbo.UserTickets", "Current", c => c.Boolean(nullable: false));
            DropColumn("dbo.UserTickets", "Status");
        }
    }
}
