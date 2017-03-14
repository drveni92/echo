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
    public class TestCustomersController
    {
        CustomersController controller = new CustomersController();
        HttpConfiguration config = new HttpConfiguration();
        HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, "api/customers");

        [TestInitialize]
        void Init()
        {
            using (BillingContext context = new BillingContext())
            {
                context.Database.Delete();
                context.Database.Create();

                Town town = new Town()
                {
                    Name = "Sarajevo",
                    Region = Region.Sarajevo,
                    Zip = "7100"
                };

                Town town2 = new Town()
                {
                    Name = "Zenica",
                    Region = Region.Zenica,
                    Zip = "71241"
                };

                context.Towns.Add(town);
                context.Towns.Add(town2);
                context.SaveChanges();

                Customer customer = new Customer()
                {
                    Name = "Džeko",
                    Address = "Adresa bb",
                    Town = town
                };

                Customer customer2 = new Customer()
                {
                    Name = "Pjanić",
                    Address = "Adresa bb",
                    Town = town
                };

                context.Customers.Add(customer);
                context.Customers.Add(customer2);
                context.SaveChanges();
            }
        }

        void GetReady()
        {
            var route = config.Routes.MapHttpRoute("default", "api/{controller}/{id}");
            var routeData = new HttpRouteData(route, new HttpRouteValueDictionary { { "controller", "customers" } });

            controller.ControllerContext = new HttpControllerContext(config, routeData, request);
            controller.Request = request;
            controller.Request.Properties[HttpPropertyKeys.HttpConfigurationKey] = config;
        }

        [TestMethod]
        public void GetAllCustomers()
        {
            Init();
            GetReady();
            var actRes = controller.Get();
            var response = actRes.ExecuteAsync(CancellationToken.None).Result;

            Assert.IsNotNull(response.Content);
        }

        [TestMethod]
        public void GetCustomersByName()
        {
            GetReady();
            var actRes = controller.Get("džeko");
            var response = actRes.ExecuteAsync(CancellationToken.None).Result;

            Assert.IsNotNull(response.Content);
        }

        [TestMethod]
        public void GetCustomersById()
        {
            GetReady();
            var actRes = controller.GetById(1);
            var response = actRes.ExecuteAsync(CancellationToken.None).Result;

            Assert.IsNotNull(response.Content);
        }

        [TestMethod]
        public void GetCustomersByWrongId()
        {
            GetReady();
            var actRes = controller.GetById(999);
            var response = actRes.ExecuteAsync(CancellationToken.None).Result;

            Assert.IsNull(response.Content);
        }

        [TestMethod]
        public void GetCustomersByTown()
        {
            GetReady();
            var actRes = controller.GetByTown(1);
            var response = actRes.ExecuteAsync(CancellationToken.None).Result;

            Assert.IsNotNull(response.Content);
        }

        [TestMethod]
        public void GetCustomersByWrongTown()
        {
            GetReady();
            var actRes = controller.GetByTown(999);
            var response = actRes.ExecuteAsync(CancellationToken.None).Result;

            Assert.IsFalse(response.IsSuccessStatusCode);
        }

        [TestMethod]
        public void PostCustomerGood()
        {
            GetReady();
            var actRes = controller.Post(new CustomerModel() { Name = "Amer", Town = new CustomerModel.CustomerTown() { Id = 1 } });
            var response = actRes.ExecuteAsync(CancellationToken.None).Result;

            Assert.IsTrue(response.IsSuccessStatusCode);
        }

        [TestMethod]
        public void PostAgentBad()
        {
            GetReady();
            var actRes = controller.Post(new CustomerModel() { Name = "Amer", Town = new CustomerModel.CustomerTown() { Id = 999 } });
            var response = actRes.ExecuteAsync(CancellationToken.None).Result;

            Assert.IsFalse(response.IsSuccessStatusCode);
        }

        [TestMethod]
        public void ChangeCustomerName()
        {
            GetReady();
            var actRes = controller.Put(1, new CustomerModel() { Id = 1, Name = "Amer", Town = new CustomerModel.CustomerTown() { Id = 1 } });
            var response = actRes.ExecuteAsync(CancellationToken.None).Result;

            Assert.IsTrue(response.IsSuccessStatusCode);
        }

        [TestMethod]
        public void ChangeCustomerTown()
        {
            GetReady();
            var actRes = controller.Put(1, new CustomerModel() { Id = 1, Name = "Džeko", Town = new CustomerModel.CustomerTown() { Id = 2 } });
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
