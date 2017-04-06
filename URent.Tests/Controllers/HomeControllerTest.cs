using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using URent;
using URent.Controllers;
using URent.Models.Interfaces;
using URent.Models.Manager;
using URent.App_Start;
using Ninject;

namespace URent.Tests.Controllers
{
    [TestClass]
    public class HomeControllerTest
    {
        private  ICategory category;
        private ISearch search;
        private IOption option;
        private IClient client;
        private IRentPrice price;
        private IReservation reservation;

        [TestInitialize]
        public void MyTestInitialize()
        {
            IKernel kernel = new StandardKernel();
            category = kernel.Get<CategoryManager>();
        }

        [TestMethod]
        public void ListAllCategories()
        {
            //var category = new CategoryManager();
            var list = category.ListCategories();
            Assert.IsTrue(list.Count > 0);
        }
    }
}