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
using URent.Models.Model;

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
            option = kernel.Get<OptionManager>();
            client = kernel.Get<ClientManager>();
        }

        [TestMethod]
        public void GenerateDataClient()
        {
            var result = client.Generate();
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void CreateOneClient()
        {
            var model = new Client();
            model.FirstName = "URent";
            model.Surname = "Surname";
            model.Password = "password";
            model.Email = "test@email.com";
            var result = client.CreateUpdate(model);
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void ModifyOneClient()
        {
            var model = new Client();
            model.ClientId = 5;
            model.FirstName = "URent2";
            model.Surname = "Surname2";
            model.Password = "password2";
            model.Email = "test2@email.com";
            var result = client.CreateUpdate(model);
            var obj = client.ListClient(5);
            Assert.IsTrue(result && obj.Email == "test2@email.com");
        }

        [TestMethod]
        public void CorrectAuthentificationUser()
        {
            var model = new Client();
            model.Email = "test2@email.com";
            model.Password = "password2";
            var result = client.Authentification(model);
            Assert.IsTrue(result.GetType() == typeof(Client) && result.ClientId == 5);
        }
    }
}