namespace AWO_Team14.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class StarterSeed : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Genres",
                c => new
                    {
                        GenreID = c.Int(nullable: false, identity: true),
                        GenreName = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.GenreID);
            
            CreateTable(
                "dbo.Movies",
                c => new
                    {
                        MovieID = c.Int(nullable: false, identity: true),
                        Title = c.String(nullable: false),
                        Tagline = c.String(nullable: false),
                        Overview = c.String(nullable: false),
                        ReleaseYear = c.DateTime(nullable: false),
                        MPAA_Rating = c.Int(nullable: false),
                        RunTime = c.Int(nullable: false),
                        Actors = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.MovieID);
            
            CreateTable(
                "dbo.MovieGenres",
                c => new
                    {
                        Movie_MovieID = c.Int(nullable: false),
                        Genre_GenreID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Movie_MovieID, t.Genre_GenreID })
                .ForeignKey("dbo.Movies", t => t.Movie_MovieID, cascadeDelete: true)
                .ForeignKey("dbo.Genres", t => t.Genre_GenreID, cascadeDelete: true)
                .Index(t => t.Movie_MovieID)
                .Index(t => t.Genre_GenreID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.MovieGenres", "Genre_GenreID", "dbo.Genres");
            DropForeignKey("dbo.MovieGenres", "Movie_MovieID", "dbo.Movies");
            DropIndex("dbo.MovieGenres", new[] { "Genre_GenreID" });
            DropIndex("dbo.MovieGenres", new[] { "Movie_MovieID" });
            DropTable("dbo.MovieGenres");
            DropTable("dbo.Movies");
            DropTable("dbo.Genres");
        }
    }
}
