namespace Billing.Database.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class uniqueinvoiceno : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Invoices", "InvoiceNo", c => c.String(nullable: false, maxLength: 15));
            CreateIndex("dbo.Invoices", "InvoiceNo", unique: true);
        }
        
        public override void Down()
        {
            DropIndex("dbo.Invoices", new[] { "InvoiceNo" });
            AlterColumn("dbo.Invoices", "InvoiceNo", c => c.String(nullable: false));
        }
    }
}
