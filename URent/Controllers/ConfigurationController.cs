using Ninject;
using System.Web.Mvc;
using URent.Models.Interfaces;

namespace URent.Controllers
{
    public class ConfigurationController : Controller
    {
        private readonly IClient _client;
        private readonly ICar _car;
        private readonly ICategory _category;
        private readonly IOption _option;
        private readonly IReservation _reservation;
        private readonly IRentPrice _price;
        public ConfigurationController([Named("Prod")] IClient client, ICar car, ICategory category, IOption option, IReservation reservation, IRentPrice price)
        {
            _client = client;
            _car = car;
            _category = category;
            _option = option;
            _reservation = reservation;
            _price = price;
        }


        public ActionResult GenerateData()
        {
            _category.Generate();
            _car.Generate();
            _client.Generate();
            _option.Generate();
            _price.Generate();
            _reservation.Generate();
            ViewBag.Message = "Data generated!";
            return View();
        }
    }
}