using Ninject;
using System.Web.Mvc;
using URent.Models.Interfaces;

namespace URent.Controllers
{
    public class UserController : Controller
    {
        private readonly IClient client;
        public UserController([Named("Prod")] IClient _client)
        {
            client = _client;
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Generate()
        {
            ViewBag.returnUser = client.Generate();
            return View();
        }

        public ActionResult Login()
        {
            var _user = new Models.Model.Client();
            _user.Email = "marcos@gmail.com";
            _user.Password = "13324";
            var obj = client.Authentification(_user);
            return View(obj);
        }
    }
}