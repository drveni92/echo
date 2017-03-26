using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Billing.API.Controllers;
using System.Web.Http;
using System.Net.Http;
using System.Web.Http.Routing;
using System.Web.Http.Controllers;
using System.Web.Http.Hosting;
using System.Threading;
using Billing.API.Models;

namespace Billing.Test
{
    [TestClass]
    public class TestHistoryController
    {

        HistoryController controller = new HistoryController();
        HttpConfiguration config = new HttpConfiguration();
        HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, "api/histories");


        void GetReady()
        {
            var route = config.Routes.MapHttpRoute("default", "api/{controller}/{id}");
            var routeData = new HttpRouteData(route, new HttpRouteValueDictionary { { "controller", "histories" } });

            controller.ControllerContext = new HttpControllerContext(config, routeData, request);
            controller.Request = request;
            controller.Request.Properties[HttpPropertyKeys.HttpConfigurationKey] = config;
        }


        [TestMethod]
        public void GetAllHistories()
        {
            TestHelper.InitDatabase();
            GetReady();
            var actRes = controller.Get();
            var response = actRes.ExecuteAsync(CancellationToken.None).Result;

            Assert.IsNotNull(response.Content);
        }

        [TestMethod]
        public void GetHistoryById()
        {
            GetReady();
            var actRes = controller.GetById(1);
            var response = actRes.ExecuteAsync(CancellationToken.None).Result;

            Assert.IsNotNull(response.Content);
        }

        [TestMethod]
        public void PostHistoryForInvoice()
        {
            GetReady();
            var actRes = controller.Post(new HistoryModel() { Date=DateTime.Now, Status = 2 , Invoice = new HistoryModel.HistoryInvoice { Id = 1 } });
            var response = actRes.ExecuteAsync(CancellationToken.None).Result;

            Assert.IsTrue(response.IsSuccessStatusCode);
        }

        [TestMethod]
        public void ChangeInvoicesStatus()
        {
            GetReady();
            var actRes = controller.Post(new HistoryModel() { Id = 1, Date = DateTime.Now, Status = 3, Invoice = new HistoryModel.HistoryInvoice { Id = 1 } });
            var response = actRes.ExecuteAsync(CancellationToken.None).Result;

            Assert.IsTrue(response.IsSuccessStatusCode);
        }
    }
}
