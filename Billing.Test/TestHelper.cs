﻿using Billing.Database;
using Billing.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Billing.Test
{
    static public class TestHelper
    {
        static public void InitDatabaseReports()
        {
            using (BillingContext context = new BillingContext())
            {
                context.Database.Delete();
                context.Database.Create();
            }
            Billing.Seed.Program.Run();
        }

        static public void InitDatabase()
        {
            try
            {
                using (BillingContext context = new BillingContext())
                {
                    context.Database.Delete();
                    context.Database.Create();
                }

                UnitOfWork unit = new UnitOfWork();

                unit.Towns.Insert(new Town() { Zip = "71000", Name = "Sarajevo", Region = Region.Sarajevo });
                unit.Towns.Insert(new Town() { Zip = "72000", Name = "Zenica", Region = Region.Zenica });
                unit.Towns.Insert(new Town() { Zip = "75000", Name = "Tuzla", Region = Region.Tuzla });
                unit.Commit();

                unit.Agents.Insert(new Agent() { Name = "Antonio", Username = "antonio", Towns = new List<Town>() { unit.Towns.Get(1) } });
                unit.Agents.Insert(new Agent() { Name = "Julia", Username = "julia", Towns = new List<Town>() { unit.Towns.Get(2) } });
                unit.Agents.Insert(new Agent() { Name = "Amer", Username = "amer", Towns = new List<Town>() { unit.Towns.Get(3) } });
                unit.Agents.Insert(new Agent() { Name = "Marlon", Username = "marlon", Towns = new List<Town>() { unit.Towns.Get(1) } });

                unit.Commit();

                unit.Categories.Insert(new Category() { Name = "Desktop" });
                unit.Categories.Insert(new Category() { Name = "Laptop" });
                unit.Categories.Insert(new Category() { Name = "Monitors" });
                unit.Commit();


                unit.Customers.Insert(new Customer() { Name = "Imtec", Address = "Titova 2", Town = unit.Towns.Get(1) });
                unit.Customers.Insert(new Customer() { Name = "Delta", Address = "Sarajevska 4", Town = unit.Towns.Get(2) });
                unit.Commit();
                unit.Suppliers.Insert(new Supplier() { Name = "Disti", Address = "Kranjceviceva 1", Town = unit.Towns.Get(1) });
                unit.Suppliers.Insert(new Supplier() { Name = "Dell", Address = "Bulevar 122", Town = unit.Towns.Get(3) });

                unit.Shippers.Insert(new Shipper() { Name = "Posta", Address = "Radnicka 22", Town = unit.Towns.Get(1) });
                unit.Shippers.Insert(new Shipper() { Name = "DHL", Address = "Mostarska 14", Town = unit.Towns.Get(2) });
                unit.Shippers.Insert(new Shipper() { Name = "INCO", Address = "Sarajevska 14", Town = unit.Towns.Get(2) });
                unit.Commit();

                unit.Products.Insert(new Product() { Name = "Racunar Dell 2866", Unit = "pcs", Price = 700, Category = unit.Categories.Get(1) });
                unit.Products.Insert(new Product() { Name = "Laptop Dell 2866", Unit = "pcs", Price = 699, Category = unit.Categories.Get(2) });
                unit.Products.Insert(new Product() { Name = "Laptop  2866", Unit = "pcs", Price = 699, Category = unit.Categories.Get(2) });

                unit.Commit();

                unit.Invoices.Insert(new Invoice()
                {
                    InvoiceNo = "AG4E21",
                    Date = new DateTime(2017, 1, 10),
                    ShippedOn = new DateTime(2017, 1, 18),
                    Vat = 17,
                    Shipping = 95,
                    Agent = unit.Agents.Get(1),
                    Customer = unit.Customers.Get(1),
                    Shipper = unit.Shippers.Get(1),
                    Status = 0
                });
                unit.Invoices.Insert(new Invoice()
                {
                    InvoiceNo = "EFG4E21",
                    Date = new DateTime(2017, 2, 18),
                    ShippedOn = new DateTime(2017, 2, 28),
                    Vat = 17,
                    Shipping = 59,
                    Agent = unit.Agents.Get(1),
                    Customer = unit.Customers.Get(1),
                    Shipper = unit.Shippers.Get(1),
                    Status = 0
                });

                unit.Invoices.Insert(new Invoice()
                {
                    InvoiceNo = "34552",
                    Date = new DateTime(2017, 2, 18),
                    ShippedOn = new DateTime(2017, 2, 28),
                    Vat = 27,
                    Shipping = 49,
                    Agent = unit.Agents.Get(1),
                    Customer = unit.Customers.Get(1),
                    Shipper = unit.Shippers.Get(1),
                    Status = 0
                });
                unit.Commit();

                unit.Items.Insert(new Item() { Invoice = unit.Invoices.Get(1), Product = unit.Products.Get(1), Price = 700, Quantity = 1 });
                unit.Items.Insert(new Item() { Invoice = unit.Invoices.Get(1), Product = unit.Products.Get(2), Price = 699, Quantity = 1 });
                unit.Items.Insert(new Item() { Invoice = unit.Invoices.Get(2), Product = unit.Products.Get(1), Price = 700, Quantity = 1 });
                unit.Commit();

                unit.Histories.Insert(new Event() { Invoice = unit.Invoices.Get(1), Date = DateTime.Now, Status = Status.InvoiceOnHold });
                unit.Histories.Insert(new Event() { Invoice = unit.Invoices.Get(2), Date = DateTime.Now, Status = Status.InvoicePaid });
                unit.Histories.Insert(new Event() { Invoice = unit.Invoices.Get(1), Date = DateTime.Now, Status = Status.InvoicePaid });

                unit.Procurements.Insert(new Procurement()
                {
                    Document = "55/17",
                    Date = new DateTime(2017, 1, 5),
                    Product = unit.Products.Get(1),
                    Supplier = unit.Suppliers.Get(1),
                    Quantity = 2,
                    Price = 700
                });
                unit.Procurements.Insert(new Procurement()
                {
                    Document = "2055-2",
                    Date = new DateTime(2017, 1, 11),
                    Product = unit.Products.Get(2),
                    Supplier = unit.Suppliers.Get(1),
                    Quantity = 2,
                    Price = 699
                });
                unit.Commit();
            }
            catch(Exception ex)
            {
                Console.WriteLine("Error preparing databse for tests: " + ex.Message);
            }

        }
    }
}
