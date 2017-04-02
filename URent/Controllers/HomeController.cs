using Ninject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
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
        public HomeController([Named("Prod")] ICategory _category, ISearch _search, IOption _option, IClient _client)
        {
            category = _category;
            search = _search;
            option = _option;
            client = _client;
        }

        public ActionResult Index()
        {
            var listCategory = ListCategory();
            listCategory.Insert(0, new Category { CategoryId = 0, Name = "All categories" });
            var model = new SearchViewModel();
            model.ListCategory = listCategory;
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
                var order = new List<Order>();
                order = (List<Order>)search.SearchAvailableCategories(DateTime.Parse(model.DateDeparture.ToString()), DateTime.Parse(model.DateReturn.ToString()), model.CategoryId);
                if (model.SelectedOptions.Length > 0)
                {
                    var listSelectedOptions = ListSelectedOption(model.SelectedOptions);
                    order[0].Options = listSelectedOptions;
                }
                Session["order"] = order;
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
                TempData["Redirect"] = "/Home/Summary";
                return RedirectToAction("Login", "Account");
            }
            return View();
        }

        private IList<Category> ListCategory()
        {
            return category.ListCategories();
        }

        private IList<Option> ListSelectedOption(int[] options)
        {
            var list = new List<Option>();
            foreach(var i in options)
            {
                var op = option.ListOption(i);
                list.Add(op);
            }
            return list;
        }
    }
}