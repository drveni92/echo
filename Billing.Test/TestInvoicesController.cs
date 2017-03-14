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

        [TestInitialize]
        public void Init()
        {
            using (BillingContext context = new BillingContext())
            {
                context.Database.Delete();
                context.Database.Create();

                Product product = new Product()
                {
                    Name = "Laptop",
                    Price = 25.5,
                    Stock = new Stock(),
                    Unit = "pcs",
                    Category = new Category() { Name = "Laptops" }
                };

                context.Products.Add(product);
                context.SaveChanges();

                Item item = new Item()
                {
                    Price = 25.5,
                    Quantity = 5,
                    Product = product
                };

                context.Items.Add(item);
                context.SaveChanges();

                Town town = new Town()
                {
                    Name = "Sarajevo",
                    Region = Region.Sarajevo,
                    Zip = "7100"
                };

                context.Towns.Add(town);
                context.SaveChanges();

                Shipper shipper = new Shipper()
                {
                    Name = "ShipperName",
                    Address = "Unknown",
                    Town = town
                };

                context.Shippers.Add(shipper);
                context.SaveChanges();

                Customer customer = new Customer()
                {
                    Name = "Amer",
                    Address = "Sarajevo bb",
                    Town = town
                };

                Customer customer2 = new Customer()
                {
                    Name = "Dejan",
                    Address = "Sarajevo bb",
                    Town = town
                };

                context.Customers.Add(customer);
                context.Customers.Add(customer2);
                context.SaveChanges();

                Agent agent = new Agent()
                {
                    Name = "Dejan",
                    Towns = new List<Town>() { town }
                };

                Agent agent2 = new Agent()
                {
                    Name = "Amer",
                    Towns = new List<Town>() { town }
                };

                context.Agents.Add(agent);
                context.Agents.Add(agent2);
                context.SaveChanges();

                Invoice invoice = new Invoice()
                {
                    InvoiceNo = "AG4E21",
                    Vat = 17,
                    Status = 0,
                    Shipping = 250,
                    Date = DateTime.UtcNow,
                    Items = new List<Item>() { item },
                    Shipper = shipper,
                    Customer = customer,
                    Agent = agent
                };

                Invoice invoice2 = new Invoice()
                {
                    InvoiceNo = "EOI231",
                    Vat = 17,
                    Status = 0,
                    Shipping = 250,
                    Date = DateTime.UtcNow,
                    Shipper = shipper,
                    Customer = customer,
                    Agent = agent
                };

                context.Invoices.Add(invoice);
                context.Invoices.Add(invoice2);
                context.SaveChanges();
            }
        }

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
            Init();
            GetReady();
            var actRes = controller.Get();
            var response = actRes.ExecuteAsync(CancellationToken.None).Result;

            Assert.IsNotNull(response.Content);
        }

        [TestMethod]
        public void GetInoviceByInvoiceNo()
        {
            Init();
            GetReady();
            var actRes = controller.GetByInvoiceNo("invoiceno/AG4E21");
            var response = actRes.ExecuteAsync(CancellationToken.None).Result;

            Assert.IsNotNull(response.Content);
        }

        [TestMethod]
        public void GetInoviceByWrongInvoiceNo()
        {
            Init();
            GetReady();
            var actRes = controller.GetByInvoiceNo("invoiceno/12345");
            var response = actRes.ExecuteAsync(CancellationToken.None).Result;

            Assert.IsNotNull(response.Content);
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
            var actRes = controller.Delete(2);
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


    }
}
