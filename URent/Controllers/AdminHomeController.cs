using Ninject;
using System;
using System.Collections.Generic;
using System.Web.Mvc;
using URent.Models;
using URent.Models.Interfaces;
using URent.Models.Model;

namespace URent.Controllers
{
    public class AdminHomeController : Controller
    {
        private readonly IUser user;
        private readonly IReservation reservation;
        private readonly IClient client;
        private readonly ICategory category;
        private readonly ICar car;
        private readonly IOption option;
        private readonly IRent rent;
        private readonly ISearch search;

        public AdminHomeController([Named("Prod")] IUser _user, IReservation _reservation, IClient _client, ICategory _category, ICar _car, IOption _option, IRent _rent, ISearch _search)
        {
            if (_user == null) throw new ArgumentNullException(nameof(_user));
            user = _user;
            reservation = _reservation;
            client = _client;
            category = _category;
            car = _car;
            option = _option;
            search = _search;
            rent = _rent;
        }

        // GET: AdminHome
        public ActionResult Index()
        {
            if (!user.isAuthenticated())
            {
                return RedirectToAction("Login", "AdminAccount");
            }
            return View();
        }

        public ActionResult Reservation()
        {
            if (user.isAuthenticated())
            {
                var model = new List<ReservationViewModel>();
                var list = reservation.ListReservationsWithNoRent();
                if (list != null && list.Count > 0)
                {
                    foreach (var r in list)
                    {
                        var obj = new ReservationViewModel();
                        var resultCar = car.ListCar(r.CarId);
                        var resultClient = client.ListClient(r.ClientId);
                        obj.ReservationId = r.ReservationId;
                        obj.ClientName = $"{resultClient.FirstName} {resultClient.Surname}";
                        obj.Car = resultCar.Name;
                        obj.Category = category.ListCategory(resultCar.CategoryId).Name;
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
            if (user.isAuthenticated())
            {
                var model = new List<RentViewModel>();
                var list = rent.ListRent();
                if (list != null && list.Count > 0)
                {
                    foreach (var r in list)
                    {
                        var obj = new RentViewModel();
                        var resultCar = car.ListCar(r.CarId);
                        var resultClient = client.ListClient(r.ClientId);
                        obj.RentId = r.RentId;
                        obj.ReservationId = r.ReservationId;
                        obj.ClientName = $"{resultClient.FirstName} {resultClient.Surname}";
                        obj.Car = resultCar.Name;
                        obj.Category = category.ListCategory(resultCar.CategoryId).Name;
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

            var list = reservation.ListReservation(int.Parse(reservationId));
            var listCategories = category.ListCategories();
            var listOptions = option.ListOptions();
            var listTime = ListTime();

            model.ListCategory = listCategories;
            model.ListOption = listOptions;
            model.DateDeparture = DateTime.Parse((DateTime.Parse(list.DateStartRent.ToString()).ToShortDateString()));
            model.DateReturn = DateTime.Parse((DateTime.Parse(list.DateReturnRent.ToString()).ToShortDateString()));
            model.ListTimeDeparture = listTime;
            model.ListTimeReturn = listTime;
            model.CategoryId = car.ListCar(list.CarId).CategoryId;
            model.TimeDeparture = string.Format("{0}:{1}", FormatTime(list.DateStartRent.Hour), FormatTime(list.DateStartRent.Minute));
            model.TimeReturn = string.Format("{0}:{1}", FormatTime(list.DateReturnRent.Hour), FormatTime(list.DateReturnRent.Minute));
            model.ReservationId = int.Parse(reservationId);
            model.ListClient = client.ListClients();
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
            if (user.isAuthenticated())
            {
                var rentModel = new Rent();
                var dateDeparture = FormatDateTime(DateTime.Parse(model.DateDeparture.ToString()), model.TimeDeparture);
                var dateReturn = FormatDateTime(DateTime.Parse(model.DateReturn.ToString()), model.TimeReturn);
                //Check if category is available
                if (search.CheckAvailableCategory(dateDeparture, dateReturn, model.CategoryId, model.ReservationId))
                {
                    rentModel.CarId = search.GetCarAvailable(dateDeparture, dateReturn, model.CategoryId, model.ReservationId);
                    rentModel.ClientId = model.ClientId;
                    rentModel.DateDeparture = dateDeparture;
                    rentModel.DateReturn = dateReturn;
                    rentModel.Options = new List<Option>();
                    foreach (var o in model.SelectedOptions)
                    {
                        rentModel.Options.Add(option.ListOption(o));
                    }
                    rentModel.ReservationId = model.ReservationId;
                    rentModel.UserId = int.Parse(Session["UserId"].ToString());
                    rentModel.Status = 1;
                    var id = rent.CreateUpdate(rentModel);
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
                var listCategories = category.ListCategories();
                var listOptions = option.ListOptions();
                var listTime = ListTime();
                model.ListCategory = listCategories;
                model.ListOption = listOptions;
                model.ListTimeDeparture = listTime;
                model.ListTimeReturn = listTime;
                model.ListClient = client.ListClients();
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
            var result = rent.Cancel(id);
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

        private IList<string> ListTime()
        {
            var hour = 6;
            var hourString = "";
            var minute = "";
            var list = new List<string>();
            for (var i = 0; i <= 10; i++)
            {
                hour += 1;
                hourString = FormatTime(hour);
                for (var a = 0; a <= 1; a++)
                {
                    if (a % 2 == 0)
                    {
                        minute = "00";
                    }
                    else
                    {
                        minute = "30";
                    }
                    list.Add(string.Format("{0}:{1}", hourString, minute));
                }
            }
            return list;
        }

        private string FormatTime(int time)
        {
            if (time.ToString().Length == 1)
            {
                return "0" + time.ToString();
            }
            return time.ToString();
        }

        private DateTime FormatDateTime(DateTime date, string time)
        {
            var formatDate = string.Format("{0} {1}:00", date.ToShortDateString(), time);
            return DateTime.Parse(formatDate);
        }

    }
}