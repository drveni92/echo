using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using Billing.API.Controllers;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Net.Http;
using Billing.Database;
using System.Web.Http.Routing;
using System.Web.Http.Controllers;
using System.Web.Http.Hosting;
using System.Threading;
using Billing.API.Models;
using static Billing.API.Models.AgentModel;

namespace Billing.Test
{
    [TestClass]
    public class TestAgentController
    {
        AgentsController controller = new AgentsController();
        HttpConfiguration config = new HttpConfiguration();

        HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, "api/agents");

        [TestInitialize]
        public void Init()
        {
            using(BillingContext context = new BillingContext())
            {
                context.Database.Delete();
                context.Database.Create();

                Town town = new Town()
                {
                    Name = "Sarajevo",
                    Region = Region.Sarajevo,
                    Zip = "71300"
                };

                Town town2 = new Town()
                {
                    Name = "Zenica",
                    Region = Region.Zenica,
                    Zip = "71000"
                };

                context.Towns.Add(town2);
                context.Towns.Add(town);
                context.SaveChanges();

                Agent agent = new Agent()
                {
                    Name = "Dejan",
                    Towns = new List<Town>() { town }
                };

                Agent agent2 = new Agent()
                {
                    Name = "Amer",
                    Towns = new List<Town>() { town2 }
                };

                Agent agent3 = new Agent()
                {
                    Name = "Amar",
                    Towns = new List<Town>() { town }
                };

                context.Agents.Add(agent);
                context.SaveChanges();
            }
        }

        void GetReady()
        {
            var route = config.Routes.MapHttpRoute("default", "api/{controller}/{id}");
            var routeData = new HttpRouteData(route, new HttpRouteValueDictionary { { "controller", "agents" } });

            controller.ControllerContext = new HttpControllerContext(config, routeData, request);
            controller.Request = request;
            controller.Request.Properties[HttpPropertyKeys.HttpConfigurationKey] = config;
        }

        [TestMethod]
        public void GetAllAgents()
        {
            Init();
            GetReady();
            var actRes = controller.Get();
            var response = actRes.ExecuteAsync(CancellationToken.None).Result;

            Assert.IsNotNull(response.Content);
        }

        [TestMethod]
        public void GetAgetsByName()
        {
            GetReady();
            var actRes = controller.Get("amer");
            var response = actRes.ExecuteAsync(CancellationToken.None).Result;
            Assert.IsNotNull(response.Content);
        }

        [TestMethod]
        public void GetAgetsByTown()
        {
            GetReady();
            var actRes = controller.Get("town/1");
            var response = actRes.ExecuteAsync(CancellationToken.None).Result;
            Assert.IsNotNull(response.Content);
        }

        [TestMethod]
        public void GetAgentById()
        {
            GetReady();
            var actRes = controller.GetById(1);
            var response = actRes.ExecuteAsync(CancellationToken.None).Result;

            Assert.IsNotNull(response.Content);
        }

        [TestMethod]
        public void GetAgentByWrongId()
        {
            GetReady();
            var actRes = controller.GetById(999);
            var response = actRes.ExecuteAsync(CancellationToken.None).Result;

            Assert.IsNull(response.Content);
        }

        [TestMethod]
        public void PostAgentGood()
        {
            GetReady();
            var actRes = controller.Post(new AgentModel() { Name = "Amer", Towns = new List<AgentTown>() { new AgentTown() { Id = 1 } } });
            var response = actRes.ExecuteAsync(CancellationToken.None).Result;

            Assert.IsTrue(response.IsSuccessStatusCode);
        }

        [TestMethod]
        public void PostAgentBad()
        {
            GetReady();
            var actRes = controller.Post(new AgentModel() { Name = "Amer", Towns = new List<AgentTown>() { new AgentTown() { Id = 999 } } });
            var response = actRes.ExecuteAsync(CancellationToken.None).Result;

            Assert.IsFalse(response.IsSuccessStatusCode);
        }

        [TestMethod]
        public void ChangeName()
        {
            GetReady();
            var actRes = controller.Put(1, new AgentModel() { Id = 1, Name = "Amer", Towns = new List<AgentTown>() { new AgentTown() { Id = 1 } } });
            var response = actRes.ExecuteAsync(CancellationToken.None).Result;

            Assert.IsTrue(response.IsSuccessStatusCode);
        }

        [TestMethod]
        public void ChangeTown()
        {
            GetReady();
            var actRes = controller.Put(1, new AgentModel() { Id = 1, Name = "Dejan", Towns = new List<AgentTown>() { new AgentTown() { Id = 2 } } });
            var response = actRes.ExecuteAsync(CancellationToken.None).Result;

            Assert.IsTrue(response.IsSuccessStatusCode);
        }

        [TestMethod]
        public void DeleteById()
        {
            GetReady();
            var actRes = controller.Delete(1);
            var response = actRes.ExecuteAsync(CancellationToken.None).Result;

            Assert.IsTrue(response.IsSuccessStatusCode);
        }

        [TestMethod]
        public void DeleteByWrongId()
        {
            GetReady();
            var actRes = controller.Delete(999);
            var response = actRes.ExecuteAsync(CancellationToken.None).Result;

            Assert.IsFalse(response.IsSuccessStatusCode);
        }
    }
}
