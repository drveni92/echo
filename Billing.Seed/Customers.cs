using Billing.Database;
using Billing.Repository;
using System;
using System.Data;
using System.Linq;

namespace Billing.Seed
{
    internal class Customers
    {
        public static void Get()
        {
            IBillingRepository<Customer> customers = new BillingRepository<Customer>(Help.Context);
            IBillingRepository<Town> towns = new BillingRepository<Town>(Help.Context);
            DataTable rawData = Help.OpenExcel("Customers");
            int N = 0;

            foreach (DataRow row in rawData.Rows)
            {
                try
                {
                    int oldId = Help.getInteger(row, 0);
                    string zip = Help.getString(row, 2);
                    Customer customer = new Customer()
                    {
                        Name = Help.getString(row, 1),
                        Address = Help.getString(row, 3),
                        Town = towns.Get().FirstOrDefault(x => x.Zip == zip)
                    };

                    customers.Insert(customer);
                    customers.Commit();
                    N++;
                    Help.dicCust.Add(oldId, customer.Id);
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