using Ninject;
using System.Web.Mvc;
using URent.Models;
using URent.Models.Interfaces;
using URent.Models.Model;

namespace URent.Controllers
{
    public class AdminAccountController : Controller
    {
        private readonly IUser user;
        public AdminAccountController([Named("Prod")] IUser _user)
        {
            user = _user;
        }

        // GET: AdminAccount
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Login()
        {
            if (user.isAuthenticated())
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginViewModel model)
        {
            var modelUser = new User { Email = model.Email, Password = model.Password };
            var result = user.Authentification(modelUser);
            if (result != null && result.UserId > 0)
            {
                LoadSession(result);
                return RedirectToAction("Index", "AdminHome");
            }
            else
            {
                AddErrors("Email and/or Password does not match!");
            }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            Session.Clear();
            return RedirectToAction("Index", "Home");
        }

        private void AddErrors(string error)
        {
            ModelState.AddModelError("", error);
        }

        private int UserId
        {
            get
            {
                if (Session["UserId"] != null)
                {
                    return int.Parse(Session["UserId"].ToString());
                }
                return 0;
            }
        }

        private void LoadSession(User model)
        {
            Session["UserId"] = model.UserId;
            Session["Name"] = model.FirstName;
        }
    }

}