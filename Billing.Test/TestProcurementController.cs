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

        [TestInitialize]
        public void InitTest()
        {
            using (BillingContext context = new BillingContext())
            {
                context.Database.Delete();
                context.Database.Create();

                Category category1 = new Category()
                {
                    Name = "Monitor"
                };

                context.Categories.Add(category1);
                context.SaveChanges();

                Product product1 = new Product()
                {
                    Name = "Monitor LCD 5566",
                    Unit = "pcs",
                    Price = 332,
                    Category = category1,
                    Stock = new Stock()
                };

                Product product2 = new Product()
                {
                    Name = "Monitor LG 6699",
                    Unit = "pcs",
                    Price = 444,
                    Category = category1,
                    Stock = new Stock()

                };

                context.Products.Add(product1);
                context.Products.Add(product2);
                context.SaveChanges();

                Town town1 = new Town()
                {
                    Name = "Sarajevo",
                    Region = Region.Sarajevo,
                    Zip = "71300"
                };

                context.Towns.Add(town1);
                context.SaveChanges();

                Supplier supplier1 = new Supplier()
                {
                    Name = "OTOKA&CO",
                    Address = "Safeta Sušića 11",
                    Town = town1
                };

                Supplier supplier2 = new Supplier()
                {
                    Name = "BUTMIR&CO",
                    Address = "Hiseta Adzije 12",
                    Town = town1
                };

                context.Suppliers.Add(supplier1);
                context.Suppliers.Add(supplier2);
                context.SaveChanges();

                Procurement procurement1 = new Procurement()
                {
                    Quantity = 3,
                    Price = 77,
                    Date = DateTime.UtcNow,
                    Document = "08000001", 
                    Product = product1,
                    Supplier = supplier1
                };

                Procurement procurement2 = new Procurement()
                {
                    Quantity = 5,
                    Price = 123,
                    Date = DateTime.UtcNow,
                    Document = "09000001",
                    Product = product2,
                    Supplier = supplier2
                };

                context.Procurements.Add(procurement1);
                context.Procurements.Add(procurement2);
                context.SaveChanges();

            }
        }

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
            InitTest();
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
            InitTest();
            GetReady();
            var actRes = controller.GetByDocument("08000001");
            var response = actRes.ExecuteAsync(CancellationToken.None).Result;

            Assert.IsNotNull(response.Content);
        }

        [TestMethod]
        public void GetProcurementsByDocumentBad()
        {
            InitTest();
            GetReady();
            var actRes = controller.GetByDocument("A456");
            var response = actRes.ExecuteAsync(CancellationToken.None).Result;

            Assert.IsFalse(response.IsSuccessStatusCode);
        }

        [TestMethod]
        public void GetProcurementsByProductGood()
        {
            GetReady();
            var actRes = controller.GetByProductId(1);
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
