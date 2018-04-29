namespace AWO_Team14.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateshowingsclass : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Showings", "StartTime", c => c.DateTime(nullable: false));
            AddColumn("dbo.Showings", "EndTime", c => c.DateTime(nullable: false));
            DropColumn("dbo.Movies", "RunTime");
            AddColumn("dbo.Movies", "RunTime", c => c.Time(nullable: false, precision: 7));
            DropColumn("dbo.Showings", "ShowTime");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Showings", "ShowTime", c => c.DateTime(nullable: false));
            DropColumn("dbo.Movies", "RunTime");
            AddColumn("dbo.Movies", "RunTime", c => c.Int(nullable: false));
            DropColumn("dbo.Showings", "EndTime");
            DropColumn("dbo.Showings", "StartTime");
        }
    }
}
