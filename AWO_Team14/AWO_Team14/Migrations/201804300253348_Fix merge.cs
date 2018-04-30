namespace AWO_Team14.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Fixmerge : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Votes", "User_Id", "dbo.AspNetUsers");
            DropIndex("dbo.Votes", new[] { "User_Id" });
            DropColumn("dbo.Votes", "User_Id");
            DropColumn("dbo.UserTickets", "Email");
        }
        
        public override void Down()
        {
            AddColumn("dbo.UserTickets", "Email", c => c.String());
            AddColumn("dbo.Votes", "User_Id", c => c.String(maxLength: 128));
            CreateIndex("dbo.Votes", "User_Id");
            AddForeignKey("dbo.Votes", "User_Id", "dbo.AspNetUsers", "Id");
        }
    }
}
