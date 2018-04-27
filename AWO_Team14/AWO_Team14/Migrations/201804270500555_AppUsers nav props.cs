namespace AWO_Team14.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AppUsersnavprops : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Schedules", "EndDate");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Schedules", "EndDate", c => c.DateTime(nullable: false));
        }
    }
}
