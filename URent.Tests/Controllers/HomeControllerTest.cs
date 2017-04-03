using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using URent;
using URent.Controllers;
using URent.Models.Interfaces;

namespace URent.Tests.Controllers
{
    [TestClass]
    public class HomeControllerTest
    {
        private readonly ICategory category;
        private readonly ISearch search;
        private readonly IOption option;
        private readonly IClient client;
        private readonly IRentPrice price;
        private readonly IReservation reservation;

        [TestMethod]
        public void Index()
        {
            // Arrange
            HomeController controller = new HomeController(category, search, option, client, price, reservation);

            // Act
            ViewResult result = controller.Index() as ViewResult;

            // Assert
            Assert.IsNotNull(result);
        }
    }
}