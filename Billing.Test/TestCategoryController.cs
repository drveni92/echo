using System;
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
    public class TestCategoryController
    {
        CategoriesController controller = new CategoriesController();
        HttpConfiguration config = new HttpConfiguration();

        HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, "api/categories");

        [TestInitialize]
        public void Init()
        {
            using (BillingContext context = new BillingContext())
            {
                context.Database.Delete();
                context.Database.Create();

                Category category1 = new Category()
                {
                    Name = "Desktop",
                };

                Category category2 = new Category()
                {
                    Name = "Laptop",
                };

                Category category3 = new Category()
                {
                    Name = "Printer",
                };

                context.Categories.Add(category1);
                context.Categories.Add(category2);
                context.Categories.Add(category3);
                context.SaveChanges();
          
            }
        }


        void GetReady()
        {
            var route = config.Routes.MapHttpRoute("default", "api/{controller}/{id}");
            var routeData = new HttpRouteData(route, new HttpRouteValueDictionary { { "controller", "categories" } });

            controller.ControllerContext = new HttpControllerContext(config, routeData, request);
            controller.Request = request;
            controller.Request.Properties[HttpPropertyKeys.HttpConfigurationKey] = config;
        }

        [TestMethod]
        public void GetAllSCategories()
        {
            Init();
            GetReady();
            var actRes = controller.Get();
            var response = actRes.ExecuteAsync(CancellationToken.None).Result;

            Assert.IsNotNull(response.Content);
        }
        [TestMethod]
        public void GetCategoriesByName()
        {
            GetReady();
            var actRes = controller.Get("Laptop");
            var response = actRes.ExecuteAsync(CancellationToken.None).Result;
            Assert.IsNotNull(response.Content);
        }

        [TestMethod]
        public void GetCategoryById()
        {
            GetReady();
            var actRes = controller.GetById(1);
            var response = actRes.ExecuteAsync(CancellationToken.None).Result;

            Assert.IsNotNull(response.Content);
        }

        [TestMethod]
        public void GetCategoryByWrongId()
        {
            GetReady();
            var actRes = controller.GetById(999);
            var response = actRes.ExecuteAsync(CancellationToken.None).Result;

            Assert.IsNull(response.Content);
        }


        [TestMethod]
        public void PostCategoryGood()
        {
            GetReady();
            var actRes = controller.Post(new CategoryModel() { Name = "Mp3" });
            var response = actRes.ExecuteAsync(CancellationToken.None).Result;

            Assert.IsTrue(response.IsSuccessStatusCode);
        }

        [TestMethod]
        public void ChangeName()
        {
            GetReady();
            var actRes = controller.Put(1, new CategoryModel() { Id = 1, Name = "Novi mp3" });
            var response = actRes.ExecuteAsync(CancellationToken.None).Result;

            Assert.IsTrue(response.IsSuccessStatusCode);
        }


        public void DeleteById()
        {
            GetReady();
            var actRes = controller.Delete(1);
            var response = actRes.ExecuteAsync(CancellationToken.None).Result;

            Assert.IsTrue(response.IsSuccessStatusCode);
        }

        public void DeleteByWrongId()
        {
            GetReady();
            var actRes = controller.Delete(999);
            var response = actRes.ExecuteAsync(CancellationToken.None).Result;

            Assert.IsFalse(response.IsSuccessStatusCode);
        }
    }
}
