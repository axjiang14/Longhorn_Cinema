namespace AWO_Team14.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateMovieReviewModel : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.MovieReviews",
                c => new
                    {
                        MovieReviewID = c.Int(nullable: false, identity: true),
                        Rating = c.Int(nullable: false),
                        Review = c.String(),
                        Status = c.Int(nullable: false),
                        Movie_MovieID = c.Int(),
                        User_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.MovieReviewID)
                .ForeignKey("dbo.Movies", t => t.Movie_MovieID)
                .ForeignKey("dbo.AspNetUsers", t => t.User_Id)
                .Index(t => t.Movie_MovieID)
                .Index(t => t.User_Id);
            
            CreateTable(
                "dbo.Votes",
                c => new
                    {
                        VoteID = c.Int(nullable: false, identity: true),
                        ThumbVote = c.Int(nullable: false),
                        MovieReview_MovieReviewID = c.Int(),
                    })
                .PrimaryKey(t => t.VoteID)
                .ForeignKey("dbo.MovieReviews", t => t.MovieReview_MovieReviewID)
                .Index(t => t.MovieReview_MovieReviewID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Votes", "MovieReview_MovieReviewID", "dbo.MovieReviews");
            DropForeignKey("dbo.MovieReviews", "User_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.MovieReviews", "Movie_MovieID", "dbo.Movies");
            DropIndex("dbo.Votes", new[] { "MovieReview_MovieReviewID" });
            DropIndex("dbo.MovieReviews", new[] { "User_Id" });
            DropIndex("dbo.MovieReviews", new[] { "Movie_MovieID" });
            DropTable("dbo.Votes");
            DropTable("dbo.MovieReviews");
        }
    }
}
