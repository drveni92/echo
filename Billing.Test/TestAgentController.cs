﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
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
using System.Security.Principal;

namespace Billing.Test
{
    [TestClass]
    public class TestAgentController
    {
        AgentsController controller = new AgentsController();
        HttpConfiguration config = new HttpConfiguration();

        HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, "api/agents");
 
        void GetReady()
        {
            var route = config.Routes.MapHttpRoute("default", "api/{controller}/{id}");
            var routeData = new HttpRouteData(route, new HttpRouteValueDictionary { { "controller", "agents" } });

            controller.ControllerContext = new HttpControllerContext(config, routeData, request);
            controller.Request = request;
            controller.Request.Properties[HttpPropertyKeys.HttpConfigurationKey] = config;

            controller.Request.Headers.TryAddWithoutValidation("Content-Type", "application/json");
            controller.Request.Headers.TryAddWithoutValidation("ApiKey", "R2lnaVNjaG9vbA==");
            string token = "R2lnaVNjaG9vbA==" + DateTime.UtcNow.ToString("s");
            controller.Request.Headers.TryAddWithoutValidation("Token", token);
            controller.RequestContext.Principal = new GenericPrincipal(new GenericIdentity("marlon", "billing"), new[] { "admin", "user" });
            Thread.CurrentPrincipal = controller.RequestContext.Principal;
        }

        [TestMethod]
        public void GetAllAgents()
        {
            TestHelper.InitDatabase();
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
            var actRes = controller.GetByTown(1);
            var response = actRes.ExecuteAsync(CancellationToken.None).Result;
            Assert.IsNotNull(response.Content);
        }

        [TestMethod]
        public void GetAgentById()
        {
            GetReady();
            var actRes = controller.GetById(4);
            var response = actRes.ExecuteAsync(CancellationToken.None).Result;

            Assert.IsNotNull(response.Content);
        }

        [TestMethod]
        public void GetAgentByWrongId()
        {
            GetReady();
            var actRes = controller.GetById(999);
            var response = actRes.ExecuteAsync(CancellationToken.None).Result;

            Assert.IsFalse(response.IsSuccessStatusCode);
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
            var actRes = controller.Put(4, new AgentModel() { Id = 4, Name = "Marlon New", Towns = new List<AgentTown>() { new AgentTown() { Id = 1 } } });
            var response = actRes.ExecuteAsync(CancellationToken.None).Result;

            Assert.IsTrue(response.IsSuccessStatusCode);
        }

        [TestMethod]
        public void ChangeTown()
        {
            TestHelper.InitDatabase();
            GetReady();
            var actRes = controller.Put(4, new AgentModel() { Id = 4, Name = "Marlon", Towns = new List<AgentTown>() { new AgentTown() { Id = 2 } } });
            var response = actRes.ExecuteAsync(CancellationToken.None).Result;

            Assert.IsTrue(response.IsSuccessStatusCode);
        }

        [TestMethod]
        public void DeleteById()
        {
            GetReady();
            var actRes = controller.Delete(3);
            var response = actRes.ExecuteAsync(CancellationToken.None).Result;

            Assert.IsFalse(response.IsSuccessStatusCode);
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
