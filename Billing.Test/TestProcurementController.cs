using Billing.API.Controllers;
using Billing.API.Models;
using Billing.Database;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Hosting;
using System.Web.Http.Routing;

namespace Billing.Test
{
    [TestClass]
    public class TestProcurementController
    {
        ProcurementsController controller = new ProcurementsController();
        HttpConfiguration config = new HttpConfiguration();

        HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, "api/procurements");

        void GetReady()
        {
            var route = config.Routes.MapHttpRoute("default", "api/{controller}/{id}");
            var routeData = new HttpRouteData(route, new HttpRouteValueDictionary { { "controller", "products" } });

            controller.ControllerContext = new HttpControllerContext(config, routeData, request);
            controller.Request = request;
            controller.Request.Properties[HttpPropertyKeys.HttpConfigurationKey] = config;
        }

        [TestMethod]
        public void GetAllProcurements()
        {
            TestHelper.InitDatabase();
            GetReady();
            var actRes = controller.Get();
            var response = actRes.ExecuteAsync(CancellationToken.None).Result;

            Assert.IsNotNull(response.Content);
        }

        [TestMethod]
        public void GetProcurementById()
        {
            GetReady();
            var actRes = controller.GetById(1);
            var response = actRes.ExecuteAsync(CancellationToken.None).Result;

            Assert.IsNotNull(response.Content);
        }

        [TestMethod]
        public void GetProcurementsByDocumentGood()
        {
        
            GetReady();
            var actRes = controller.GetByDocument("2055-2");
            var response = actRes.ExecuteAsync(CancellationToken.None).Result;

            Assert.IsNotNull(response.Content);
        }

        [TestMethod]
        public void GetProcurementsByDocumentBad()
        {
        
            GetReady();
            var actRes = controller.GetByDocument("AAAAA");
            var response = actRes.ExecuteAsync(CancellationToken.None).Result;

            Assert.IsFalse(response.IsSuccessStatusCode);
        }

        [TestMethod]
        public void GetProcurementsByProductGood()
        {
            GetReady();
            var actRes = controller.GetByProductId(2);
            var response = actRes.ExecuteAsync(CancellationToken.None).Result;

            Assert.IsNotNull(response.Content);
        }

        [TestMethod]
        public void GetProcurementsByProductBad()
        {
            GetReady();
            var actRes = controller.GetByProductId(999);
            var response = actRes.ExecuteAsync(CancellationToken.None).Result;

            Assert.IsNull(response.Content);
        }

        [TestMethod]
        public void GetProcurementByWrongId()
        {
            GetReady();
            var actRes = controller.GetById(999);
            var response = actRes.ExecuteAsync(CancellationToken.None).Result;

            Assert.IsNull(response.Content);
        }

        [TestMethod]
        public void PostProcurementGood()
        {
            GetReady();
            var actRes = controller.Post(new ProcurementModel() { Quantity = 4, Price = 234, Date = DateTime.UtcNow, Document = "09000083", Supplier = new ProcurementModel.ProcurementSupplier() { Id = 1 }, Product = new ProcurementModel.ProcurementProduct() { Id = 1 } });
            var response = actRes.ExecuteAsync(CancellationToken.None).Result;

            Assert.IsTrue(response.IsSuccessStatusCode);
        }

        [TestMethod]
        public void PostProcurementBad()
        {
            GetReady();
            var actRes = controller.Post(new ProcurementModel() { Quantity = 4, Price = 234, Date = DateTime.UtcNow, Document = "09000083", Supplier = new ProcurementModel.ProcurementSupplier() { Id = 1 }, Product = new ProcurementModel.ProcurementProduct() { Id = 999 } });
            var response = actRes.ExecuteAsync(CancellationToken.None).Result;

            Assert.IsFalse(response.IsSuccessStatusCode);
        }

        [TestMethod]
        public void ChangeProcurementData()
        {
            GetReady();
            var actRes = controller.Put(1, new ProcurementModel() { Id = 1, Quantity = 5, Price = 7, Date = DateTime.UtcNow, Document = "08000001", Supplier = new ProcurementModel.ProcurementSupplier() { Id = 1 }, Product = new ProcurementModel.ProcurementProduct() { Id = 1 } });
            var response = actRes.ExecuteAsync(CancellationToken.None).Result;

            Assert.IsTrue(response.IsSuccessStatusCode);
        }

        [TestMethod]
        public void ChangeProcurementProductGood()
        {
            GetReady();
            var actRes = controller.Put(1, new ProcurementModel() { Id = 1, Quantity = 3, Price = 7, Date = DateTime.UtcNow, Document = "08000001", Supplier = new ProcurementModel.ProcurementSupplier() { Id = 1 }, Product = new ProcurementModel.ProcurementProduct() { Id = 2 } });
            var response = actRes.ExecuteAsync(CancellationToken.None).Result;

            Assert.IsTrue(response.IsSuccessStatusCode);
        }

        [TestMethod]
        public void ChangeProcurementProductBad()
        {
            GetReady();
            var actRes = controller.Put(1, new ProcurementModel() { Id = 1, Quantity = 3, Price = 7, Date = DateTime.UtcNow, Document = "08000001", Supplier = new ProcurementModel.ProcurementSupplier() { Id = 1 }, Product = new ProcurementModel.ProcurementProduct() { Id = 999 } });
            var response = actRes.ExecuteAsync(CancellationToken.None).Result;

            Assert.IsFalse(response.IsSuccessStatusCode);
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
