namespace AWO_Team14.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Changedshowingsmodels : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.ShowingMovies", "Showing_ShowingID", "dbo.Showings");
            DropForeignKey("dbo.ShowingMovies", "Movie_MovieID", "dbo.Movies");
            DropIndex("dbo.ShowingMovies", new[] { "Showing_ShowingID" });
            DropIndex("dbo.ShowingMovies", new[] { "Movie_MovieID" });
            AddColumn("dbo.Showings", "Movie_MovieID", c => c.Int());
            CreateIndex("dbo.Showings", "Movie_MovieID");
            AddForeignKey("dbo.Showings", "Movie_MovieID", "dbo.Movies", "MovieID");
            DropTable("dbo.ShowingMovies");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.ShowingMovies",
                c => new
                    {
                        Showing_ShowingID = c.Int(nullable: false),
                        Movie_MovieID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Showing_ShowingID, t.Movie_MovieID });
            
            DropForeignKey("dbo.Showings", "Movie_MovieID", "dbo.Movies");
            DropIndex("dbo.Showings", new[] { "Movie_MovieID" });
            DropColumn("dbo.Showings", "Movie_MovieID");
            CreateIndex("dbo.ShowingMovies", "Movie_MovieID");
            CreateIndex("dbo.ShowingMovies", "Showing_ShowingID");
            AddForeignKey("dbo.ShowingMovies", "Movie_MovieID", "dbo.Movies", "MovieID", cascadeDelete: true);
            AddForeignKey("dbo.ShowingMovies", "Showing_ShowingID", "dbo.Showings", "ShowingID", cascadeDelete: true);
        }
    }
}
