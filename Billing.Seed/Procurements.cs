using Billing.Database;
using Billing.Repository;
using System.Data;

namespace Billing.Seed
{
    internal class Procurements
    {
        public static void Get()
        {
            IBillingRepository<Procurement> procurements = new BillingRepository<Procurement>(Help.Context);
            IBillingRepository<Supplier> suppliers = new BillingRepository<Supplier>(Help.Context);
            IBillingRepository<Product> products = new BillingRepository<Product>(Help.Context);
            int N = 0;
            DataTable rawData = Help.OpenExcel("Procurements");

            foreach (DataRow row in rawData.Rows)
            {
                try
                {
                    Procurement procurement = new Procurement()
                    {
                        Document = Help.getString(row, 0),
                        Date = Help.getDate(row, 1),
                        Supplier = suppliers.Get(Help.dicSupp[Help.getInteger(row, 2)]),
                        Product = products.Get(Help.dicProd[Help.getInteger(row, 3)]),
                        Quantity = Help.getInteger(row, 4),
                        Price = Help.getDouble(row, 5)
                    };

                    procurements.Insert(procurement);
                    N++;
                }
                catch (System.Exception ex)
                {
                    System.Console.WriteLine(ex.Message);
                }
            }
            procurements.Commit();

            System.Console.WriteLine(N);
        }
    }
}