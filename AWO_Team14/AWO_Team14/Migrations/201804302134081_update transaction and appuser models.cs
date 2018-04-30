namespace AWO_Team14.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updatetransactionandappusermodels : DbMigration
    {
		public override void Up()
		{
			AddColumn("dbo.AspNetUsers", "CC", c => c.String());
			
		}

		public override void Down()
		{
			DropColumn("dbo.AspNetUsers", "CC");
		}
	}
}
