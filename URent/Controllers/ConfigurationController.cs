using Ninject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using URent.Models.Interfaces;

namespace URent.Controllers
{
    public class ConfigurationController : Controller
    {
        private readonly IClient client;
        private readonly ICar car;
        private readonly ICategory category;
        private readonly IOption option;
        private readonly IReservation reservation;
        private readonly IRentPrice price;
        public ConfigurationController([Named("Prod")] IClient _client, ICar _car, ICategory _category, IOption _option, IReservation _reservation, IRentPrice _price)
        {
            client = _client;
            car = _car;
            category = _category;
            option = _option;
            reservation = _reservation;
            price = _price;
        }


        // GET: Configuration
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GenerateData()
        {
            category.Generate();
            car.Generate();
            client.Generate();
            option.Generate();
            price.Generate();
            //reservation.Generate();
            ViewBag.Message = "Data generated!";
            return View();
        }
    }
}