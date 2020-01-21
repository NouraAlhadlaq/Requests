namespace SparePartsRequests.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ControllersAdded : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Managers",
                c => new
                    {
                        NationalId = c.Long(nullable: false, identity: true),
                        Name = c.String(),
                        IsAdmin = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.NationalId);
            
            CreateTable(
                "dbo.Requests",
                c => new
                    {
                        RequestId = c.Long(nullable: false, identity: true),
                        Title = c.String(),
                        Desc = c.String(),
                        RequestTypeId = c.Long(nullable: false),
                        IsApproved = c.Boolean(nullable: false),
                        IsRejected = c.Boolean(nullable: false),
                        IsCanceled = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.RequestId)
                .ForeignKey("dbo.RequestTypes", t => t.RequestTypeId, cascadeDelete: true)
                .Index(t => t.RequestTypeId);
            
            CreateTable(
                "dbo.RequestTypes",
                c => new
                    {
                        RequestTypeId = c.Long(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.RequestTypeId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Requests", "RequestTypeId", "dbo.RequestTypes");
            DropIndex("dbo.Requests", new[] { "RequestTypeId" });
            DropTable("dbo.RequestTypes");
            DropTable("dbo.Requests");
            DropTable("dbo.Managers");
        }
    }
}
