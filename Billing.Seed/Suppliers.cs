using Billing.Database;
using Billing.Repository;
using System;
using System.Data;
using System.Linq;

namespace Billing.Seed
{
    internal class Suppliers
    {
        public static void Get()
        {
            IBillingRepository<Supplier> suppliers = new BillingRepository<Supplier>(Help.Context);
            IBillingRepository<Town> towns = new BillingRepository<Town>(Help.Context);
            DataTable rawData = Help.OpenExcel("Suppliers");
            int N = 0;

            foreach (DataRow row in rawData.Rows)
            {
                try
                {
                    int oldId = Help.getInteger(row, 0);
                    string zip = Help.getString(row, 2);
                    Supplier supplier = new Supplier()
                    {
                        Name = Help.getString(row, 1),
                        Address = Help.getString(row, 3),
                        Town = towns.Get().FirstOrDefault(x => x.Zip == zip)
                    };

                    suppliers.Insert(supplier);
                    suppliers.Commit();
                    N++;
                    Help.dicSupp.Add(oldId, supplier.Id);
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