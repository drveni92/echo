namespace Billing.Database.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class add_agents : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Agents", "Username", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Agents", "Username");
        }
    }
}
