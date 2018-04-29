namespace AWO_Team14.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updatemoviemodel : DbMigration
    {
        public override void Up()
        {
			DropColumn("dbo.Movies", "ReleaseYear");
            AddColumn("dbo.Movies", "ReleaseYear", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            AddColumn("dbo.Movies", "ReleaseYear", c => c.DateTime(nullable: false));
        }
    }
}
