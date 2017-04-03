using Billing.Database;
using Billing.Repository;
using Billing.Seed;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Seed
{
    internal class Histories
    {
        public static void Get()
        {
            IBillingRepository<Event> histories = new BillingRepository<Event>(Help.Context);
            IBillingRepository<Invoice> invoices = new BillingRepository<Invoice>(Help.Context);

            int N = 0;
            DataTable rawData = Help.OpenExcel("Histories");

            foreach (DataRow row in rawData.Rows)
            {
                try
                {
                    string invoiceNo = Help.getString(row, 0);
                    Event item = new Event()
                    {
                        Invoice = invoices.Get().FirstOrDefault(x => x.InvoiceNo == invoiceNo),
                        Date = Help.getDate(row, 1),
                        Status = (Status)Help.getInteger(row, 2)
                    };

                    histories.Insert(item);
                    N++;
                }
                catch (System.Exception ex)
                {
                    System.Console.WriteLine(ex.Message);
                }
            }

            histories.Commit();
            Console.WriteLine(N);

        }

    }
}
