using Billing.Database;
using Billing.Repository;
using System;
using System.Data;

namespace Billing.Seed
{
    internal class Products
    {
        public static void Get()
        {
            IBillingRepository<Product> products = new BillingRepository<Product>(Help.Context);
            IBillingRepository<Category> categories = new BillingRepository<Category>(Help.Context);
            IBillingRepository<Stock> stocks = new BillingRepository<Stock>(Help.Context);
            Random r = new Random();
            DataTable rawData = Help.OpenExcel("Products");
            int N = 0;

            foreach (DataRow row in rawData.Rows)
            {
                try
                {
                    int oldId = Help.getInteger(row, 0);
                    Category category = categories.Get(Help.dicCatt[Help.getInteger(row, 3)]);

                    Product product = new Product()
                    {
                        Name = Help.getString(row, 1),
                        Unit = Help.getString(row, 2),
                        Price = Help.getDouble(row, 4),
                        Category = category,
                    };
                    N++;
                    products.Insert(product);
                    products.Commit();
                    stocks.Insert(new Stock()
                    {
                        Id = product.Id,
                        Input = r.Next(100, 300),
                        Output = r.Next(0, 100)
                    });
                    products.Commit();
                    Help.dicProd.Add(oldId, product.Id);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }

            Console.WriteLine(N);
        }
    }
}