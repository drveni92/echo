using Billing.Database;
using Billing.Repository;
using System;
using System.Data;
using System.Linq;

namespace Billing.Seed
{
    internal class Items
    {
        public static void Get()
        {
            IBillingRepository<Item> items = new BillingRepository<Item>(Help.Context);
            IBillingRepository<Invoice> invoices = new BillingRepository<Invoice>(Help.Context);
            IBillingRepository<Product> products = new BillingRepository<Product>(Help.Context);
            int N = 0;
            DataTable rawData = Help.OpenExcel("Items");

            foreach (DataRow row in rawData.Rows)
            {
                try
                {
                    string invoiceNo = Help.getString(row, 0);
                    Item item = new Item()
                    {
                        Invoice = invoices.Get().FirstOrDefault(x => x.InvoiceNo == invoiceNo),
                        Product = products.Get(Help.dicProd[Help.getInteger(row, 1)]),
                        Quantity = Help.getInteger(row, 2),
                        Price = Help.getDouble(row, 3)
                    };

                    items.Insert(item);
                    N++;
                }
                catch (System.Exception ex)
                {
                    System.Console.WriteLine(ex.Message);
                }
            }

            items.Commit();
            Console.WriteLine(N);

        }
    }
}