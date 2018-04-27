namespace AWO_Team14.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Changetoappuser : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.MovieReviews", "Movie_MovieID", "dbo.Movies");
            DropForeignKey("dbo.MovieReviews", "User_UserID", "dbo.Users");
            DropForeignKey("dbo.Votes", "MovieReview_MovieReviewID", "dbo.MovieReviews");
            DropForeignKey("dbo.Votes", "User_UserID", "dbo.Users");
            DropIndex("dbo.Transactions", new[] { "User_UserID" });
            DropIndex("dbo.MovieReviews", new[] { "Movie_MovieID" });
            DropIndex("dbo.MovieReviews", new[] { "User_UserID" });
            DropIndex("dbo.Votes", new[] { "MovieReview_MovieReviewID" });
            DropIndex("dbo.Votes", new[] { "User_UserID" });
            RenameColumn(table: "dbo.Transactions", name: "User_UserID", newName: "User_Id");
            AlterColumn("dbo.Transactions", "User_Id", c => c.String(maxLength: 128));
            CreateIndex("dbo.Transactions", "User_Id");
            DropTable("dbo.MovieReviews");
            DropTable("dbo.Votes");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.Votes",
                c => new
                    {
                        VoteID = c.Int(nullable: false, identity: true),
                        ThumbVote = c.Int(nullable: false),
                        MovieReview_MovieReviewID = c.Int(),
                        User_UserID = c.Int(),
                    })
                .PrimaryKey(t => t.VoteID);
            
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
                .PrimaryKey(t => t.MovieReviewID);
            
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
            
            DropIndex("dbo.Transactions", new[] { "User_Id" });
            AlterColumn("dbo.Transactions", "User_Id", c => c.Int());
            RenameColumn(table: "dbo.Transactions", name: "User_Id", newName: "User_UserID");
            CreateIndex("dbo.Votes", "User_UserID");
            CreateIndex("dbo.Votes", "MovieReview_MovieReviewID");
            CreateIndex("dbo.MovieReviews", "User_UserID");
            CreateIndex("dbo.MovieReviews", "Movie_MovieID");
            CreateIndex("dbo.Transactions", "User_UserID");
            AddForeignKey("dbo.Votes", "User_UserID", "dbo.Users", "UserID");
            AddForeignKey("dbo.Votes", "MovieReview_MovieReviewID", "dbo.MovieReviews", "MovieReviewID");
            AddForeignKey("dbo.MovieReviews", "User_UserID", "dbo.Users", "UserID");
            AddForeignKey("dbo.MovieReviews", "Movie_MovieID", "dbo.Movies", "MovieID");
        }
    }
}
