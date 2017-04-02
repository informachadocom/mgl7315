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
    public class AccountController : Controller
    {
        private readonly IClient client;
        public AccountController([Named("Prod")] IClient _client)
        {
            client = _client;
        }

        // GET: Account
        public ActionResult Index()
        {
            return View();

        }

        //
        // GET: /Account/Register
        [AllowAnonymous]
        public ActionResult Register()
        {
            //Session["ClientId"] = 5;
            if (ClientId > 0)
            {
                var model = client.ListClient(ClientId);
                return View(model);
            }
            return View();
        }

        //
        // GET: /Account/Register
        [AllowAnonymous]
        public ActionResult Manage()
        {
            //Session["ClientId"] = 5;
            if (ClientId > 0)
            {
                var model = client.ListClient(ClientId);
                return View(model);
            }
            return View();
        }

        //
        // POST: /Account/Register
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Register(Client model)
        {
            if (ModelState.IsValid)
            {
                var result = client.CreateUpdate(model);
                if (result)
                {
                    LoadSession(model);
                    if (TempData["Redirect"] != null)
                    {
                        return RedirectToAction("Summary", "Home");
                    }
                    else
                    {
                        return RedirectToAction("Index", "Home");
                    }
                }
                AddErrors("Error");
            }

            // Something failed, redisplay form
            return View(model);
        }

        //
        // POST: /Account/Manage
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Manage(Client model)
        {
            if (ModelState.IsValid)
            {
                var result = client.CreateUpdate(model);
                if (result)
                {
                    LoadSession(model);
                    return RedirectToAction("Index", "Home");
                }
                AddErrors("Error");
            }

            // Something failed, redisplay form
            return View(model);
        }

        public ActionResult Login()
        {
            if (client.isAuthenticated())
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginViewModel model)
        {
            var modelClient = new Client { Email = model.Email, Password = model.Password };
            var result = client.Authentification(modelClient);
            if (result.ClientId > 0)
            {
                LoadSession(result);
                if (TempData["Redirect"] != null)
                {
                    return Redirect(TempData["Redirect"].ToString());
                }
                else
                {
                    return RedirectToAction("Index", "Home");
                }
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

        private void LoadSession(Client model)
        {
            Session["ClientId"] = model.ClientId;
            Session["Name"] = model.FirstName;
        }
    }
}