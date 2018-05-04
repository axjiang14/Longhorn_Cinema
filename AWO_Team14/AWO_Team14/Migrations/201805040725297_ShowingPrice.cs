namespace AWO_Team14.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ShowingPrice : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Showings", "ShowingPrice", c => c.Decimal(nullable: false, precision: 18, scale: 2));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Showings", "ShowingPrice");
        }
    }
}
