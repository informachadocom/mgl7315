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
        private readonly ICategory _category;
        private readonly ISearch _search;
        private readonly IOption _option;
        private readonly IClient _client;
        private readonly IRentPrice _price;
        private readonly IReservation _reservation;
        public HomeController([Named("Prod")] ICategory category, ISearch search, IOption option, IClient client, IRentPrice price, IReservation reservation)
        {
            _category = category;
            _search = search;
            _option = option;
            _client = client;
            _price = price;
            _reservation = reservation;
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
            Session["SearchViewModel"] = model;
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
                var listCategories = _search.GetCategoryAvailable(DateTime.Parse(model.DateDeparture.ToString()), DateTime.Parse(model.DateReturn.ToString()), model.CategoryId);
                if (listCategories.Count > 0)
                {
                    //Get options
                    var listOptions = _option.ListOptions();
                    model.ListCategory = listCategories;
                    model.ListOption = listOptions;
                    model.DateDeparture = DateTime.Parse((DateTime.Parse(model.DateDeparture.ToString()).ToShortDateString()));
                    model.DateReturn = DateTime.Parse((DateTime.Parse(model.DateReturn.ToString()).ToShortDateString()));
                    //return RedirectToAction("Result", model);
                    return View("Result", model);
                }
                else
                {
                    //No vehicules available
                    AddErrors("No vehicules are available for this date.");
                }
            }
            model = (SearchViewModel)Session["SearchViewModel"];
            return View(model);
        }

        //
        // GET: /Home/Result
        public ActionResult Result(SearchViewModel model)
        {
            if (model == null || model.DateDeparture == null)
            {
                return RedirectToAction("Index", "Home");
            }
            return View(model);
        }

        //POST: /Home/Result
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ResultPost(SearchViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (model.CategoryId > 0)
                {
                    model.DateDeparture = DateTime.Parse(
                        DateTime.Parse(model.DateDeparture.ToString()).ToShortDateString() + " " + model.TimeDeparture +
                        ":00");
                    model.DateReturn = DateTime.Parse(DateTime.Parse(model.DateReturn.ToString()).ToShortDateString() +
                                                      " " + model.TimeReturn + ":00");
                    var order = (List<Order>)_search.SearchAvailableCategories(DateTime.Parse(model.DateDeparture.ToString()),
                        DateTime.Parse(model.DateReturn.ToString()), model.CategoryId);
                    if (model.SelectedOptions != null && model.SelectedOptions.Length > 0)
                    {
                        var listSelectedOptions = ListSelectedOption(model.SelectedOptions);
                        order[0].Options = listSelectedOptions;
                        order = _price.RecalculatePriceSearch(order) as List<Order>;
                    }
                    if (order != null) Session["order"] = order[0];
                    return RedirectToAction("Summary");
                }
                else
                {
                    AddErrors("No vehicules are availables.");
                }
            }

            // Something failed, redisplay form
            return RedirectToAction("Index", "Home", model);
        }

        //
        // GET: /Home/Summary
        public ActionResult Summary()
        {
            if (Session["order"] != null)
            {
                if (!_client.IsAuthenticated())
                {
                    TempData["Message"] = "To continue you have to login or register a new client.";
                    TempData["Redirect"] = "Summary";
                    return RedirectToAction("Login", "Account");
                }
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
            return View((Order)Session["order"]);
        }

        //
        // GET: /Home/Reservation
        public ActionResult Reservation()
        {
            if (Session["Order"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        //POST: /Home/Result
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Reservation(Order model)
        {
            if (model == null) throw new ArgumentNullException(nameof(model));
            if (_client.IsAuthenticated())
            {
                model = (Order)Session["Order"];
                if (ModelState.IsValid)
                {
                    //Check if the category still available
                    var result = _search.CheckAvailableCategory(model.DateDeparture, model.DateReturn, model.Category.CategoryId);
                    if (result)
                    {
                        var reserv = new Reservation();
                        reserv.CarId = _search.GetCarAvailable(model.DateDeparture, model.DateReturn, model.Category.CategoryId);
                        reserv.ClientId = (int)Session["ClientId"];
                        reserv.Cost = model.Total;
                        reserv.DateReservation = DateTime.Today;
                        reserv.DateStartRent = model.DateDeparture;
                        reserv.DateReturnRent = model.DateReturn;
                        reserv.Options = model.Options;
                        reserv.Status = 1;
                        var resultReservation = _reservation.CreateUpdate(reserv);
                        if (resultReservation != null && resultReservation.ReservationId > 0)
                        {
                            ViewBag.Reservation = true;
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
            if (ViewBag.Reservation != null && ViewBag.Reservation == true)
            {
                if (_client.IsAuthenticated())
                {
                    Session["order"] = null;
                    return View(model);
                }
                else
                {
                    return View("Index");
                }
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        public ActionResult ListReservation()
        {
            if (_client.IsAuthenticated())
            {
                var model = _reservation.ListReservationByClient(ClientId);
                return View(model);
            }
            return RedirectToAction("Login", "Account");
        }


        [HttpPost]
        [AllowAnonymous]
        public ActionResult Cancel(int id)
        {
            //var delay = reservation.CheckCancelDelay(id);
            //if (delay)
            //{
            //On peut canceler la reservation sans frais d'annulation
            var result = _reservation.Cancel(id);
            if (!result)
            {
                AddErrors("Error in cancelation!");
            }
            return RedirectToAction("ListReservation", "Home");
            //}
            //else
            //{
            //Il y a un frais d'annulation à payer
            //return RedirectToAction("ListReservation", "Home");
            //}
        }

        private IList<Category> ListCategory()
        {
            return _category.ListCategories();
        }

        private IList<Option> ListSelectedOption(int[] options)
        {
            var list = new List<Option>();
            foreach (var i in options)
            {
                var op = _option.ListOption(i);
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
            var list = new List<string>();
            for (var i = 0; i <= 10; i++)
            {
                hour += 1;
                var hourString = FormatTime(hour);
                for (var a = 0; a <= 1; a++)
                {
                    string minute;
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

        private int ClientId
        {
            get
            {
                if (Session["ClientId"] != null)
                {
                    return int.Parse(Session["ClientId"].ToString());
                }
                return 0;
            }
        }

    }
}