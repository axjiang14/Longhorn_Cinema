namespace AWO_Team14.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class NotSure : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Schedules",
                c => new
                    {
                        ScheduleID = c.Int(nullable: false, identity: true),
                        Published = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.ScheduleID);
            
            AddColumn("dbo.Showings", "Schedule_ScheduleID", c => c.Int());
            CreateIndex("dbo.Showings", "Schedule_ScheduleID");
            AddForeignKey("dbo.Showings", "Schedule_ScheduleID", "dbo.Schedules", "ScheduleID");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Showings", "Schedule_ScheduleID", "dbo.Schedules");
            DropIndex("dbo.Showings", new[] { "Schedule_ScheduleID" });
            DropColumn("dbo.Showings", "Schedule_ScheduleID");
            DropTable("dbo.Schedules");
        }
    }
}
