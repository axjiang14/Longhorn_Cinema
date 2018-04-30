namespace AWO_Team14.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class votesmodelstuff : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Votes", "User_Id", c => c.String(maxLength: 128));
            AddColumn("dbo.UserTickets", "Email", c => c.String());
            CreateIndex("dbo.Votes", "User_Id");
            AddForeignKey("dbo.Votes", "User_Id", "dbo.AspNetUsers", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Votes", "User_Id", "dbo.AspNetUsers");
            DropIndex("dbo.Votes", new[] { "User_Id" });
            DropColumn("dbo.UserTickets", "Email");
            DropColumn("dbo.Votes", "User_Id");
        }
    }
}
