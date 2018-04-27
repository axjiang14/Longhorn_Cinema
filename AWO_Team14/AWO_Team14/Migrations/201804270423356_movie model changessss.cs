namespace AWO_Team14.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class moviemodelchangessss : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Movies", "Tagline", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Movies", "Tagline", c => c.String(nullable: false));
        }
    }
}
