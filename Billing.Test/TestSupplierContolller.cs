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
    public class TestSupplierController
    {
        SuppliersController controller = new SuppliersController();
        HttpConfiguration config = new HttpConfiguration();

        HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, "api/suppliers");

        [TestInitialize]
        public void Init()
        {
            using (BillingContext context = new BillingContext())
            {
                context.Database.Delete();
                context.Database.Create();

                Town town1 = new Town()
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

                context.Towns.Add(town1);
                context.Towns.Add(town2);
                context.SaveChanges();

                Supplier supplier1 = new Supplier()
                {
                    Name = "OTOKA COMPANY",
                    Address ="Hiseta 13",
                    Town = town1
                };

                Supplier supplier2 = new Supplier()
                {
                    Name = "HECO",
                    Address = "Lukavička cesta 11",
                    Town = town1
                };

                Supplier supplier3 = new Supplier()
                {
                    Name = "CENTAR",
                    Address = "Safeta Heđića 65a",
                    Town = town2
                };


                context.Suppliers.Add(supplier1);
                context.Suppliers.Add(supplier2);
                context.Suppliers.Add(supplier3);
                context.SaveChanges();
            }
        }

        void GetReady()
        {
            var route = config.Routes.MapHttpRoute("default", "api/{controller}/{id}");
            var routeData = new HttpRouteData(route, new HttpRouteValueDictionary { { "controller", "suppliers" } });

            controller.ControllerContext = new HttpControllerContext(config, routeData, request);
            controller.Request = request;
            controller.Request.Properties[HttpPropertyKeys.HttpConfigurationKey] = config;
        }

        [TestMethod]
        public void GetAllSuppliers()
        {
            Init();
            GetReady();
            var actRes = controller.Get();
            var response = actRes.ExecuteAsync(CancellationToken.None).Result;

            Assert.IsNotNull(response.Content);
        }

        [TestMethod]
        public void GetSuppliersByName()
        {
            GetReady();
            var actRes = controller.Get("OTOKA COMPANY");
            var response = actRes.ExecuteAsync(CancellationToken.None).Result;
            Assert.IsNotNull(response.Content);
        }

        [TestMethod]
        public void GetSuppliersByTown()
        {
            GetReady();
            var actRes = controller.Get("town/1");
            var response = actRes.ExecuteAsync(CancellationToken.None).Result;
            Assert.IsNotNull(response.Content);
        }

        [TestMethod]
        public void GetSuppliersById()
        {
            GetReady();
            var actRes = controller.Get(1);
            var response = actRes.ExecuteAsync(CancellationToken.None).Result;

            Assert.IsNotNull(response.Content);
        }

        [TestMethod]
        public void GetSuppliersByWrongId()
        {
            GetReady();
            var actRes = controller.Get(999);
            var response = actRes.ExecuteAsync(CancellationToken.None).Result;

            Assert.IsNull(response.Content);
        }

        [TestMethod]
        public void PostSupplierGood()
        {
            GetReady();
            var actRes = controller.Post(new SupplierModel() { Name = "BINGO", Address = "Hiseta Adžije 22", Town = new SupplierModel.SupplierTown() { Id = 1 } });
            var response = actRes.ExecuteAsync(CancellationToken.None).Result;

            Assert.IsTrue(response.IsSuccessStatusCode);
        }

        [TestMethod]
        public void PostSupplierBad()
        {
            GetReady();
            var actRes = controller.Post(new SupplierModel() { Name = "ORION", Address = "Milana Preloga 10", Town = new SupplierModel.SupplierTown() { Id = 999 } });
            var response = actRes.ExecuteAsync(CancellationToken.None).Result;

            Assert.IsFalse(response.IsSuccessStatusCode);
        }

        [TestMethod]
        public void ChangeName()
        {
            GetReady();
            var actRes = controller.Put(1, new SupplierModel() { Id = 1, Name = "OTOKA&CO", Address = "Hiseta 13", Town = new SupplierModel.SupplierTown() { Id = 1 } });
            var response = actRes.ExecuteAsync(CancellationToken.None).Result;

            Assert.IsTrue(response.IsSuccessStatusCode);
        }

        [TestMethod]
        public void ChangeTown()
        {
            GetReady();
            var actRes = controller.Put(1, new SupplierModel() { Id = 1, Name = "OTOKA&CO", Address = "Hiseta 13", Town = new SupplierModel.SupplierTown() { Id = 2 } });
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
