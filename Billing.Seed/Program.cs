using System;

namespace Billing.Seed
{
    public class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Data migration in progress");
            Console.WriteLine("--------------------------");
            Run();
            Console.WriteLine("-------------------------");
            Console.WriteLine("Press any key to continue");
            Console.ReadKey();
        }

        public static void Run()
        {
            Categories.Get();
            Products.Get();
            Towns.Get();
            Agents.Get();
            Shippers.Get();
            Suppliers.Get();
            Customers.Get();
            Procurements.Get();
            Invoices.Get();
            Items.Get();
        }
    }
}
