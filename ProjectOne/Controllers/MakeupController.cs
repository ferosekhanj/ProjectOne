using ProjectOne.Models;
using ProjectOne.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ProjectOne.Controllers
{
    public class MakeupController : Controller
    {
        //
        // GET: /Makeup/

        public ActionResult List()
        {
            using (IPrayerLogRepository aRepository = new PrayerLogRepository())
            {
                MakeupViewModel mvm = new MakeupViewModel();
                mvm.Repository = aRepository;
                mvm.LoadData();
                return View(mvm);
            }
        }

        [HttpPost]
        public ActionResult Save(MakeupViewModel mvm)
        {
            using (IPrayerLogRepository aRepository = new PrayerLogRepository())
            {
                if (ModelState.IsValid)
                {
                    mvm.Repository = aRepository;
                    mvm.SaveData();
                }
                return View("List", mvm);
            }
        }
    }
}
