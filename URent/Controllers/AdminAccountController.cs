using Ninject;
using System.Web.Mvc;
using URent.Models;
using URent.Models.Interfaces;
using URent.Models.Model;

namespace URent.Controllers
{
    public class AdminAccountController : Controller
    {
        private readonly IUser _user;
        public AdminAccountController([Named("Prod")] IUser user)
        {
            _user = user;
        }

        //
        // GET: /AdminAccount/Register
        [AllowAnonymous]
        public ActionResult Register()
        {
            //Session["ClientId"] = 5;
            return View();
        }

        //
        // POST: /AdminAccount/Register
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Register(ClientViewModel model)
        {
            if (ModelState.IsValid)
            {
                //Check email already exists
                var emailExist = _user.CheckAvailableEmail(0, model.Email);

                if (!emailExist)
                {
                    var modelClient = new User();
                    modelClient.UserId = model.ClientId;
                    modelClient.FirstName = model.FirstName;
                    modelClient.Surname = model.Surname;
                    modelClient.Email = model.Email;
                    modelClient.Password = model.Password;
                    var result = _user.CreateUpdate(modelClient);
                    if (result)
                    {
                        return RedirectToAction("Index", "AdminHome");
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
        // GET: /AdminAccount/Manage
        [AllowAnonymous]
        public ActionResult Manage()
        {
            //Session["ClientId"] = 5;
            if (UserId > 0)
            {
                var modelUser = _user.ListUser(UserId);
                var model = new UpdateClientViewModel();
                model.ClientId = modelUser.UserId;
                model.FirstName = modelUser.FirstName;
                model.Surname = modelUser.Surname;
                model.Email = modelUser.Email;
                return View(model);
            }
            else
            {
                return RedirectToAction("Index", "AdminHome");
            }
        }

        //
        // POST: /AdminAccount/Manage
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Manage(UpdateClientViewModel model)
        {
            if (ModelState.IsValid)
            {
                //Check email already exists
                var emailExist = _user.CheckAvailableEmail(UserId, model.Email);

                if (!emailExist)
                {

                    var modelUser = _user.ListUser(model.ClientId);
                    modelUser.FirstName = model.FirstName;
                    modelUser.Surname = model.Surname;
                    modelUser.Email = model.Email;
                    var result = _user.CreateUpdate(modelUser);
                    if (result)
                    {
                        return RedirectToAction("Index", "AdminHome");
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
        // POST: /AdminAccount/Delete
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(UpdateClientViewModel model)
        {
            if (model.ClientId > 0)
            {
                var result = _user.Remove(model.ClientId);
                if (result)
                {
                    Session.Clear();
                    return RedirectToAction("Index", "AdminHome");
                }
            }
            return View("Manage");
        }

        //
        // GET: /AdminAccount/ChangePassword
        [AllowAnonymous]
        public ActionResult ChangePassword()
        {
            //Session["ClientId"] = 5;
            if (UserId > 0)
            {
                return View();
            }
            return RedirectToAction("Index", "AdminHome");
        }

        //
        // POST: /AdminAccount/ChangePassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ChangePassword(ChangePasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                //Load client
                var modelUser = _user.ListUser(UserId);
                modelUser.Password = model.CurrentPassword;
                //Check if current password is correct
                var resultAuth = _user.Authentification(modelUser);
                if (resultAuth != null && resultAuth.UserId > 0)
                {
                    modelUser.Password = model.NewPassword;
                    var result = _user.CreateUpdate(modelUser);
                    if (result)
                    {
                        return RedirectToAction("Index", "AdminHome");
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
            if (_user.IsAuthenticated())
            {
                return RedirectToAction("Index", "AdminHome");
            }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginViewModel model)
        {
            if (!_user.IsAuthenticated())
            {
                var modelUser = new User { Email = model.Email, Password = model.Password };
                var result = _user.Authentification(modelUser);
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
            else
            {
                return RedirectToAction("Index", "AdminHome");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            Session.Clear();
            return RedirectToAction("Login", "AdminAccount");
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
            Session["NameAdmin"] = model.FirstName;
        }
    }

}