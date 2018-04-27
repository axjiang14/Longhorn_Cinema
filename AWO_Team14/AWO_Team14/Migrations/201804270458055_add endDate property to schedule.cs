namespace AWO_Team14.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addendDatepropertytoschedule : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Schedules", "EndDate", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Schedules", "EndDate");
        }
    }
}
