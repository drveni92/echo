using Billing.Database;
using Billing.Repository;
using System.Data;

namespace Billing.Seed
{
    internal class Invoices
    {
        public static void Get()
        {
            IBillingRepository<Invoice> invoices = new BillingRepository<Invoice>(Help.Context);
            IBillingRepository<Shipper> shippers = new BillingRepository<Shipper>(Help.Context);
            IBillingRepository<Agent> agents = new BillingRepository<Agent>(Help.Context);
            IBillingRepository<Customer> customers = new BillingRepository<Customer>(Help.Context);
            DataTable rawData = Help.OpenExcel("Invoices");
            int N = 0;

            foreach (DataRow row in rawData.Rows)
            {
                try
                {
                    Invoice invoice = new Invoice()
                    {
                        InvoiceNo = Help.getString(row, 0),
                        Date = Help.getDate(row, 1),
                        ShippedOn = Help.getDate(row, 2),
                        Shipper = shippers.Get(Help.dicShip[Help.getInteger(row, 3)]),
                        Agent = agents.Get(Help.dicAgen[Help.getInteger(row, 4)]),
                        Customer = customers.Get(Help.dicCust[Help.getInteger(row, 5)]),
                        Vat = Help.getDouble(row, 6),
                        Shipping = Help.getDouble(row, 7)
                    };

                    invoices.Insert(invoice);
                    N++;
                }
                catch (System.Exception ex)
                {
                    System.Console.WriteLine(ex.Message);
                }
            }
            invoices.Commit();
            System.Console.WriteLine(N);
        }
    }
}