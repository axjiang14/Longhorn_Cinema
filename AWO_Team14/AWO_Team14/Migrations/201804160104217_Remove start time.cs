namespace AWO_Team14.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Removestarttime : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Showings", "StartTime");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Showings", "StartTime", c => c.DateTime(nullable: false));
        }
    }
}
