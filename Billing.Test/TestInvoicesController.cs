using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Billing.API.Controllers;
using System.Web.Http;
using System.Net.Http;
using Billing.Database;
using System.Collections.Generic;
using System.Web.Http.Routing;
using System.Web.Http.Controllers;
using System.Web.Http.Hosting;
using System.Threading;
using Billing.API.Models;
using System.Security.Principal;

namespace Billing.Test
{
    [TestClass]
    public class TestInvoicesController
    {
        readonly InvoicesController controller = new InvoicesController();
        readonly HttpConfiguration config = new HttpConfiguration();

        readonly HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, "api/invoices");
        private InvoiceModel.InvoiceShipper shipperModel;
        private InvoiceModel.InvoiceAgent agentModel;
        private InvoiceModel.InvoiceCustomer customerModel;
        private TownModel townModel;

        void GetReady(string user = "marlon")
        {
            var route = config.Routes.MapHttpRoute("default", "api/{controller}/{id}");
            var routeData = new HttpRouteData(route, new HttpRouteValueDictionary { { "controller", "invoices" } });

            controller.ControllerContext = new HttpControllerContext(config, routeData, request);
            controller.Request = request;
            controller.Request.Properties[HttpPropertyKeys.HttpConfigurationKey] = config;

            controller.Request.Headers.TryAddWithoutValidation("Content-Type", "application/json");
            controller.Request.Headers.TryAddWithoutValidation("ApiKey", "R2lnaVNjaG9vbA==");
            string token = "R2lnaVNjaG9vbA==" + DateTime.UtcNow.ToString("s");
            controller.Request.Headers.TryAddWithoutValidation("Token", token);
            controller.RequestContext.Principal = new GenericPrincipal(new GenericIdentity(user, "billing"), new[] { "admin", "user" });
            Thread.CurrentPrincipal = controller.RequestContext.Principal;
        }

        void setUpData(int townId, int customerId, int agentId, int shipperId)
        {
            townModel = new TownModel()
            {
                Id = townId
            };
            customerModel = new InvoiceModel.InvoiceCustomer()
            {
                Id = customerId
            };
            agentModel = new InvoiceModel.InvoiceAgent()
            {
                Id = agentId
            };
            shipperModel = new InvoiceModel.InvoiceShipper()
            {
                Id = shipperId
            };
        }

        [TestMethod]
        public void GetAllInvoices()
        {   
            TestHelper.InitDatabase();
            GetReady();
            var actRes = controller.Get();
            var response = actRes.ExecuteAsync(CancellationToken.None).Result;

            Assert.IsNotNull(response.Content);
        }

        [TestMethod]
        public void GetInoviceByInvoiceNo()
        {
           
            GetReady();
            var actRes = controller.GetByInvoiceNo("AG4E21");
            var response = actRes.ExecuteAsync(CancellationToken.None).Result;

            Assert.IsNotNull(response.Content);
        }

        [TestMethod]
        public void GetInoviceByWrongInvoiceNo()
        {
         
            GetReady();
            var actRes = controller.GetByInvoiceNo("12345");
            var response = actRes.ExecuteAsync(CancellationToken.None).Result;

            Assert.IsFalse(response.IsSuccessStatusCode);
        }

        [TestMethod]
        public void GetInvoiceById()
        {
            GetReady();
            var actRes = controller.GetById(1);
            var response = actRes.ExecuteAsync(CancellationToken.None).Result;

            Assert.IsNotNull(response.Content);
        }

        [TestMethod]
        public void GetInvoiceByWrongId()
        {
            GetReady();
            var actRes = controller.GetById(999);
            var response = actRes.ExecuteAsync(CancellationToken.None).Result;

            Assert.IsNull(response.Content);
        }

        [TestMethod]
        public void GetInvoicesByCustomerGood()
        {
            GetReady();
            var actRes = controller.GetByInvoicesByCustomerId(1);
            var response = actRes.ExecuteAsync(CancellationToken.None).Result;

            Assert.IsNotNull(response.Content);
        }

        [TestMethod]
        public void GetInvoicesByCustomerBad()
        {
            GetReady();
            var actRes = controller.GetByInvoicesByCustomerId(999);
            var response = actRes.ExecuteAsync(CancellationToken.None).Result;

            Assert.IsNull(response.Content);
        }

        [TestMethod]
        public void GetInvoicesByAgentGood()
        {
            GetReady();
            var actRes = controller.GetByInvoicesByAgentId(1);
            var response = actRes.ExecuteAsync(CancellationToken.None).Result;

            Assert.IsNotNull(response.Content);
        }

        [TestMethod]
        public void GetInvoicesByAgentBad()
        {
            GetReady();
            var actRes = controller.GetByInvoicesByAgentId(999);
            var response = actRes.ExecuteAsync(CancellationToken.None).Result;

            Assert.IsNull(response.Content);
        }

        [TestMethod]
        public void PostInvoiceAllGood()
        {
            GetReady();
            setUpData(1, 1, 1, 1);
            var actRes = controller.Post(new InvoiceModel()
            {
                InvoiceNo = "EF3E67",
                Vat = 17,
                Status = 0,
                Shipping = 250,
                Date = DateTime.UtcNow,
                Shipper = shipperModel,
                Customer = customerModel,
                Agent = agentModel
            });
            var response = actRes.ExecuteAsync(CancellationToken.None).Result;

            Assert.IsTrue(response.IsSuccessStatusCode);
        }

        [TestMethod]
        public void PostInvoiceInvoiceNoBad()
        {
            GetReady();
            setUpData(1, 1, 1, 1);

            var actRes = controller.Post(new InvoiceModel()
            {
                InvoiceNo = "AG4E21",
                Vat = 17,
                Status = 0,
                Shipping = 250,
                Date = DateTime.UtcNow,
                Shipper = shipperModel,
                Customer = customerModel,
                Agent = agentModel
            });
            var response = actRes.ExecuteAsync(CancellationToken.None).Result;

            Assert.IsFalse(response.IsSuccessStatusCode);
        }

        [TestMethod]
        public void PostInvoiceForAgentBad()
        {
            GetReady();
            setUpData(1, 1, 999, 1);
            
            var actRes = controller.Post(new InvoiceModel()
            {
                InvoiceNo = "EF3E67",
                Vat = 17,
                Status = 0,
                Shipping = 250,
                Date = DateTime.UtcNow,
                Shipper = shipperModel,
                Customer = customerModel,
                Agent = agentModel
            });
            var response = actRes.ExecuteAsync(CancellationToken.None).Result;

            Assert.IsFalse(response.IsSuccessStatusCode);
        }

        [TestMethod]
        public void PostInvoiceForCustomerBad()
        {
            GetReady();
            setUpData(1, 999, 1, 1);
            var actRes = controller.Post(new InvoiceModel()
            {
                InvoiceNo = "EF3E67",
                Vat = 17,
                Status = 0,
                Shipping = 250,
                Date = DateTime.UtcNow,
                Shipper = shipperModel,
                Customer = customerModel,
                Agent = agentModel
            });
            var response = actRes.ExecuteAsync(CancellationToken.None).Result;

            Assert.IsFalse(response.IsSuccessStatusCode);
        }

        [TestMethod]
        public void PostInvoiceForShipperBad()
        {
            GetReady();
            setUpData(1, 1, 1, 999);
            var actRes = controller.Post(new InvoiceModel()
            {
                InvoiceNo = "EF3E67",
                Vat = 17,
                Status = 0,
                Shipping = 250,
                Date = DateTime.UtcNow,
                Shipper = shipperModel,
                Customer = customerModel,
                Agent = agentModel
            });
            var response = actRes.ExecuteAsync(CancellationToken.None).Result;

            Assert.IsFalse(response.IsSuccessStatusCode);
        }

        [TestMethod]
        public void PutInvoiceForAgentGood()
        {
            GetReady("julia");
            setUpData(1, 1, 2, 1);

            var actRes = controller.Put(1, new InvoiceModel()
            {
                Id = 1,
                InvoiceNo = "AG4E21",
                Vat = 17,
                Status = 0,
                Shipping = 250,
                Date = DateTime.UtcNow,
                Shipper = shipperModel,
                Customer = customerModel,
                Agent = agentModel
            });
            var response = actRes.ExecuteAsync(CancellationToken.None).Result;

            Assert.IsTrue(response.IsSuccessStatusCode);
        }

        [TestMethod]
        public void PutInvoiceForAgentBad()
        {
            GetReady();
            setUpData(1, 1, 999, 1);

            var actRes = controller.Put(1, new InvoiceModel()
            {
                Id = 1,
                InvoiceNo = "AG4E21",
                Vat = 17,
                Status = 0,
                Shipping = 250,
                Date = DateTime.UtcNow,
                Shipper = shipperModel,
                Customer = customerModel,
                Agent = agentModel
            });
            var response = actRes.ExecuteAsync(CancellationToken.None).Result;

            Assert.IsFalse(response.IsSuccessStatusCode);
        }

        [TestMethod]
        public void PutInvoiceForCustomerGood()
        {
            GetReady("antonio");
            setUpData(1, 2, 1, 1);
          
            var actRes = controller.Put(1, new InvoiceModel()
            {
                Id = 1,
                InvoiceNo = "AG4E21",
                Vat = 17,
                Status = 0,
                Shipping = 250,
                Date = DateTime.UtcNow,
                Shipper = shipperModel,
                Customer = customerModel,
                Agent = agentModel
            });
            var response = actRes.ExecuteAsync(CancellationToken.None).Result;

            Assert.IsTrue(response.IsSuccessStatusCode);
        }

        [TestMethod]
        public void PutInvoiceForCustomerBad()
        {
            GetReady();
            setUpData(1, 999, 1, 1);
            
            var actRes = controller.Put(1, new InvoiceModel()
            {
                Id = 1,
                InvoiceNo = "AG4E21",
                Vat = 17,
                Status = 0,
                Shipping = 250,
                Date = DateTime.UtcNow,
                Shipper = shipperModel,
                Customer = customerModel,
                Agent = agentModel
            });
            var response = actRes.ExecuteAsync(CancellationToken.None).Result;

            Assert.IsFalse(response.IsSuccessStatusCode);
        }

        [TestMethod]
        public void PutInvoiceDataShipping()
        {
            GetReady("antonio");
            setUpData(1, 1, 1, 1);
            var actRes = controller.Put(1, new InvoiceModel()
            {
                Id = 1,
                InvoiceNo = "AG4E21",
                Vat = 17,
                Status = 0,
                Shipping = 999,
                Date = DateTime.UtcNow,
                Shipper = shipperModel,
                Customer = customerModel,
                Agent = agentModel
            });
            var response = actRes.ExecuteAsync(CancellationToken.None).Result;

            Assert.IsTrue(response.IsSuccessStatusCode);
        }

        [TestMethod]
        public void DeleteInvoiceGood()
        {
            GetReady("antonio");
            var actRes = controller.Delete(3);
            var response = actRes.ExecuteAsync(CancellationToken.None).Result;

            Assert.IsTrue(response.IsSuccessStatusCode);
        }

        [TestMethod]
        public void DeleteInvoiceBad()
        {
            GetReady();
            var actRes = controller.Delete(999);
            var response = actRes.ExecuteAsync(CancellationToken.None).Result;

            Assert.IsFalse(response.IsSuccessStatusCode);
        }

        [TestMethod]
        public void DeleteInvoiceWithItems()
        {
            GetReady();
            var actRes = controller.Delete(1);
            var response = actRes.ExecuteAsync(CancellationToken.None).Result;

            Assert.IsFalse(response.IsSuccessStatusCode);
        }

        [TestMethod]
        public void GetHistoryInvoicesById()
        {

            GetReady();
            var actRes = controller.GetHistoryInvoicesById(1);
            var response = actRes.ExecuteAsync(CancellationToken.None).Result;

            Assert.IsNotNull(response.Content);
        }


    }
}
