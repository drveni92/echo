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

namespace Billing.Test
{
    [TestClass]
    public class TestInvoicesController
    {
        InvoicesController controller = new InvoicesController();
        HttpConfiguration config = new HttpConfiguration();

        HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, "api/invoices");


        void GetReady()
        {
            var route = config.Routes.MapHttpRoute("default", "api/{controller}/{id}");
            var routeData = new HttpRouteData(route, new HttpRouteValueDictionary { { "controller", "invoices" } });

            controller.ControllerContext = new HttpControllerContext(config, routeData, request);
            controller.Request = request;
            controller.Request.Properties[HttpPropertyKeys.HttpConfigurationKey] = config;
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
            TownModel townModel = new TownModel()
            {
                Id = 1
            };
            InvoiceModel.InvoiceCustomer customerModel = new InvoiceModel.InvoiceCustomer()
            {
                Id = 1
            };
            InvoiceModel.InvoiceAgent agentModel = new InvoiceModel.InvoiceAgent()
            {
                Id = 1
            };
            InvoiceModel.InvoiceShipper shipperModel = new InvoiceModel.InvoiceShipper()
            {
                Id = 1
            };

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
            TownModel townModel = new TownModel()
            {
                Id = 1
            };
            InvoiceModel.InvoiceCustomer customerModel = new InvoiceModel.InvoiceCustomer()
            {
                Id = 1
            };
            InvoiceModel.InvoiceAgent agentModel = new InvoiceModel.InvoiceAgent()
            {
                Id = 1
            };
            InvoiceModel.InvoiceShipper shipperModel = new InvoiceModel.InvoiceShipper()
            {
                Id = 1
            };

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
            TownModel townModel = new TownModel()
            {
                Id = 1
            };
            InvoiceModel.InvoiceCustomer customerModel = new InvoiceModel.InvoiceCustomer()
            {
                Id = 1
            };
            InvoiceModel.InvoiceAgent agentModel = new InvoiceModel.InvoiceAgent()
            {
                Id = 999
            };
            InvoiceModel.InvoiceShipper shipperModel = new InvoiceModel.InvoiceShipper()
            {
                Id = 1
            };

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
            TownModel townModel = new TownModel()
            {
                Id = 1
            };
            InvoiceModel.InvoiceCustomer customerModel = new InvoiceModel.InvoiceCustomer()
            {
                Id = 999
            };
            InvoiceModel.InvoiceAgent agentModel = new InvoiceModel.InvoiceAgent()
            {
                Id = 1
            };
            InvoiceModel.InvoiceShipper shipperModel = new InvoiceModel.InvoiceShipper()
            {
                Id = 1
            };

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
            TownModel townModel = new TownModel()
            {
                Id = 1
            };
            InvoiceModel.InvoiceCustomer customerModel = new InvoiceModel.InvoiceCustomer()
            {
                Id = 1
            };
            InvoiceModel.InvoiceAgent agentModel = new InvoiceModel.InvoiceAgent()
            {
                Id = 1
            };
            InvoiceModel.InvoiceShipper shipperModel = new InvoiceModel.InvoiceShipper()
            {
                Id = 999
            };

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
            GetReady();

            TownModel townModel = new TownModel()
            {
                Id = 1
            };
            InvoiceModel.InvoiceCustomer customerModel = new InvoiceModel.InvoiceCustomer()
            {
                Id = 1
            };
            InvoiceModel.InvoiceAgent agentModel = new InvoiceModel.InvoiceAgent()
            {
                Id = 2
            };
            InvoiceModel.InvoiceShipper shipperModel = new InvoiceModel.InvoiceShipper()
            {
                Id = 1
            };

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

            TownModel townModel = new TownModel()
            {
                Id = 1
            };
            InvoiceModel.InvoiceCustomer customerModel = new InvoiceModel.InvoiceCustomer()
            {
                Id = 1
            };
            InvoiceModel.InvoiceAgent agentModel = new InvoiceModel.InvoiceAgent()
            {
                Id = 999
            };
            InvoiceModel.InvoiceShipper shipperModel = new InvoiceModel.InvoiceShipper()
            {
                Id = 1
            };

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
            GetReady();

            TownModel townModel = new TownModel()
            {
                Id = 1
            };
            InvoiceModel.InvoiceCustomer customerModel = new InvoiceModel.InvoiceCustomer()
            {
                Id = 2
            };
            InvoiceModel.InvoiceAgent agentModel = new InvoiceModel.InvoiceAgent()
            {
                Id = 1
            };
            InvoiceModel.InvoiceShipper shipperModel = new InvoiceModel.InvoiceShipper()
            {
                Id = 1
            };

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

            TownModel townModel = new TownModel()
            {
                Id = 1
            };
            InvoiceModel.InvoiceCustomer customerModel = new InvoiceModel.InvoiceCustomer()
            {
                Id = 999
            };
            InvoiceModel.InvoiceAgent agentModel = new InvoiceModel.InvoiceAgent()
            {
                Id = 1
            };
            InvoiceModel.InvoiceShipper shipperModel = new InvoiceModel.InvoiceShipper()
            {
                Id = 1
            };

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
            GetReady();

            TownModel townModel = new TownModel()
            {
                Id = 1
            };
            InvoiceModel.InvoiceCustomer customerModel = new InvoiceModel.InvoiceCustomer()
            {
                Id = 1
            };
            InvoiceModel.InvoiceAgent agentModel = new InvoiceModel.InvoiceAgent()
            {
                Id = 1
            };
            InvoiceModel.InvoiceShipper shipperModel = new InvoiceModel.InvoiceShipper()
            {
                Id = 1
            };

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
            GetReady();
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
