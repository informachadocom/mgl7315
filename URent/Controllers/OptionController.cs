using Ninject;
using System.Web.Mvc;
using URent.Models.Interfaces;

namespace URent.Controllers
{
    public class OptionController : Controller
    {
        private readonly IOption option;
        public OptionController([Named("Prod")] IOption _option)
        {
            option = _option;
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Generate()
        {
            ViewBag.returnOption = option.Generate();
            return View();
        }

        public ActionResult List()
        {
            var list = option.ListOptions();
            return View(list);
        }
    }
}