using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using URent.Models.Interfaces;
using URent.Models.Manager;
using Ninject;
using URent.Models.Model;

namespace URent.Tests.Controllers
{
    [TestClass]
    public class HomeControllerTest
    {
        private ICategory category;
        private ICar car;
        private ISearch search;
        private IOption option;
        private IClient client;
        private IRentPrice price;
        private IReservation reservation;
        private IUser user;
        private IRent rent;

        [TestInitialize]
        public void MyTestInitialize()
        {
            IKernel kernel = new StandardKernel();
            category = kernel.Get<CategoryManager>();
            car = kernel.Get<CarManager>();
            option = kernel.Get<OptionManager>();
            client = kernel.Get<ClientManager>();
            search = kernel.Get<SearchManager>();
            price = kernel.Get<RentPriceManager>();
            reservation = kernel.Get<ReservationManager>();
            user = kernel.Get<UserManager>();
            rent = kernel.Get<RentManager>();
        }

        [TestMethod]
        public void GenerateDataCategory()
        {
            var result = category.Generate();
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void GenerateDataOption()
        {
            var result = option.Generate();
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void GenerateDataClient()
        {
            var result = client.Generate();
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void GenerateDataCar()
        {
            var result = car.Generate();
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void GenerateDataPrice()
        {
            var result = price.Generate();
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void GenerateDataReservation()
        {
            var result = reservation.Generate();
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void GenerateDataUserAdmin()
        {
            var result = user.Generate();
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void ListAllCategories()
        {
            var list = category.ListCategories();
            Assert.IsTrue(list.Count > 0);
        }

        [TestMethod]
        public void ListCategoryById()
        {
            var obj = category.ListCategory(1);
            Assert.IsTrue(obj.CategoryId == 1 && obj.Name.ToUpper() == "COMPACT");
        }

        [TestMethod]
        public void ListAllOptions()
        {
            var list = option.ListOptions();
            Assert.IsTrue(list.Count > 0);
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
        public void SuccessfulAuthentificationClient()
        {
            var model = new Client();
            model.Email = "test2@email.com";
            model.Password = "password2";
            var result = client.Authentification(model);
            Assert.IsTrue(result.GetType() == typeof(Client) && result.ClientId == 5);
        }

        [TestMethod]
        public void UnsuccessfulAuthentificationClient()
        {
            var model = new Client();
            model.Email = "noexist@email.com";
            model.Password = "noexist";
            var result = client.Authentification(model);
            Assert.IsTrue(result == null);
        }

        [TestMethod]
        public void DeleteClientExist()
        {
            var result = client.Remove(5);
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void DeleteClientDoesNotExist()
        {
            var result = client.Remove(100);
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void ListAllAvailablesCategories()
        {
            var date1 = DateTime.Parse("2017-03-30");
            var date2 = DateTime.Parse("2017-03-30");
            var result = search.SearchAvailableCategories(date1, date2, 0);
            Assert.IsTrue(result.Count == 3);
        }

        [TestMethod]
        public void ListAvailablesCategoriesWithoutCategory2()
        {
            var date1 = DateTime.Parse("2017-04-01");
            var date2 = DateTime.Parse("2017-04-01");
            var result = search.SearchAvailableCategories(date1, date2, 0);
            Assert.IsFalse(result.Any(c => c.Category.CategoryId == 2));
        }

        [TestMethod]
        public void CheckIfCategoryIsAvailableTrue()
        {
            var date1 = DateTime.Parse("2017-04-01");
            var date2 = DateTime.Parse("2017-04-01");
            var result = search.CheckAvailableCategory(date1, date2, 1);
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void CheckIfCategoryIsAvailableFalse()
        {
            var date1 = DateTime.Parse("2017-04-01");
            var date2 = DateTime.Parse("2017-04-01");
            var result = search.CheckAvailableCategory(date1, date2, 2);
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void CreateReservation()
        {
            var listOption = new List<Option> { option.ListOption(1) };
            var objReservation = new Reservation
            {
                ClientId = 1,
                CarId = 1,
                DateReservation = DateTime.Now.AddDays(3),
                DateStartRent = DateTime.Now.AddDays(3),
                DateReturnRent = DateTime.Now.AddDays(4),
                Cost = 80,
                Options = listOption
            };

            var result = reservation.CreateUpdate(objReservation);
            Assert.IsTrue(result.ReservationId > 0);
        }

        [TestMethod]
        public void ListReservationWithoutRent()
        {
            var list = reservation.ListReservationsWithNoRent();
            Assert.IsTrue(list.Count > 0);
        }

        [TestMethod]
        public void TransformReservationToRent()
        {
            var reserve = reservation.ListReservation(1);
            var objRent = new Rent();
            objRent.CarId = reserve.CarId;
            objRent.ClientId = reserve.ClientId;
            objRent.Cost = reserve.Cost;
            //objRent.DateDeparture = reserve.DateStartRent;
            //objRent.DateReturn = reserve.DateReturnRent;
            objRent.DateDeparture = DateTime.Now.AddDays(3);
            objRent.DateReturn = DateTime.Now.AddDays(4);
            objRent.Options = new List<Option>();
            objRent.Options = reserve.Options;
            objRent.UserId = 1;
            objRent.ReservationId = reserve.ReservationId;
            var id = rent.CreateUpdate(objRent);
            Assert.IsTrue(id > 0);
        }

        [TestMethod]
        public void CancelReservation()
        {
            var reserv = reservation.ListReservation(1);
            reserv.Status = 0;
            var model = reservation.CreateUpdate(reserv);
            Assert.IsTrue(model.Status == 0);
        }

        [TestMethod]
        public void CancelRent()
        {
            var list = rent.ListRent();
            var obj = list[0];
            obj.Status = 0;
            var id = rent.CreateUpdate(obj);
            var list2 = rent.ListRent().Where(r => r.RentId == id).ToList();
            Assert.IsTrue(list2[0].Status == 0);
        }

        [TestMethod]
        public void ValidCancelDelayReservation()
        {
            var result = reservation.CheckCancelDelay(5);
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void InvalidCancelDelayReservation()
        {
            var result = reservation.CheckCancelDelay(4);
            Assert.IsFalse(result);
        }
    }
}