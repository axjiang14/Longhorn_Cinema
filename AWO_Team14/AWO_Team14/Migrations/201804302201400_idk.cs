namespace AWO_Team14.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class idk : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.AspNetUsers", "CC");
        }
        
        public override void Down()
        {
            AddColumn("dbo.AspNetUsers", "CC", c => c.String());
        }
    }
}
