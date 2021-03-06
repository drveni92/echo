﻿using System;
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
using static Billing.API.Models.ShipperModel;

namespace Billing.Test
{
    [TestClass]
    public class TestShipperController
    {
        ShippersController controller = new ShippersController();
        HttpConfiguration config = new HttpConfiguration();

        HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, "api/agents");
   

        void GetReady()
        {
            var route = config.Routes.MapHttpRoute("default", "api/{controller}/{id}");
            var routeData = new HttpRouteData(route, new HttpRouteValueDictionary { { "controller", "shippers" } });

            controller.ControllerContext = new HttpControllerContext(config, routeData, request);
            controller.Request = request;
            controller.Request.Properties[HttpPropertyKeys.HttpConfigurationKey] = config;
        }

        [TestMethod]
        public void GetAllShippers()
        {
            TestHelper.InitDatabase();
            GetReady();
            var actRes = controller.Get();
            var response = actRes.ExecuteAsync(CancellationToken.None).Result;

            Assert.IsNotNull(response.Content);
        }

        [TestMethod]
        public void GetShippersByName()
        {
            GetReady();
            var actRes = controller.Get("Posta");
            var response = actRes.ExecuteAsync(CancellationToken.None).Result;
            Assert.IsNotNull(response.Content);
        }

        [TestMethod]
        public void GetShippersByTown()
        {
            GetReady();
            var actRes = controller.GetShippersByTown(1);
            var response = actRes.ExecuteAsync(CancellationToken.None).Result;
            Assert.IsNotNull(response.Content);
        }

        [TestMethod]
        public void GetShipperById()
        {
            GetReady();
            var actRes = controller.GetById(1);
            var response = actRes.ExecuteAsync(CancellationToken.None).Result;

            Assert.IsNotNull(response.Content);
        }

        [TestMethod]
        public void GetShipperByWrongId()
        {
            GetReady();
            var actRes = controller.GetById(999);
            var response = actRes.ExecuteAsync(CancellationToken.None).Result;

            Assert.IsNull(response.Content);
        }

        [TestMethod]
        public void PostShipperGood()
        {
            GetReady();
            var actRes = controller.Post(new ShipperModel() { Name = "Posta", Address = "Milana Preloga /3", Town = new ShipperTown() {  Id = 1}  });
            var response = actRes.ExecuteAsync(CancellationToken.None).Result;

            Assert.IsTrue(response.IsSuccessStatusCode);
        }

        [TestMethod]
        public void ChangeName()
        {
            GetReady();
            var actRes = controller.Put(1, new ShipperModel() { Id = 1, Name = "Posta", Address = "Milana Preloga /3", Town = new ShipperTown() { Id = 1 } });
            var response = actRes.ExecuteAsync(CancellationToken.None).Result;

            Assert.IsTrue(response.IsSuccessStatusCode);
        }

        [TestMethod]
        public void ChangeAdress()
        {
            GetReady();
            var actRes = controller.Put(1, new ShipperModel() { Id = 1, Name = "Posta", Address = "Zmaja od Bosne bb", Town = new ShipperTown() { Id = 1 } });
            var response = actRes.ExecuteAsync(CancellationToken.None).Result;

            Assert.IsTrue(response.IsSuccessStatusCode);
        }

        [TestMethod]
        public void ChangeTown()
        {
            GetReady();
            var actRes = controller.Put(1, new ShipperModel() { Id = 1, Name = "Posta", Address = "Zmaja od Bosne bb", Town = new ShipperTown() { Id = 2 } });
            var response = actRes.ExecuteAsync(CancellationToken.None).Result;

            Assert.IsTrue(response.IsSuccessStatusCode);
        }

        [TestMethod]
        public void DeleteById()
        {
            GetReady();
            var actRes = controller.Delete(3);
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
