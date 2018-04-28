namespace AWO_Team14.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class adddataannotationstomoviereviews : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "CreditCardNumber1", c => c.String());
            AddColumn("dbo.AspNetUsers", "CreditCardNumber2", c => c.String());
            AlterColumn("dbo.MovieReviews", "Review", c => c.String(maxLength: 100));
            DropColumn("dbo.AspNetUsers", "CreditCardNumber");
        }
        
        public override void Down()
        {
            AddColumn("dbo.AspNetUsers", "CreditCardNumber", c => c.String());
            AlterColumn("dbo.MovieReviews", "Review", c => c.String());
            DropColumn("dbo.AspNetUsers", "CreditCardNumber2");
            DropColumn("dbo.AspNetUsers", "CreditCardNumber1");
        }
    }
}
