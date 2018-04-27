namespace AWO_Team14.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Uncommentuserontrans : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        UserID = c.Int(nullable: false, identity: true),
                        UserType = c.Int(nullable: false),
                        FirstName = c.String(nullable: false),
                        MiddleInitial = c.String(),
                        LastName = c.String(nullable: false),
                        Email = c.String(nullable: false),
                        Street = c.String(),
                        City = c.String(),
                        State = c.Int(nullable: false),
                        Zip = c.String(maxLength: 5),
                        Birthday = c.DateTime(nullable: false),
                        PhoneNumer = c.String(nullable: false),
                        Password = c.String(nullable: false),
                        CreditCardNumber = c.String(),
                        PopcornPoints = c.Int(nullable: false),
                        Archived = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.UserID);
            
            CreateTable(
                "dbo.MovieReviews",
                c => new
                    {
                        MovieReviewID = c.Int(nullable: false, identity: true),
                        Rating = c.Int(nullable: false),
                        Review = c.String(),
                        Movie_MovieID = c.Int(),
                        User_UserID = c.Int(),
                    })
                .PrimaryKey(t => t.MovieReviewID)
                .ForeignKey("dbo.Movies", t => t.Movie_MovieID)
                .ForeignKey("dbo.Users", t => t.User_UserID)
                .Index(t => t.Movie_MovieID)
                .Index(t => t.User_UserID);
            
            CreateTable(
                "dbo.Votes",
                c => new
                    {
                        VoteID = c.Int(nullable: false, identity: true),
                        ThumbVote = c.Int(nullable: false),
                        MovieReview_MovieReviewID = c.Int(),
                        User_UserID = c.Int(),
                    })
                .PrimaryKey(t => t.VoteID)
                .ForeignKey("dbo.MovieReviews", t => t.MovieReview_MovieReviewID)
                .ForeignKey("dbo.Users", t => t.User_UserID)
                .Index(t => t.MovieReview_MovieReviewID)
                .Index(t => t.User_UserID);
            
            AddColumn("dbo.Transactions", "User_UserID", c => c.Int());
            CreateIndex("dbo.Transactions", "User_UserID");
            AddForeignKey("dbo.Transactions", "User_UserID", "dbo.Users", "UserID");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Votes", "User_UserID", "dbo.Users");
            DropForeignKey("dbo.Transactions", "User_UserID", "dbo.Users");
            DropForeignKey("dbo.Votes", "MovieReview_MovieReviewID", "dbo.MovieReviews");
            DropForeignKey("dbo.MovieReviews", "User_UserID", "dbo.Users");
            DropForeignKey("dbo.MovieReviews", "Movie_MovieID", "dbo.Movies");
            DropIndex("dbo.Votes", new[] { "User_UserID" });
            DropIndex("dbo.Votes", new[] { "MovieReview_MovieReviewID" });
            DropIndex("dbo.MovieReviews", new[] { "User_UserID" });
            DropIndex("dbo.MovieReviews", new[] { "Movie_MovieID" });
            DropIndex("dbo.Transactions", new[] { "User_UserID" });
            DropColumn("dbo.Transactions", "User_UserID");
            DropTable("dbo.Votes");
            DropTable("dbo.MovieReviews");
            DropTable("dbo.Users");
        }
    }
}
