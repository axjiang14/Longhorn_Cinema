namespace AWO_Team14.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RestOfModels : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Movies", "MovieNumber", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Movies", "MovieNumber");
        }
    }
}
