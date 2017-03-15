namespace Billing.Database.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class changes : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Invoices", "Agent_Id", "dbo.Agents");
            DropForeignKey("dbo.TownAgents", "Town_Id", "dbo.Towns");
            DropForeignKey("dbo.TownAgents", "Agent_Id", "dbo.Agents");
            DropForeignKey("dbo.Invoices", "Customer_Id", "dbo.Customers");
            DropForeignKey("dbo.Invoices", "Shipper_Id", "dbo.Shippers");
            DropForeignKey("dbo.Procurements", "Supplier_Id", "dbo.Suppliers");
            DropForeignKey("dbo.Procurements", "Product_Id", "dbo.Products");
            DropForeignKey("dbo.Products", "Category_Id", "dbo.Categories");
            AddForeignKey("dbo.Invoices", "Agent_Id", "dbo.Agents", "Id");
            AddForeignKey("dbo.TownAgents", "Town_Id", "dbo.Towns", "Id");
            AddForeignKey("dbo.TownAgents", "Agent_Id", "dbo.Agents", "Id");
            AddForeignKey("dbo.Invoices", "Customer_Id", "dbo.Customers", "Id");
            AddForeignKey("dbo.Invoices", "Shipper_Id", "dbo.Shippers", "Id");
            AddForeignKey("dbo.Procurements", "Supplier_Id", "dbo.Suppliers", "Id");
            AddForeignKey("dbo.Procurements", "Product_Id", "dbo.Products", "Id");
            AddForeignKey("dbo.Products", "Category_Id", "dbo.Categories", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Products", "Category_Id", "dbo.Categories");
            DropForeignKey("dbo.Procurements", "Product_Id", "dbo.Products");
            DropForeignKey("dbo.Procurements", "Supplier_Id", "dbo.Suppliers");
            DropForeignKey("dbo.Invoices", "Shipper_Id", "dbo.Shippers");
            DropForeignKey("dbo.Invoices", "Customer_Id", "dbo.Customers");
            DropForeignKey("dbo.TownAgents", "Agent_Id", "dbo.Agents");
            DropForeignKey("dbo.TownAgents", "Town_Id", "dbo.Towns");
            DropForeignKey("dbo.Invoices", "Agent_Id", "dbo.Agents");
            AddForeignKey("dbo.Products", "Category_Id", "dbo.Categories", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Procurements", "Product_Id", "dbo.Products", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Procurements", "Supplier_Id", "dbo.Suppliers", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Invoices", "Shipper_Id", "dbo.Shippers", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Invoices", "Customer_Id", "dbo.Customers", "Id", cascadeDelete: true);
            AddForeignKey("dbo.TownAgents", "Agent_Id", "dbo.Agents", "Id", cascadeDelete: true);
            AddForeignKey("dbo.TownAgents", "Town_Id", "dbo.Towns", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Invoices", "Agent_Id", "dbo.Agents", "Id", cascadeDelete: true);
        }
    }
}
