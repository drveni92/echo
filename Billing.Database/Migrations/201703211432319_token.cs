namespace Billing.Database.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class token : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ApiUsers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Secret = c.String(),
                        AppId = c.String(),
                        CreatedBy = c.Int(nullable: false),
                        CreatedOn = c.DateTime(nullable: false),
                        Deleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.AuthTokens",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Token = c.String(),
                        Expiration = c.DateTime(nullable: false),
                        CreatedBy = c.Int(nullable: false),
                        CreatedOn = c.DateTime(nullable: false),
                        Deleted = c.Boolean(nullable: false),
                        ApiUser_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ApiUsers", t => t.ApiUser_Id)
                .Index(t => t.ApiUser_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AuthTokens", "ApiUser_Id", "dbo.ApiUsers");
            DropIndex("dbo.AuthTokens", new[] { "ApiUser_Id" });
            DropTable("dbo.AuthTokens");
            DropTable("dbo.ApiUsers");
        }
    }
}
