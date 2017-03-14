using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Billing.API.Controllers;
using System.Web.Http;
using System.Net.Http;
using Billing.Database;
using System.Web.Http.Routing;
using System.Web.Http.Controllers;
using System.Web.Http.Hosting;
using System.Threading;
using Billing.API.Models;

namespace Billing.Test
{
   
    [TestClass]
    public class TestItemController
    {
        ItemsController controller = new ItemsController();
        HttpConfiguration config = new HttpConfiguration();

        HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, "api/items");

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

                Product product2 = new Product()
                {
                    Name = "Notebook",
                    Price = 25,
                    Stock = new Stock(),
                    Unit = "pcs",
                    Category = new Category() { Name = "Laptops" }
                };

                context.Products.Add(product);
                context.Products.Add(product2);
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
                    Shipper = shipper,
                    Customer = customer,
                    Agent = agent
                };              

                context.Invoices.Add(invoice);        
                context.SaveChanges();


                Item item1 = new Item()
                {
                    Quantity = 10,
                    Price = 1444,
                    Product = product,
                    Invoice = invoice
                };

                Item item2 = new Item()
                {
                    Quantity = 20,
                    Price = 44,
                    Product = product,
                    Invoice = invoice
                };
      
                context.Items.Add(item1);
                context.Items.Add(item2);
                context.SaveChanges();
            }
        }

        void GetReady()
        {
            var route = config.Routes.MapHttpRoute("default", "api/{controller}/{id}");
            var routeData = new HttpRouteData(route, new HttpRouteValueDictionary { { "controller", "items" } });

            controller.ControllerContext = new HttpControllerContext(config, routeData, request);
            controller.Request = request;
            controller.Request.Properties[HttpPropertyKeys.HttpConfigurationKey] = config;
        }

        [TestMethod]
        public void GetAllItems()
        {
            Init();
            GetReady();
            var actRes = controller.Get();
            var response = actRes.ExecuteAsync(CancellationToken.None).Result;

            Assert.IsNotNull(response.Content);
        }

        [TestMethod]
        public void GetItemById()
        {
            GetReady();
            var actRes = controller.GetById(1);
            var response = actRes.ExecuteAsync(CancellationToken.None).Result;

            Assert.IsNotNull(response.Content);
        }

        [TestMethod]
        public void GetItemByWrongId()
        {
            GetReady();
            var actRes = controller.GetById(999);
            var response = actRes.ExecuteAsync(CancellationToken.None).Result;

            Assert.IsNull(response.Content);
        }

        [TestMethod]
        public void GetItemByInvoiceGood()
        {
            GetReady();
            var actRes = controller.GetItemsByInvoice(1);
            var response = actRes.ExecuteAsync(CancellationToken.None).Result;

            Assert.IsNotNull(response.Content);
        }

        [TestMethod]
        public void GetItemByInvoiceBad()
        {
            GetReady();
            var actRes = controller.GetItemsByInvoice(999);
            var response = actRes.ExecuteAsync(CancellationToken.None).Result;

            Assert.IsNull(response.Content);
        }

        [TestMethod]
        public void GetItemByProductGood()
        {
            GetReady();
            var actRes = controller.GetItemByProduct(1);
            var response = actRes.ExecuteAsync(CancellationToken.None).Result;

            Assert.IsNotNull(response.Content);
        }

        [TestMethod]
        public void GetItemByProductBad()
        {
            GetReady();
            var actRes = controller.GetItemByProduct(999);
            var response = actRes.ExecuteAsync(CancellationToken.None).Result;

            Assert.IsNull(response.Content);
        }

        [TestMethod]
        public void PostItemForProductGood()
        {
            GetReady();
            var actRes = controller.Post(new ItemModel() { Quantity = 10, Price = 256, SubTotal = 2560, Product = new ItemModel.ItemProduct() { Id = 1 }, Invoice = new ItemModel.ItemInvoice { Id=1 } });
            var response = actRes.ExecuteAsync(CancellationToken.None).Result;

            Assert.IsTrue(response.IsSuccessStatusCode);
        }

        [TestMethod]
        public void PostItemForProductBad()
        {
            GetReady();
            var actRes = controller.Post(new ItemModel() { Quantity = 10, Price = 256, SubTotal = 2560, Product = new ItemModel.ItemProduct() { Id = 555 }, Invoice = new ItemModel.ItemInvoice { Id = 1 } });
            var response = actRes.ExecuteAsync(CancellationToken.None).Result;

            Assert.IsFalse(response.IsSuccessStatusCode);
        }

        [TestMethod]
        public void ChangeItemsData()
        {
            GetReady();
            var actRes = controller.Put(1, new ItemModel() { Id=1 , Quantity = 100, Price = 256, SubTotal = 2560, Product = new ItemModel.ItemProduct() { Id = 1 }, Invoice = new ItemModel.ItemInvoice { Id = 1 } });
            var response = actRes.ExecuteAsync(CancellationToken.None).Result;

            Assert.IsTrue(response.IsSuccessStatusCode);
        }


        [TestMethod]
        public void ChangeItemsProductGood()
        {
            GetReady();
            var actRes = controller.Put(1, new ItemModel() { Id = 1, Quantity = 100, Price = 256, SubTotal = 2560, Product = new ItemModel.ItemProduct() { Id = 2 }, Invoice = new ItemModel.ItemInvoice { Id = 1 } });
            var response = actRes.ExecuteAsync(CancellationToken.None).Result;

            Assert.IsTrue(response.IsSuccessStatusCode);
        }

        [TestMethod]
        public void ChangeItemsProductBad()
        {
            GetReady();
            var actRes = controller.Put(1, new ItemModel() { Id = 1, Quantity = 100, Price = 256, SubTotal = 2560, Product = new ItemModel.ItemProduct() { Id = 999 }, Invoice = new ItemModel.ItemInvoice { Id = 1 } });
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
