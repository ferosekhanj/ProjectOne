using ProjectOne.Mailers;
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
            if (WebSecurity.IsAuthenticated)
                return RedirectToAction("Index", "Log");

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
                    WebSecurity.CreateUserAndAccount(theFormData.Username, theFormData.Password, new { Email = theFormData.Email, Active = true, Timezone=Common.CalculateTimezoneFromOffset(theFormData.TimezoneOffset) }, false);
                    WebSecurity.Login(theFormData.Username, theFormData.Password);

                    IUserMailer aMailer = new UserMailer();
                    var aResetMail = aMailer.Welcome(theFormData.Username,theFormData.Email);
                    aResetMail.Send();

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
            using (IUserProfileRepository r = new UserProfileRepository(WebSecurity.CurrentUserId))
            {
                UserProfileViewModel pvm = new UserProfileViewModel();
                pvm.Repository = r;
                pvm.Load();
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
                if (WebSecurity.CurrentUserName=="demo786")
                {
                    return View(theFormData);
                }
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
                using (IUserProfileRepository r = new UserProfileRepository(WebSecurity.CurrentUserId))
                {
                    theFormData.Repository = r;
                    theFormData.Save();
                }

            }
            return View(theFormData);
        }
        //
        // GET: /Account/ForgotPassword
        [AllowAnonymous]
        public ActionResult ForgotPassword()
        {
            ViewBag.MailSent = false;
            return View();
        }

        //
        // POST: /Account/ForgotPassword
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public ActionResult ForgotPassword(string Username)
        {
            if (ModelState.IsValid)
            {
                if (!string.IsNullOrEmpty(Username))
                {
                    try
                    {
                        using (IUserProfileRepository r = new UserProfileRepository(Username))
                        {
                            var aResetToken = WebSecurity.GeneratePasswordResetToken(r.UserProfile.Name);
                            IUserMailer aMailer = new UserMailer();

                            var aResetMail = aMailer.ForgotPassword(r.UserProfile.Email, aResetToken);
                            aResetMail.Send();
                            ViewBag.MailSent = true;
                            ViewBag.Message = "We have sent a recovery mail to your email id. Follow the steps in the mail.";
                        }
                    }
                    catch (InvalidOperationException)
                    {
                        ViewBag.MailSent = false;
                        ModelState.AddModelError("", string.Format("We could not find an user linked to {0} in our database. Kindly check the username.", Username));
                    }
                }
            }
            return View();
        }

        //
        // GET: /Account/ResetPassword/{token}
        [AllowAnonymous]
        public ActionResult ResetPassword(string token)
        {
            ResetPasswordViewModel r = new ResetPasswordViewModel { NewPassword = "", Token = token };
            return View(r);
        }

        //
        // POST: /Account/ResetPassword/
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public ActionResult ResetPassword(ResetPasswordViewModel theFormData)
        {
            if (ModelState.IsValid)
            {
                if (!string.IsNullOrEmpty(theFormData.NewPassword) && !string.IsNullOrEmpty(theFormData.Token))
                {
                    try
                    {
                        WebSecurity.ResetPassword(theFormData.Token, theFormData.NewPassword);
                        return RedirectToAction("Login");
                    }
                    catch (InvalidOperationException e)
                    {
                        ModelState.AddModelError("", e.Message);
                    }
                }
            }
            return View(theFormData);
        }

        //
        // GET: /Account/ShowDemo/
        [AllowAnonymous]
        public ActionResult ShowDemo()
        {
            WebSecurity.Login("demo786", "123456", false);
            return RedirectToAction("Index", "Log");
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
