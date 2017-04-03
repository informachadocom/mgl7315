using Ninject;
using System;
using System.Collections.Generic;
using System.Web.Mvc;
using URent.Models;
using URent.Models.Interfaces;
using URent.Models.Model;

namespace URent.Controllers
{
    public class HomeController : Controller
    {
        private readonly ICategory category;
        private readonly ISearch search;
        private readonly IOption option;
        private readonly IClient client;
        private readonly IRentPrice price;
        private readonly IReservation reservation;
        public HomeController([Named("Prod")] ICategory _category, ISearch _search, IOption _option, IClient _client, IRentPrice _price, IReservation _reservation)
        {
            category = _category;
            search = _search;
            option = _option;
            client = _client;
            price = _price;
            reservation = _reservation;
        }

        public ActionResult Index()
        {
            var listCategory = ListCategory();
            listCategory.Insert(0, new Category { CategoryId = 0, Name = "All categories" });
            var model = new SearchViewModel();
            model.ListCategory = listCategory;
            //Get time
            var listTime = ListTime();
            model.ListTimeDeparture = listTime;
            model.ListTimeReturn = listTime;
            return View(model);
        }

        //POST: /Home/Index
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Index(SearchViewModel model)
        {
            if (ModelState.IsValid)
            {
                //Get available categories for these dates
                var listCategories = search.GetCategoryAvailable(DateTime.Parse(model.DateDeparture.ToString()), DateTime.Parse(model.DateReturn.ToString()), model.CategoryId);
                //Get options
                var listOptions = option.ListOptions();
                model.ListCategory = listCategories;
                model.ListOption = listOptions;
                model.DateDeparture = DateTime.Parse((DateTime.Parse(model.DateDeparture.ToString()).ToShortDateString()));
                model.DateReturn = DateTime.Parse((DateTime.Parse(model.DateReturn.ToString()).ToShortDateString()));
                return View("Result", model);
            }
            return View();
        }

        //
        // GET: /Home/Result
        public ActionResult Result()
        {
            return View();
        }

        //POST: /Home/Result
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Result(SearchViewModel model)
        {
            if (ModelState.IsValid)
            {
                model.DateDeparture = DateTime.Parse(DateTime.Parse(model.DateDeparture.ToString()).ToShortDateString() + " " + model.TimeDeparture + ":00");
                model.DateReturn = DateTime.Parse(DateTime.Parse(model.DateReturn.ToString()).ToShortDateString() + " " + model.TimeReturn + ":00");
                var order = new List<Order>();
                order = (List<Order>)search.SearchAvailableCategories(DateTime.Parse(model.DateDeparture.ToString()), DateTime.Parse(model.DateReturn.ToString()), model.CategoryId);
                if (model.SelectedOptions.Length > 0)
                {
                    var listSelectedOptions = ListSelectedOption(model.SelectedOptions);
                    order[0].Options = listSelectedOptions;
                    order = price.RecalculatePriceSearch(order) as List<Order>;
                }
                Session["order"] = order[0];
                return RedirectToAction("Summary");
            }

            // Something failed, redisplay form
            return View(model);
        }

        //
        // GET: /Home/Summary
        public ActionResult Summary()
        {
            if (!client.isAuthenticated())
            {
                TempData["Message"] = "To continue you have to login or register a new client.";
                TempData["Redirect"] = "Summary";
                return RedirectToAction("Login", "Account");
            }
            return View();
        }

        //
        // GET: /Home/Reservation
        public ActionResult Reservation()
        {
            return View();
        }

        //POST: /Home/Result
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Reservation(Order model)
        {
            if (client.isAuthenticated())
            {
                model = (Order)Session["Order"];
                if (ModelState.IsValid)
                {
                    //Check if the category still available
                    var result = search.CheckAvailableCategory(model.DateDeparture, model.DateReturn, model.Category.CategoryId);
                    if (result)
                    {
                        var reserv = new Reservation();
                        reserv.CarId = search.GetCarAvailable(model.DateDeparture, model.DateReturn, model.Category.CategoryId);
                        reserv.ClientId = (int)Session["ClientId"];
                        reserv.Cost = model.Total;
                        reserv.DateReservation = DateTime.Today;
                        reserv.DateStartRent = model.DateDeparture;
                        reserv.DateReturnRent = model.DateReturn;
                        reserv.Options = model.Options;
                        var resultReservation = reservation.CreateUpdate(reserv);
                        if (resultReservation != null && resultReservation.ReservationId > 0)
                        {
                            return View("Confirmation", reserv);
                        }
                        else
                        {
                            AddErrors("Error on the reservation. Please try again.");
                            return View();
                        }
                    }
                    else
                    {
                        AddErrors("The reservation was canceled because the category is not more available. Please try again.");
                    }
                }
            }
            else
            {
                TempData["Message"] = "Your session was over. Please login.";
                TempData["Redirect"] = "Summary";
                return RedirectToAction("Login", "Account");
            }

            // Something failed, redisplay form
            return View(model);
        }

        public ActionResult Confirmation(Reservation model)
        {
            if (client.isAuthenticated())
            {
                Session["order"] = null;
                return View(model);
            }
            else
            {
                return View("Index");
            }
        }

        private IList<Category> ListCategory()
        {
            return category.ListCategories();
        }

        private IList<Option> ListSelectedOption(int[] options)
        {
            var list = new List<Option>();
            foreach (var i in options)
            {
                var op = option.ListOption(i);
                list.Add(op);
            }
            return list;
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
            for(var i = 0; i <= 10; i++)
            {
                hour += 1;
                hourString = FormatTime(hour);
                for (var a = 0; a <= 1; a++)
                {
                    if (a%2 == 0)
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
    }
}