using ProjectOne.Models;
using ProjectOne.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using WebMatrix.WebData;

namespace ProjectOne.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        //
        // GET: /Account/Login
        [AllowAnonymous]
        public ActionResult Login()
        {
            return View();
        }

        //
        // GET: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginViewModel theFormData)
        {
            if (ModelState.IsValid && WebSecurity.Login(theFormData.Username, theFormData.Password, theFormData.RememberMe))
            {
                return RedirectToAction("Index", "Log");
            }

            // If we got this far, something failed, redisplay form
            ModelState.AddModelError("", "The user name or password provided is incorrect.");
            return View();
        }

        //
        // POST: /Account/Signup
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Signup(SignupViewModel theFormData)
        {
            if (ModelState.IsValid)
            {
                // Attempt to register the user
                try
                {
                    WebSecurity.CreateUserAndAccount(theFormData.Username, theFormData.Password, new { Email = theFormData.Email, Active = true }, false);
                    WebSecurity.Login(theFormData.Username, theFormData.Password);
                    return RedirectToAction("Index", "Log");
                }
                catch (MembershipCreateUserException e)
                {
                    ModelState.AddModelError("", ErrorCodeToString(e.StatusCode));
                }
            }
            return View(theFormData);
        }

        //
        // GET: /Account/Logout
        public ActionResult Logout()
        {
            WebSecurity.Logout();
            return View();
        }

        //
        // GET: /Account/Profile
        public ActionResult UserProfile()
        {
            using (DatabaseContext db = new DatabaseContext())
            {
                UserProfileViewModel pvm = new UserProfileViewModel();
                pvm.Load(db);
                return View(pvm);
            }
        }

        //
        // POST: /Account/Profile
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UserProfile(UserProfileViewModel theFormData)
        {
            if (ModelState.IsValid)
            {
                if (!string.IsNullOrEmpty(theFormData.OldPassword) 
                    && !string.IsNullOrEmpty(theFormData.NewPassword) 
                    && !string.IsNullOrEmpty(theFormData.ConfirmPassword))
                {
                    try
                    {
                        WebSecurity.ChangePassword(WebSecurity.CurrentUserName, theFormData.OldPassword, theFormData.NewPassword);
                    }
                    catch (MembershipCreateUserException e)
                    {
                        ModelState.AddModelError("", ErrorCodeToString(e.StatusCode));
                    }
                }
                using (DatabaseContext db = new DatabaseContext())
                {
                    theFormData.Save(db);
                }

            }
            return View(theFormData);
        }

        private static string ErrorCodeToString(MembershipCreateStatus createStatus)
        {
            switch (createStatus)
            {
                case MembershipCreateStatus.DuplicateUserName:
                    return "User name already exists. Please enter a different user name.";

                case MembershipCreateStatus.DuplicateEmail:
                    return "A user name for that e-mail address already exists. Please enter a different e-mail address.";

                case MembershipCreateStatus.InvalidPassword:
                    return "The password provided is invalid. Please enter a valid password value.";

                case MembershipCreateStatus.InvalidEmail:
                    return "The e-mail address provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidUserName:
                    return "The user name provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.ProviderError:
                    return "The authentication provider returned an error. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

                case MembershipCreateStatus.UserRejected:
                    return "The user creation request has been canceled. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

                default:
                    return "An unknown error occurred. Please verify your entry and try again. If the problem persists, please contact your system administrator.";
            }
        }

    }
}
