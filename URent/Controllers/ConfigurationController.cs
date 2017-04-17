using Ninject;
using System.Web.Mvc;
using URent.Models.Interfaces;

namespace URent.Controllers
{
    public class ConfigurationController : Controller
    {
        private readonly IClientManager _clientManager;
        private readonly ICarManager _carManager;
        private readonly ICategoryManager _categoryManager;
        private readonly IOptionManager _optionManager;
        private readonly IReservationManager _reservationManager;
        private readonly IRentPriceManager _priceManager;
        public ConfigurationController([Named("Prod")] IClientManager _clientManager, ICarManager _carManager, ICategoryManager _categoryManager, IOptionManager optionManager, IReservationManager _reservationManager, IRentPriceManager _priceManager)
        {
            this._clientManager = _clientManager;
            this._carManager = _carManager;
            this._categoryManager = _categoryManager;
            _optionManager = optionManager;
            this._reservationManager = _reservationManager;
            this._priceManager = _priceManager;
        }


        public ActionResult GenerateData()
        {
            _categoryManager.Generate();
            _carManager.Generate();
            _clientManager.Generate();
            _optionManager.Generate();
            _priceManager.Generate();
            _reservationManager.Generate();
            ViewBag.Message = "Data generated!";
            return View();
        }
    }
}