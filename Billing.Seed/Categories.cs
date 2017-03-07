using Billing.Database;
using Billing.Repository;
using System;
using System.Data;

namespace Billing.Seed
{
    internal class Categories
    {
        public static void Get()
        {
            IBillingRepository<Category> categories = new BillingRepository<Category>(Help.Context);
            DataTable rawData = Help.OpenExcel("Categories");
            int N = 0;

            foreach (DataRow row in rawData.Rows)
            {
                try
                {
                    int oldId = Help.getInteger(row, 0);
                    Category cat = new Category()
                    {
                        Name = Help.getString(row, 1)
                    };
                    N++;
                    categories.Insert(cat);
                    categories.Commit();
                    Help.dicCatt.Add(oldId, cat.Id);
                }
                catch(Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            Console.WriteLine(N);
        }
    }
}