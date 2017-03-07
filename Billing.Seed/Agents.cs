using Billing.Database;
using Billing.Repository;
using System;
using System.Data;
using System.Linq;

namespace Billing.Seed
{
    internal class Agents
    {
        public static void Get()
        {
            IBillingRepository<Agent> agents = new BillingRepository<Agent>(Help.Context);
            IBillingRepository<Town> towns = new BillingRepository<Town>(Help.Context);
            int N = 0;
            DataTable rawData = Help.OpenExcel("Agents");

            foreach (DataRow row in rawData.Rows)
            {
                try
                {
                    int oldId = Help.getInteger(row, 0);
                    Agent agent = new Agent()
                    {
                        Name = Help.getString(row, 1)
                    };

                    string[] zones = Help.getString(row, 2).Split(',');
                    foreach (string zone in zones)
                    {
                        Region r = (Region)Convert.ToInt32(zone);
                        var areas = towns.Get().Where(x => x.Region == r).ToList();
                        foreach (var city in areas)
                        {
                            agent.Towns.Add(city);
                        }
                    }

                    agents.Insert(agent);
                    agents.Commit();
                    N++;
                    Help.dicAgen.Add(oldId, agent.Id);
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