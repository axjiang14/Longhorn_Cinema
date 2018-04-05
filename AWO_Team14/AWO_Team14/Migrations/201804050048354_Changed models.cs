namespace AWO_Team14.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Changedmodels : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Showings",
                c => new
                    {
                        ShowingID = c.Int(nullable: false, identity: true),
                        ShowDate = c.DateTime(nullable: false),
                        ShowTime = c.Int(nullable: false),
                        Special = c.Boolean(nullable: false),
                        Theater = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ShowingID);
            
            CreateTable(
                "dbo.ShowingMovies",
                c => new
                    {
                        Showing_ShowingID = c.Int(nullable: false),
                        Movie_MovieID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Showing_ShowingID, t.Movie_MovieID })
                .ForeignKey("dbo.Showings", t => t.Showing_ShowingID, cascadeDelete: true)
                .ForeignKey("dbo.Movies", t => t.Movie_MovieID, cascadeDelete: true)
                .Index(t => t.Showing_ShowingID)
                .Index(t => t.Movie_MovieID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ShowingMovies", "Movie_MovieID", "dbo.Movies");
            DropForeignKey("dbo.ShowingMovies", "Showing_ShowingID", "dbo.Showings");
            DropIndex("dbo.ShowingMovies", new[] { "Movie_MovieID" });
            DropIndex("dbo.ShowingMovies", new[] { "Showing_ShowingID" });
            DropTable("dbo.ShowingMovies");
            DropTable("dbo.Showings");
        }
    }
}
