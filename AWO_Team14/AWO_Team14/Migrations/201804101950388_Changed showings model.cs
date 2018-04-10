namespace AWO_Team14.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Changedshowingsmodel : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Showings", "StartHour", c => c.Int(nullable: false));
            AddColumn("dbo.Showings", "StartMinute", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Showings", "StartMinute");
            DropColumn("dbo.Showings", "StartHour");
        }
    }
}
