using ProjectOne.Models;
using ProjectOne.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ProjectOne.Controllers
{
    public class ReviewController : Controller
    {
        //
        // GET: /Review/{year}

        public ActionResult Year(int year)
        {
            using (IPrayerLogRepository aRepository = new PrayerLogRepository())
            {
                ReviewViewModel rvm = new ReviewViewModel(aRepository,year);
                return View(rvm);
            }
        }

        //
        // GET: /Review/{year}/{month}
        public ActionResult Month(int year, int month)
        {
            using (IPrayerLogRepository aRepository = new PrayerLogRepository())
            {
                ReviewViewModel rvm = new ReviewViewModel(aRepository, year, month);
                return View(rvm);
            }
        }
    }
}
