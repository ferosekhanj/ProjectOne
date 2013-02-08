using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ProjectOne.ViewModels;
using ProjectOne.Models;

namespace ProjectOne.Controllers
{
    public class LogController : Controller
    {
        //
        // GET: /Log/Index/year/month/date

        public ActionResult Index(int year=0,int month=1,int day=1)
        {
            using (IPrayerLogRepository aRepository = new PrayerLogRepository())
            {
                LogViewModel aModel = new LogViewModel((year != 0) ? new DateTime(year, month, day) : DateTime.Now);
                aModel.Repository = aRepository;
                aModel.LoadData();
                return View(aModel);
            }
        }

        //
        // POST : /Log/Save/
        [HttpPost]
        public ActionResult Save(LogViewModel theModel)
        {
            using (IPrayerLogRepository aRepository = new PrayerLogRepository())
            {
                if (ModelState.IsValid)
                {
                    theModel.Repository = aRepository;
                    theModel.SaveData();
                    aRepository.Save();
                }
                return View("Index", theModel);
            }
        }
    }
}
