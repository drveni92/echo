namespace Billing.Database.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class add_auth_token_agent : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AuthTokens",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Token = c.String(),
                        Expiration = c.DateTime(nullable: false),
                        Remember = c.String(),
                        CreatedBy = c.Int(nullable: false),
                        CreatedOn = c.DateTime(nullable: false),
                        Deleted = c.Boolean(nullable: false),
                        Agent_Id = c.Int(),
                        ApiUser_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Agents", t => t.Agent_Id)
                .ForeignKey("dbo.ApiUsers", t => t.ApiUser_Id)
                .Index(t => t.Agent_Id)
                .Index(t => t.ApiUser_Id);
              
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AuthTokens", "ApiUser_Id", "dbo.ApiUsers");
            DropForeignKey("dbo.AuthTokens", "Agent_Id", "dbo.Agents");
            DropIndex("dbo.AuthTokens", new[] { "ApiUser_Id" });
            DropIndex("dbo.AuthTokens", new[] { "Agent_Id" });
            DropTable("dbo.AuthTokens");
        }
    }
}
