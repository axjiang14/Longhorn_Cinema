namespace AWO_Team14.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addStartDatepropertytoSchedule : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Schedules", "StartDate", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Schedules", "StartDate");
        }
    }
}
