namespace AWO_Team14.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateseeduserdata : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Users", "SSN");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Users", "SSN", c => c.String());
        }
    }
}
