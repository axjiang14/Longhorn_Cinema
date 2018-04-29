namespace AWO_Team14.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Whoknows : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.AspNetUsers", "Street", c => c.String(nullable: false));
            AlterColumn("dbo.AspNetUsers", "City", c => c.String(nullable: false));
            AlterColumn("dbo.AspNetUsers", "Zip", c => c.String(nullable: false, maxLength: 5));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.AspNetUsers", "Zip", c => c.String(maxLength: 5));
            AlterColumn("dbo.AspNetUsers", "City", c => c.String());
            AlterColumn("dbo.AspNetUsers", "Street", c => c.String());
        }
    }
}
