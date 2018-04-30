namespace AWO_Team14.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Addpopcornspointsspent : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Transactions", "PopcornPointsSpent", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Transactions", "PopcornPointsSpent");
        }
    }
}
