using Billing.Database;
using Billing.Repository;
using System;
using System.Data;
using System.Linq;

namespace Billing.Seed
{
    internal class Shippers
    {
        public static void Get()
        {
            IBillingRepository<Shipper> shippers = new BillingRepository<Shipper>(Help.Context);
            IBillingRepository<Town> towns = new BillingRepository<Town>(Help.Context);
            DataTable rawData = Help.OpenExcel("Shippers");
            int N = 0;

            foreach (DataRow row in rawData.Rows)
            {
                try
                {
                    int oldId = Help.getInteger(row, 0);
                    string zip = Help.getString(row, 1);
                    Shipper shipper = new Shipper()
                    {
                        Name = Help.getString(row, 2),
                        Address = Help.getString(row, 3),
                        Town = towns.Get().FirstOrDefault(x => x.Zip == zip)
                    };

                    shippers.Insert(shipper);
                    shippers.Commit();
                    N++;
                    Help.dicShip.Add(oldId, shipper.Id);
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