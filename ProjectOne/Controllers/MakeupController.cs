using ProjectOne.Models;
using ProjectOne.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebMatrix.WebData;

namespace ProjectOne.Controllers
{
    [Authorize]
    public class MakeupController : Controller
    {
        //
        // GET: /Makeup/

        public ActionResult List()
        {
            UserProfile aProfile;
            using (var db = new DatabaseContext())
            {
                using (IUserProfileRepository aRepository = new UserProfileRepository(WebSecurity.CurrentUserId,db))
                {
                    aProfile = aRepository.UserProfile;
                }
                using (IPrayerLogRepository aRepository = new PrayerLogRepository(db))
                {
                    MakeupViewModel mvm = new MakeupViewModel();
                    mvm.Repository = aRepository;
                    mvm.LoadData(aProfile.Timezone);
                    return View(mvm);
                }
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
                return RedirectToAction("List");
            }
        }
    }
}
