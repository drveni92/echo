using Billing.Database;
using Billing.Repository;
using System;
using System.Data;

namespace Billing.Seed
{
    internal class Towns
    {
        public static void Get()
        {
            IBillingRepository<Town> towns = new BillingRepository<Town>(Help.Context);
            DataTable rawData = Help.OpenExcel("Towns");
            int N = 0;

            foreach (DataRow row in rawData.Rows)
            {
                try
                {
                    Town town = new Town()
                    {
                        Zip = Help.getString(row, 0),
                        Name = Help.getString(row, 1),
                        Region = (Region)Help.getInteger(row, 2)
                    };
                    N++;
                    towns.Insert(town);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            towns.Commit();

            Console.WriteLine(N);
        }
    }
}