using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using URent.Models.Interfaces;
using URent.Models.Manager;
using URent.Models.Model;

namespace URent.Tests.Controllers
{
    [TestClass]
    public class HomeControllerTest
    {
        private readonly ICategory _category;
        private readonly ICar _car;
        private readonly ISearch _search;
        private readonly IOption _option;
        private readonly IClient _client;
        private readonly IRentPrice _price;
        private readonly IReservation _reservation;
        private readonly IUser _user;
        private readonly IRent _rent;

        public HomeControllerTest()
        {
            _client = new ClientManager();
            _category = new CategoryManager();
            _car = new CarManager();
            _option = new OptionManager();
            _search = new SearchManager();
            _price = new RentPriceManager();
            _reservation = new ReservationManager();
            _user = new UserManager();
            _rent = new RentManager();
        }

        //[TestInitialize]
        //public void MyTestInitialize()
        //{
            //IKernel kernel = new StandardKernel();
            //helper = kernel.Get<Helper>();
            //crypt = kernel.Get<Crypt>();
            //category = kernel.Get<CategoryManager>();
            //car = kernel.Get<CarManager>();
            //option = kernel.Get<OptionManager>();
            //client = kernel.Get<ClientManager>();
            //search = kernel.Get<SearchManager>();
            //price = kernel.Get<RentPriceManager>();
            //reservation = kernel.Get<ReservationManager>();
            //user = kernel.Get<UserManager>();
            //rent = kernel.Get<RentManager>();
        //}

        [TestMethod]
        public void GenerateDataCategory()
        {
            var result = _category.Generate();
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void GenerateDataOption()
        {
            var result = _option.Generate();
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void GenerateDataClient()
        {
            var result = _client.Generate();
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void GenerateDataCar()
        {
            var result = _car.Generate();
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void GenerateDataPrice()
        {
            var result = _price.Generate();
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void GenerateDataReservation()
        {
            var result = _reservation.Generate();
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void GenerateDataUserAdmin()
        {
            var result = _user.Generate();
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void ListAllCategories()
        {
            var list = _category.ListCategories();
            Assert.IsTrue(list.Count > 0);
        }

        [TestMethod]
        public void ListCategoryById()
        {
            var obj = _category.ListCategory(1);
            Assert.IsTrue(obj.CategoryId == 1 && obj.Name.ToUpper() == "COMPACT");
        }

        [TestMethod]
        public void ListAllOptions()
        {
            var list = _option.ListOptions();
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
            var result = _client.CreateUpdate(model);
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
            var result = _client.CreateUpdate(model);
            var obj = _client.ListClient(5);
            Assert.IsTrue(result && obj.Email == "test2@email.com");
        }

        [TestMethod]
        public void SuccessfulAuthentificationClient()
        {
            var model = new Client();
            model.Email = "test2@email.com";
            model.Password = "password2";
            var result = _client.Authentification(model);
            Assert.IsTrue(result.GetType() == typeof(Client) && result.ClientId == 5);
        }

        [TestMethod]
        public void UnsuccessfulAuthentificationClient()
        {
            var model = new Client();
            model.Email = "noexist@email.com";
            model.Password = "noexist";
            var result = _client.Authentification(model);
            Assert.IsTrue(result == null);
        }

        [TestMethod]
        public void DeleteClientExist()
        {
            var result = _client.Remove(5);
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void DeleteClientDoesNotExist()
        {
            var result = _client.Remove(100);
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void ListAllAvailablesCategories()
        {
            var date1 = DateTime.Parse("2017-03-30");
            var date2 = DateTime.Parse("2017-03-30");
            var result = _search.SearchAvailableCategories(date1, date2, 0);
            Assert.IsTrue(result.Count == 3);
        }

        [TestMethod]
        public void ListAvailablesCategoriesWithoutCategory2()
        {
            var date1 = DateTime.Parse("2017-04-01");
            var date2 = DateTime.Parse("2017-04-01");
            var result = _search.SearchAvailableCategories(date1, date2, 0);
            Assert.IsFalse(result.Any(c => c.Category.CategoryId == 2));
        }

        [TestMethod]
        public void CheckIfCategoryIsAvailableTrue()
        {
            var date1 = DateTime.Parse("2017-04-01");
            var date2 = DateTime.Parse("2017-04-01");
            var result = _search.CheckAvailableCategory(date1, date2, 1);
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void CheckIfCategoryIsAvailableFalse()
        {
            var date1 = DateTime.Parse("2017-04-01");
            var date2 = DateTime.Parse("2017-04-01");
            var result = _search.CheckAvailableCategory(date1, date2, 2);
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void CreateReservation()
        {
            var listOption = new List<Option> {_option.ListOption(1)};
            var objReservation = new Reservation
            {
                ClientId = 1,
                CarId = 1,
                DateReservation = DateTime.Now.AddDays(3),
                DateStartRent = DateTime.Now.AddDays(3),
                DateReturnRent = DateTime.Now.AddDays(4),
                Cost = 80,
                Options = listOption,
                Status = 1
            };

            var result = _reservation.CreateUpdate(objReservation);
            Assert.IsTrue(result.ReservationId > 0);
        }

        [TestMethod]
        public void ListReservationWithoutRent()
        {
            var list = _reservation.ListReservationsWithNoRent();
            Assert.IsTrue(list.Count > 0);
        }

        [TestMethod]
        public void TransformReservationToRent()
        {
            var reserve = _reservation.ListReservation(1);
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
            objRent.Status = 1;
            var id = _rent.CreateUpdate(objRent);
            Assert.IsTrue(id > 0);
        }

        [TestMethod]
        public void CancelReservation()
        {
            var reserv = _reservation.ListReservation(1);
            reserv.Status = 0;
            var model = _reservation.CreateUpdate(reserv);
            Assert.IsTrue(model.Status == 0);
        }

        [TestMethod]
        public void CancelRent()
        {
            var list = _rent.ListRent();
            var obj = list[0];
            obj.Status = 0;
            var id = _rent.CreateUpdate(obj);
            var list2 = _rent.ListRent().Where(r => r.RentId == id).ToList();
            Assert.IsTrue(list2[0].Status == 0);
        }

        [TestMethod]
        public void ValidCancelDelayReservation()
        {
            var result = _reservation.CheckCancelDelay(5);
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void InvalidCancelDelayReservation()
        {
            var result = _reservation.CheckCancelDelay(4);
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void CreateUserAndCreateReservationAndRent()
        {
            //Create client
            var model = new Client();
            model.FirstName = "URentTest";
            model.Surname = "SurnameTest";
            model.Password = "password";
            model.Email = "test2@email.com";
            var result = _client.CreateUpdate(model);
            Assert.IsTrue(result);

            //Create reservation
            var listOption = new List<Option> {_option.ListOption(1)};
            var objReservation = new Reservation
            {
                ClientId = model.ClientId,
                CarId = 1,
                DateReservation = DateTime.Now.AddDays(3),
                DateStartRent = DateTime.Now.AddDays(3),
                DateReturnRent = DateTime.Now.AddDays(4),
                Cost = 80,
                Options = listOption
            };

            var result2 = _reservation.CreateUpdate(objReservation);
            Assert.IsTrue(result2.ReservationId > 0);

            //Create User (agent)
            var modelUser = new User();
            modelUser.FirstName = "AgentRent";
            modelUser.Surname = "AgentSurname";
            modelUser.Password = "password";
            modelUser.Email = "agent@email.com";
            var result3 = _user.CreateUpdate(modelUser);
            Assert.IsTrue(result3);

            //Create rent
            var reserve = _reservation.ListReservation(result2.ReservationId);
            var objRent = new Rent();
            objRent.CarId = reserve.CarId;
            objRent.ClientId = reserve.ClientId;
            objRent.Cost = reserve.Cost;
            objRent.DateDeparture = reserve.DateStartRent;
            objRent.DateReturn = reserve.DateReturnRent;
            objRent.Options = new List<Option>();
            objRent.Options = reserve.Options;
            objRent.UserId = 1;
            objRent.Status = 1;
            objRent.ReservationId = reserve.ReservationId;
            var id = _rent.CreateUpdate(objRent);
            Assert.IsTrue(id > 0);
        }

        [TestMethod]
        public void CreateReservationAndCancel()
        {
            //Create reservation
            var listOption = new List<Option> { _option.ListOption(1) };
            var objReservation = new Reservation
            {
                ClientId = 1,
                CarId = 1,
                DateReservation = DateTime.Now.AddDays(3),
                DateStartRent = DateTime.Now.AddDays(3),
                DateReturnRent = DateTime.Now.AddDays(4),
                Cost = 80,
                Options = listOption,
                Status = 1
            };
            var result = _reservation.CreateUpdate(objReservation);
            Assert.IsTrue(result.ReservationId > 0);

            //Cancel reservation
            var result2 = _reservation.Cancel(result.ReservationId);
            Assert.IsTrue(result2);
        }

        [TestMethod]
        public void CreateRentAndCancel()
        {
            //Create rent
            var reserve = _reservation.ListReservation(1);
            var objRent = new Rent();
            objRent.CarId = reserve.CarId;
            objRent.ClientId = reserve.ClientId;
            objRent.Cost = reserve.Cost;
            objRent.DateDeparture = reserve.DateStartRent;
            objRent.DateReturn = reserve.DateReturnRent;
            objRent.Options = new List<Option>();
            objRent.Options = reserve.Options;
            objRent.UserId = 1;
            objRent.Status = 1;
            objRent.ReservationId = reserve.ReservationId;
            var id = _rent.CreateUpdate(objRent);
            Assert.IsTrue(id > 0);

            //Cancel rent
            var result2 = _rent.Cancel(id);
            Assert.IsTrue(result2);
        }
    }
}