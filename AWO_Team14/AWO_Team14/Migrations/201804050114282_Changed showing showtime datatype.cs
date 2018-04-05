namespace AWO_Team14.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Changedshowingshowtimedatatype : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Showings", "ShowTime", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Showings", "ShowTime", c => c.Int(nullable: false));
        }
    }
}
