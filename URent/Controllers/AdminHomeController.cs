using Ninject;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Web.Mvc;
using URent.Models;
using URent.Models.Interfaces;
using URent.Models.Model;

namespace URent.Controllers
{
    public class AdminHomeController : Controller
    {
        private readonly IUser _user;
        private readonly IReservation _reservation;
        private readonly IClient _client;
        private readonly ICategory _category;
        private readonly ICar _car;
        private readonly IOption _option;
        private readonly IRent _rent;
        private readonly ISearch _search;

        public AdminHomeController([Named("Prod")] IUser user, IReservation reservation, IClient client, ICategory category, ICar car, IOption option, IRent rent, ISearch search)
        {
            if (user == null) throw new ArgumentNullException(nameof(user));
            _user = user;
            _reservation = reservation;
            _client = client;
            _category = category;
            _car = car;
            _option = option;
            _search = search;
            _rent = rent;
        }

        // GET: AdminHome
        public ActionResult Index()
        {
            if (!_user.IsAuthenticated())
            {
                return RedirectToAction("Login", "AdminAccount");
            }
            return View();
        }

        public ActionResult Reservation()
        {
            if (_user.IsAuthenticated())
            {
                var model = new List<ReservationViewModel>();
                var list = _reservation.ListReservationsWithNoRent();
                if (list != null && list.Count > 0)
                {
                    foreach (var r in list)
                    {
                        var obj = new ReservationViewModel();
                        var resultCar = _car.ListCar(r.CarId);
                        var resultClient = _client.ListClient(r.ClientId);
                        obj.ReservationId = r.ReservationId;
                        obj.ClientName = $"{resultClient.FirstName} {resultClient.Surname}";
                        obj.Car = resultCar.Name;
                        obj.Category = _category.ListCategory(resultCar.CategoryId).Name;
                        obj.Cost = r.Cost;
                        obj.DateReservation = r.DateReservation;
                        obj.DateStartRent = r.DateStartRent;
                        obj.DateReturnRent = r.DateReturnRent;
                        model.Add(obj);
                    }
                }
                return View(model);
            }
            else
            {
                return RedirectToAction("Login", "AdminAccount");
            }
        }

        public ActionResult ListRent()
        {
            if (_user.IsAuthenticated())
            {
                var model = new List<RentViewModel>();
                var list = _rent.ListRent();
                if (list != null && list.Count > 0)
                {
                    foreach (var r in list)
                    {
                        var obj = new RentViewModel();
                        var resultCar = _car.ListCar(r.CarId);
                        var resultClient = _client.ListClient(r.ClientId);
                        obj.RentId = r.RentId;
                        obj.ReservationId = r.ReservationId;
                        obj.ClientName = $"{resultClient.FirstName} {resultClient.Surname}";
                        obj.Car = resultCar.Name;
                        obj.Category = _category.ListCategory(resultCar.CategoryId).Name;
                        obj.Cost = r.Cost;
                        obj.DateDeparture = r.DateDeparture;
                        obj.DateReturn = r.DateReturn;
                        obj.Status = r.Status;
                        model.Add(obj);
                    }
                }
                return View(model);
            }
            else
            {
                return RedirectToAction("Login", "AdminAccount");
            }
        }

        public ActionResult Rent()
        {
            var model = new SearchViewModel();

            string reservationId = Request.QueryString["ReservationId"];

            var list = _reservation.ListReservation(int.Parse(reservationId));
            var listCategories = _category.ListCategories();
            var listOptions = _option.ListOptions();
            var listTime = ListTime();

            model.ListCategory = listCategories;
            model.ListOption = listOptions;
            model.DateDeparture = DateTime.Parse((DateTime.Parse(list.DateStartRent.ToString(CultureInfo.InvariantCulture)).ToShortDateString()));
            model.DateReturn = DateTime.Parse((DateTime.Parse(list.DateReturnRent.ToString(CultureInfo.InvariantCulture)).ToShortDateString()));
            model.ListTimeDeparture = listTime;
            model.ListTimeReturn = listTime;
            model.CategoryId = _car.ListCar(list.CarId).CategoryId;
            model.TimeDeparture = $"{FormatTime(list.DateStartRent.Hour)}:{FormatTime(list.DateStartRent.Minute)}";
            model.TimeReturn = $"{FormatTime(list.DateReturnRent.Hour)}:{FormatTime(list.DateReturnRent.Minute)}";
            model.ReservationId = int.Parse(reservationId);
            model.ListClient = _client.ListClients();
            model.ClientId = list.ClientId;
            int[] selectedOptions = new int[list.Options.Count];
            var i = 0;
            foreach (var o in list.Options)
            {
                selectedOptions[i] = o.OptionId;
                i++;
            }
            model.SelectedOptions = selectedOptions;
            return View(model);
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Rent(SearchViewModel model)
        {
            if (_user.IsAuthenticated())
            {
                var rentModel = new Rent();
                var dateDeparture = FormatDateTime(DateTime.Parse(model.DateDeparture.ToString()), model.TimeDeparture);
                var dateReturn = FormatDateTime(DateTime.Parse(model.DateReturn.ToString()), model.TimeReturn);
                //Check if category is available
                if (_search.CheckAvailableCategory(dateDeparture, dateReturn, model.CategoryId, model.ReservationId))
                {
                    rentModel.CarId = _search.GetCarAvailable(dateDeparture, dateReturn, model.CategoryId, model.ReservationId);
                    rentModel.ClientId = model.ClientId;
                    rentModel.DateDeparture = dateDeparture;
                    rentModel.DateReturn = dateReturn;
                    rentModel.Options = new List<Option>();
                    foreach (var o in model.SelectedOptions)
                    {
                        rentModel.Options.Add(_option.ListOption(o));
                    }
                    rentModel.ReservationId = model.ReservationId;
                    rentModel.UserId = int.Parse(Session["UserId"].ToString());
                    rentModel.Status = 1;
                    var id = _rent.CreateUpdate(rentModel);
                    if (id > 0)
                    {
                        rentModel.RentId = id;
                        return View("Confirmation", rentModel);
                    }
                    else
                    {
                        AddErrors("Error in creating the rent!");
                    }
                }
                else
                {
                    AddErrors("Category is not more available!");
                }
                var listCategories = _category.ListCategories();
                var listOptions = _option.ListOptions();
                var listTime = ListTime();
                model.ListCategory = listCategories;
                model.ListOption = listOptions;
                model.ListTimeDeparture = listTime;
                model.ListTimeReturn = listTime;
                model.ListClient = _client.ListClients();
                return View(model);
            }
            else
            {
                return RedirectToAction("Login", "AdminAccount");
            }
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult Cancel(int id)
        {
            var result = _rent.Cancel(id);
            if (!result)
            {
                AddErrors("Error in cancelation!");
            }
            return RedirectToAction("ListRent", "AdminHome");
        }

        public ActionResult Confirmation()
        {
            return View();
        }

        private void AddErrors(string error)
        {
            ModelState.AddModelError("", error);
        }

        private static IList<string> ListTime()
        {
            var hour = 6;
            var list = new List<string>();
            for (var i = 0; i <= 10; i++)
            {
                hour += 1;
                var hourString = FormatTime(hour);
                for (var a = 0; a <= 1; a++)
                {
                    var minute = a % 2 == 0 ? "00" : "30";
                    list.Add($"{hourString}:{minute}");
                }
            }
            return list;
        }

        private static string FormatTime(int time)
        {
            if (time.ToString().Length == 1)
            {
                return "0" + time.ToString();
            }
            return time.ToString();
        }

        private static DateTime FormatDateTime(DateTime date, string time)
        {
            var formatDate = $"{date.ToShortDateString()} {time}:00";
            return DateTime.Parse(formatDate);
        }

    }
}