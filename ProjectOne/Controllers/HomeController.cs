using ProjectOne.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ProjectOne.Controllers
{
    public class HomeController : Controller
    {
        //
        // GET: /Home/

        public ActionResult Index()
        {
            return View(new SignupViewModel { TimezoneOffset = 0.0 });
        }

        //
        // GET: /Home/Login/
        public ActionResult Login()
        {
            return View();
        }
    }
}
