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
        // POST: /Account/Register
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Register(ClientViewModel model)
        {
            if (ModelState.IsValid)
            {
                //Check email already exists
                var emailExist = client.CheckAvailableEmail(0, model.Email);

                if (!emailExist)
                {
                    var modelClient = new Client();
                    modelClient.ClientId = model.ClientId;
                    modelClient.FirstName = model.FirstName;
                    modelClient.Surname = model.Surname;
                    modelClient.Email = model.Email;
                    modelClient.Password = model.Password;
                    var result = client.CreateUpdate(modelClient);
                    if (result)
                    {
                        LoadSession(modelClient);
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
                else
                {
                    AddErrors("This email already exists.");
                }
            }

            // Something failed, redisplay form
            return View(model);
        }

        //
        // GET: /Account/Manage
        [AllowAnonymous]
        public ActionResult Manage()
        {
            Session["ClientId"] = 5;
            if (ClientId > 0)
            {
                var modelClient = client.ListClient(ClientId);
                var model = new UpdateClientViewModel();
                model.ClientId = modelClient.ClientId;
                model.FirstName = modelClient.FirstName;
                model.Surname = modelClient.Surname;
                model.Email = modelClient.Email;
                return View(model);
            }
            return View();
        }

        //
        // POST: /Account/Manage
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Manage(UpdateClientViewModel model)
        {
            if (ModelState.IsValid)
            {
                //Check email already exists
                var emailExist = client.CheckAvailableEmail(ClientId, model.Email);

                if (!emailExist)
                {

                    var modelClient = client.ListClient(model.ClientId);
                    modelClient.FirstName = model.FirstName;
                    modelClient.Surname = model.Surname;
                    modelClient.Email = model.Email;
                    var result = client.CreateUpdate(modelClient);
                    if (result)
                    {
                        LoadSession(modelClient);
                        return RedirectToAction("Index", "Home");
                    }
                    AddErrors("Error");
                }
                else
                {
                    AddErrors("This email already exists.");
                }
            }

            // Something failed, redisplay form
            return View(model);
        }

        //
        // POST: /Account/Delete
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(UpdateClientViewModel model)
        {
            if (model.ClientId > 0)
            {
                var result = client.Remove(model.ClientId);
                if (result)
                {
                    Session.Clear();
                    return RedirectToAction("Index", "Home");
                }
            }
            return View("Manage");
        }

        //
        // GET: /Account/ChangePassword
        [AllowAnonymous]
        public ActionResult ChangePassword()
        {
            //Session["ClientId"] = 5;
            if (ClientId > 0)
            {
                return View();
            }
            return RedirectToAction("../Home/Index");
        }

        //
        // POST: /Account/ChangePassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ChangePassword(ChangePasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                //Load client
                var modelClient = client.ListClient(ClientId);
                modelClient.Password = model.CurrentPassword;
                //Check if current password is correct
                var resultAuth = client.Authentification(modelClient);
                if (resultAuth != null && resultAuth.ClientId > 0)
                {
                    modelClient.Password = model.NewPassword;
                    var result = client.CreateUpdate(modelClient);
                    if (result)
                    {
                        LoadSession(modelClient);
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
                else
                {
                    AddErrors("Current password does not match!");
                }
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
            if (result != null && result.ClientId > 0)
            {
                LoadSession(result);
                if (TempData["Redirect"] != null && TempData["Redirect"].ToString() == "Summary")
                {
                    return View("../Home/Summary", (Order)Session["order"]);
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