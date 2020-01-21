namespace SparePartsRequests.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class userRequestsRelation : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Requests", "ApplicationUserID", c => c.String(maxLength: 128));
            CreateIndex("dbo.Requests", "ApplicationUserID");
            AddForeignKey("dbo.Requests", "ApplicationUserID", "dbo.AspNetUsers", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Requests", "ApplicationUserID", "dbo.AspNetUsers");
            DropIndex("dbo.Requests", new[] { "ApplicationUserID" });
            DropColumn("dbo.Requests", "ApplicationUserID");
        }
    }
}
