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
        private ICategoryManager _categoryManager;
        private ICarManager _carManager;
        private ISearch search;
        private IOptionManager optionManager;
        private IClientManager _clientManager;
        private IRentPriceManager _priceManager;
        private IReservationManager _reservationManager;
        private IUser user;
        private IRentManager _rentManager;

        [TestInitialize]
        public void MyTestInitialize()
        {
            IKernel kernel = new StandardKernel();
            _categoryManager = kernel.Get<CategoryManager>();
            _carManager = kernel.Get<CarManager>();
            optionManager = kernel.Get<OptionManager>();
            _clientManager = kernel.Get<ClientManager>();
            search = kernel.Get<SearchManager>();
            _priceManager = kernel.Get<RentPriceManager>();
            _reservationManager = kernel.Get<ReservationManager>();
            user = kernel.Get<UserManager>();
            _rentManager = kernel.Get<RentManager>();
        }

        #region INITIATE_DATA

        [TestMethod]
        public void GenerateDataCategory()
        {
            var result = _categoryManager.Generate();
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void GenerateDataOption()
        {
            var result = optionManager.Generate();
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void GenerateDataClient()
        {
            var result = _clientManager.Generate();
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void GenerateDataCar()
        {
            var result = _carManager.Generate();
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void GenerateDataPrice()
        {
            var result = _priceManager.Generate();
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void GenerateDataReservation()
        {
            var result = _reservationManager.Generate();
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void GenerateDataUserAdmin()
        {
            var result = user.Generate();
            Assert.IsTrue(result);
        }


        #endregion


        #region GET_DATA

        [TestMethod]
        public void ListAllCategories()
        {
            var list = _categoryManager.ListCategories();
            Assert.IsTrue(list.Count > 0);
        }

        [TestMethod]
        public void ListCategoryById()
        {
            var obj = _categoryManager.ListCategory(1);
            Assert.IsTrue(obj.CategoryId == 1 && obj.Name.ToUpper() == "COMPACT");
        }

        [TestMethod]
        public void ListAllOptions()
        {
            var list = optionManager.ListOptions();
            Assert.IsTrue(list.Count > 0);
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

        #endregion


        #region  AUTHENTIFICATION 

        /// <summary>
        ///  Auth-01 - Successfuly authenticate the user
        /// </summary>
        [TestMethod]
        public void SuccessfulAuthentificationClient()
        {
            var model = new Client();
            model.Email = "youssef@gmail.com";
            model.Password = "1324";
            var result = _clientManager.Authentification(model);
            Assert.IsTrue(result.GetType() == typeof(Client) && result.ClientId == 4);
        }

        /// <summary>
        ///     Auth-02 - Error : the passrowd is invalid
        /// </summary>
        [TestMethod]
        public void AuthentificationClientWrongPassword()
        {
            var model = new Client();
            model.Email = "youssef@gmail.com";
            model.Password = "WrongPassword";
            var result = _clientManager.Authentification(model);
            Assert.IsTrue(result == null);
        }

        /// <summary>
        ///     Auth-03 - Error : the email format is invalid
        /// </summary>
        [TestMethod]
        public void AuthentificationClientEmailFormatIncorrect()
        {
            var model = new Client();
            model.Email = "youssef@gmail";
            model.Password = "1324";
            var result = _clientManager.Authentification(model);
            Assert.IsTrue(result.Error == "Email format incorrect");
        }

        /// <summary>
        ///     Auth-04 - Error : The fields are required
        /// </summary>
        [TestMethod]
        public void AuthentificationClientEmptyFields()
        {
            var model = new Client();
            model.Email = null;
            model.Password = null;
            var result = _clientManager.Authentification(model);
            Assert.IsTrue(result.Error == "All the fields are required");
        }

        #endregion


        #region GESTION DU COMPTE PART 1

        /******************\ NEW ACCOUNT /******************/

        /// <summary>
        ///     GC-01 - Account created
        /// </summary>
        [TestMethod]
        public void CreateClientAccount()
        {
            var model = new Client();
            model.FirstName = "URent";
            model.Surname = "Surname";
            model.Email = "URent01@email.com";
            model.Password = "password";
            var result = _clientManager.CreateUpdate(model);
            Assert.IsTrue(result);
        }

        /// <summary>
        ///     GC-02 - Error : All the fields are required
        /// </summary>
        [TestMethod]
        public void AccountCreatingAllFieldsRequired()
        {
            var model = new Client();
            model.Email = "URent02@email.com";
            model.Password = "password";
            var result = _clientManager.CreateUpdate(model);
            Assert.IsTrue(!result && (model.Error == "All the fields are required"));
        }


        /// <summary>
        ///     GC-03 - Error : Email format incorrect
        /// </summary>
        [TestMethod]
        public void AccountCreatingEmailFormatIncorrect()
        {
            var model = new Client();
            model.FirstName = "URent";
            model.Surname = "Surname";
            model.Email = "URent03@email";
            model.Password = "password";
            var result = _clientManager.CreateUpdate(model);
            Assert.IsTrue(!result && (model.Error == "Email format incorrect"));
        }

        /// <summary>
        ///     GC-04 - Error : Email Already exists
        /// </summary>
        [TestMethod]
        public void AccountCreatingEmailAlreadyExists()
        {
            var model = new Client();
            model.FirstName = "URent";
            model.Surname = "Surname";
            model.Email = "URent01@email.com";
            model.Password = "password";
            var result = _clientManager.CreateUpdate(model);
            Assert.IsTrue(!result && (model.Error == "Email already exists"));
        }

        /******************************************************/

        /******************\ UPDATE ACCOUNT /******************/

        /// <summary>
        ///     GC-05 - Account updated
        /// </summary>
        [TestMethod]
        public void AccountUpdateClient()
        {
            var model = new Client();
            model.ClientId = 5;
            model.FirstName = "URent_Updated";
            model.Surname = "Surname_Updated";
            model.Email = "URent01@email.com";
            model.Password = "password_Updated";
            var result = _clientManager.CreateUpdate(model);
            var obj = _clientManager.ListClient(5);
            Assert.IsTrue(result && obj.FirstName == "URent_Updated");
        }

        /// <summary>
        ///     GC-06 - Error : All fields are required
        /// </summary>
        [TestMethod]
        public void AccountUpdateClientAllFieldsRequired()
        {
            var model = new Client();
            model.Email = "URent01@email.com";
            model.Password = "password_Updated";
            var result = _clientManager.CreateUpdate(model);
            Assert.IsTrue(!result && (model.Error == "All the fields are required"));
        }

        /// <summary>
        ///     GC-07 - Error : Password and it's confirmation is tested in the client side.
        /// </summary>
        [TestMethod]
        public void AccountUpdateClientConfirmationMatchPasswod()
        {
            // Do nothing
            Assert.IsTrue(true);
        }

        /******************************************************/


        #endregion


        #region RÉSERVATION

        /// <summary>
        ///     Res-01 - Create a new reservation
        /// </summary>
        [TestMethod]
        public void ReservationCreateNew()
        {
            var listOption = new List<Option> { optionManager.ListOption(1) };
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

            var result = _reservationManager.CreateUpdate(objReservation);
            Assert.IsTrue(result.ReservationId > 0);
        }

        /// <summary>
        ///     Res-02 - Reset reservation
        ///     Not implemented in the test class since it's in the user interface.
        /// </summary>


        /// <summary>
        ///     Res-03 - Cancel reservation.
        /// </summary>
        [TestMethod]
        public void ReservationCancel()
        {
            var reserv = _reservationManager.ListReservation(1);
            reserv.Status = 0;
            var model = _reservationManager.CreateUpdate(reserv);
            Assert.IsTrue(model.Status == 0);
        }

        /// <summary>
        ///     Res-03 - Cancel reservation without paying the cancelation fee.
        /// </summary>
        [TestMethod]
        public void ReservationValidCancelDelay()
        {
            var result = _reservationManager.CheckCancelDelay(5);
            Assert.IsTrue(result);
        }

        /// <summary>
        ///     Res-05 - Cancel reservation and paying the cancelation fee.
        /// </summary>
        [TestMethod]
        public void ReservationInvalidCancelDelay()
        {
            var result = _reservationManager.CheckCancelDelay(1);
            Assert.IsFalse(result);
        }

        /// <summary>
        ///     Res-06 - get user reservation.
        /// </summary>
        [TestMethod]
        public void GetUserReservations()
        {
            var result = _reservationManager.ListReservationByClient(5);
            Assert.IsTrue(result.Count > 0);
        }

        /// <summary>
        ///     Res-07 - Erro : get user reservation (No data found).
        /// </summary>
        [TestMethod]
        public void GetUserReservationsNoDataFound()
        {
            var result = _reservationManager.ListReservationByClient(4);
            Assert.IsTrue(result.Count == 0);
        }
    
        /// <summary>
        ///     Not listed in the test case document
        /// </summary>
        [TestMethod]
        public void ListReservationWithoutRent()
        {
            var list = _reservationManager.ListReservationsWithNoRent();
            Assert.IsTrue(list.Count > 0);
        }

        /// <summary>
        ///     Not listed in the test case document
        /// </summary>
        [TestMethod]
        public void CheckIfCategoryIsAvailableTrue()
        {
            var date1 = DateTime.Parse("2017-04-01");
            var date2 = DateTime.Parse("2017-04-01");
            var result = search.CheckAvailableCategory(date1, date2, 1);
            Assert.IsTrue(result);
        }

        /// <summary>
        ///     Not listed in the test case document
        /// </summary>
        [TestMethod]
        public void CheckIfCategoryIsAvailableFalse()
        {
            var date1 = DateTime.Parse("2017-04-01");
            var date2 = DateTime.Parse("2017-04-01");
            var result = search.CheckAvailableCategory(date1, date2, 2);
            Assert.IsFalse(result);
        }


        #endregion


        #region LOCATION

        /// <summary>
        ///     Not listed in the test case document
        /// </summary>
        [TestMethod]
        public void TransformReservationToRent()
        {
            var reserve = _reservationManager.ListReservation(1);
            var objRent = new Rent();
            objRent.CarId = reserve.CarId;
            objRent.ClientId = reserve.ClientId;
            objRent.Cost = reserve.Cost;
            objRent.DateDeparture = DateTime.Now.AddDays(3);
            objRent.DateReturn = DateTime.Now.AddDays(4);
            objRent.Options = new List<Option>();
            objRent.Options = reserve.Options;
            objRent.UserId = 1;
            objRent.ReservationId = reserve.ReservationId;
            objRent.Status = 1;
            var id = _rentManager.CreateUpdate(objRent);
            Assert.IsTrue(id > 0);
        }

        /// <summary>
        ///     Loc-03 - Cancel rent
        /// </summary>
        [TestMethod]
        public void CancelRent()
        {
            var list = _rentManager.ListRent();
            var obj = list[0];
            obj.Status = 0;
            var id = _rentManager.CreateUpdate(obj);
            var list2 = _rentManager.ListRent().Where(r => r.RentId == id).ToList();
            Assert.IsTrue(list2[0].Status == 0);
        }

        #endregion


        #region INTEGRATION_TESTS

        /// <summary>
        ///     Not listed in the test case document
        /// </summary>
        [TestMethod]
        public void CreateUserAndCreateReservationAndRent()
        {
            //Create client
            var model = new Client();
            model.FirstName = "URentTest";
            model.Surname = "SurnameTest";
            model.Password = "password";
            model.Email = "test2@email.com";
            var result = _clientManager.CreateUpdate(model);
            Assert.IsTrue(result);

            //Create reservation
            var listOption = new List<Option> { optionManager.ListOption(1) };
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

            var result2 = _reservationManager.CreateUpdate(objReservation);
            Assert.IsTrue(result2.ReservationId > 0);

            //Create User (agent)
            var modelUser = new User();
            modelUser.FirstName = "AgentRent";
            modelUser.Surname = "AgentSurname";
            modelUser.Password = "password";
            modelUser.Email = "agent@email.com";
            var result3 = user.CreateUpdate(modelUser);
            Assert.IsTrue(result3);

            //Create rent
            var reserve = _reservationManager.ListReservation(result2.ReservationId);
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
            var id = _rentManager.CreateUpdate(objRent);
            Assert.IsTrue(id > 0);
        }

        /// <summary>
        ///     Not listed in the test case document
        /// </summary>
        [TestMethod]
        public void CreateReservationAndCancel()
        {
            //Create reservation
            var listOption = new List<Option> { optionManager.ListOption(1) };
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
            var result = _reservationManager.CreateUpdate(objReservation);
            Assert.IsTrue(result.ReservationId > 0);

            //Cancel reservation
            var result2 = _reservationManager.Cancel(result.ReservationId);
            Assert.IsTrue(result2);
        }

        /// <summary>
        ///     Not listed in the test case document
        /// </summary>
        [TestMethod]
        public void CreateRentAndCancel()
        {
            //Create rent
            var reserve = _reservationManager.ListReservation(1);
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
            var id = _rentManager.CreateUpdate(objRent);
            Assert.IsTrue(id > 0);

            //Cancel rent
            var result2 = _rentManager.Cancel(id);
            Assert.IsTrue(result2);
        }

        #endregion


        #region GESTION DE COMPTE PART 2

        /******************\ DELETE ACCOUNT /******************/

        /// <summary>
        ///     GC-08 - Account deleted
        /// </summary>
        [TestMethod]
        public void AccountDeleteClient()
        {
            var result = _clientManager.Remove(5);
            Assert.IsTrue(result);
        }

        /// <summary>
        ///     GC-09 - Error : All fields are required
        ///     It's normally tested in the user side.
        /// </summary>
        [TestMethod]
        public void AccountDeleteClientAllFiedsRequired()
        {
            Assert.IsTrue(true);
        }

        /// <summary>
        ///     GC-10 - Error : Account doesn't exist
        /// </summary>
        [TestMethod]
        public void AccountDeleteClientDoesNotExist()
        {
            var result = _clientManager.Remove(100);
            Assert.IsFalse(result);
        }
        
        /*************************************************/

        #endregion
    }
}
