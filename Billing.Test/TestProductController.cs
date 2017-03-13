﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using Billing.Api.Controllers;
using System.Web.Http;
using System.Threading;
using System.Net.Http;
using System.Web.Http.Routing;
using System.Web.Http.Controllers;
using System.Web.Http.Hosting;
using Billing.Database;
using Billing.Api.Models;

namespace Billing.Tests
{
    [TestClass]
    public class TestProductsController
    {
        ProductsController controller = new ProductsController();
        HttpConfiguration config = new HttpConfiguration();

        HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, "api/products");

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

                Category category2 = new Category()
                {
                    Name = "Projector"
                };

                context.Categories.Add(category1);
                context.Categories.Add(category2);
                context.SaveChanges();

                Product product1 = new Product()
                {
                    Name = "Monitor LCD 6557",
                    Unit = "pcs",
                    Price = 609,
                    Category = category1,
                    Stock = new Stock()
                };

                Product product2 = new Product()
                {
                    Name = "Monitor LCD 4489",
                    Unit = "pcs",
                    Price = 529,
                    Category = category1,
                    Stock = new Stock()
                };

                Product product3 = new Product()
                {
                    Name = "Projector LCD 7788",
                    Unit = "pcs",
                    Price = 529,
                    Category = category2,
                    Stock = new Stock()
                };

                context.Products.Add(product1);
                context.Products.Add(product2);
                context.Products.Add(product3);
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
        public void GetAllProducts()
        {
            InitTest();
            GetReady();
            var actRes = controller.Get();
            var response = actRes.ExecuteAsync(CancellationToken.None).Result;

            Assert.IsNotNull(response.Content);
        }

        [TestMethod]
        public void GetProductById()
        {
            GetReady();
            var actRes = controller.Get(1);
            var response = actRes.ExecuteAsync(CancellationToken.None).Result;

            Assert.IsNotNull(response.Content);
        }

        [TestMethod]
        public void GetProductByWrongId()
        {
            GetReady();
            var actRes = controller.Get(999);
            var response = actRes.ExecuteAsync(CancellationToken.None).Result;

            Assert.IsNull(response.Content);
        }

        [TestMethod]
        public void PostProductGood()
        {
            GetReady();
            var actRes = controller.Post(new ProductModel() { Name = "Projector LCD 6993", Unit = "pcs", Price = 634, Category = new ProductModel.ProductCategory() { Id = 1 } });
            var response = actRes.ExecuteAsync(CancellationToken.None).Result;

            Assert.IsTrue(response.IsSuccessStatusCode);
        }

        [TestMethod]
        public void PostProductBad()
        {
            GetReady();
            var actRes = controller.Post(new ProductModel() { Name = "MP3 Player 6580", Unit = "pcs", Price = 100, Category = new ProductModel.ProductCategory() { Id = 999 } });
            var response = actRes.ExecuteAsync(CancellationToken.None).Result;

            Assert.IsFalse(response.IsSuccessStatusCode);
        }

        [TestMethod]
        public void ChangeName()
        {
            GetReady();
            var actRes = controller.Put(1, new ProductModel() { Id = 1, Name = "Monitor LCD 6557", Unit = "pcs", Price = 609, Category = new ProductModel.ProductCategory() { Id = 1 } });
            var response = actRes.ExecuteAsync(CancellationToken.None).Result;

            Assert.IsTrue(response.IsSuccessStatusCode);
        }

        [TestMethod]
        public void ChangeCategory()
        {
            GetReady();
            var actRes = controller.Put(1, new ProductModel() { Id = 1, Name = "Monitor LCD 6557", Unit = "pcs", Price = 609, Category = new ProductModel.ProductCategory() { Id = 2 } });
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