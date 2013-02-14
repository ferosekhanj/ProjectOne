using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ProjectOne.Models;
using WebMatrix.WebData;

namespace ProjectOne.ViewModels
{
    /// <summary>
    /// This is the view model for the logging missed prayer
    /// </summary>
    public class MakeupViewModel
    {
        /// <summary>
        /// The days with open prayers
        /// </summary>
        public IList<DateTime> SalahDate
        {
            get;
            set;
        }

        /// <summary>
        /// The status of each prayer for the seven days days as rows
        /// </summary>
        public IList<bool> SalahStatus
        {
            get;
            set;
        }

        /// <summary>
        /// The repository to use for saving and loading data.
        /// </summary>
        public IPrayerLogRepository Repository { get; set; }


        /// <summary>
        /// Empty constructor
        /// </summary>
        public MakeupViewModel()
        {
            SalahDate = new List<DateTime>();
            SalahStatus = new List<bool>();
        }

        /// <summary>
        /// Fetch the data from the database
        /// </summary>
        public void LoadData(string theUserTimezoneId)
        {
            DateTime aToday = Common.GetTodayInTimezone(theUserTimezoneId);
            int aCurrentUserId = WebSecurity.CurrentUserId;
            IEnumerable<PrayerLog> aLogs = Repository.Find(aCurrentUserId);

            bool bCheckMissingMonth = false;
            DateTime aNextMonth = aToday;
            foreach (var log in aLogs)
            {
                PrayerLog aLog = log;

                // we dont store the records for months which doesn't have atleast one completed prayer
                // If the next month from db is not equal to the expected next month
                // Then add those months as in complete prayers.
                while (bCheckMissingMonth && (aNextMonth.Year != aLog.Year || aNextMonth.Month != aLog.Month))
                {
                    // a record is missing for the month
                    aLog = new PrayerLog(aNextMonth.Year, aNextMonth.Month, aCurrentUserId);
                    if (FindOpenPrayers(aLog,aToday))
                    {
                        return;
                    }
                    // move to next month
                    aNextMonth = new DateTime(aLog.Year, aLog.Month, 1).AddMonths(1);
                    // set the db record back
                    aLog = log;
                }

                if (FindOpenPrayers(aLog,aToday))
                {
                    return;
                }

                // Expected next month from the current month
                aNextMonth = new DateTime(aLog.Year, aLog.Month, 1).AddMonths(1);
                bCheckMissingMonth = true;
            }

            // If the user has not entered any data during the last set of months. Then include them too.
            while (aNextMonth < aToday)
            {
                if (FindOpenPrayers(new PrayerLog(aNextMonth.Year,aNextMonth.Month,aCurrentUserId),aToday))
                {
                    return;
                }
                aNextMonth = aNextMonth.AddMonths(1);
            }

        }

        /// <summary>
        /// Find the days when the prayers are not caclulated.
        /// </summary>
        /// <param name="log"></param>
        /// <param name="today"></param>
        /// <returns></returns>
        private bool FindOpenPrayers(PrayerLog log,DateTime today)
        {
            if (log.Completed)
            {
                return false;
            }
            DateTime aDay = new DateTime(log.Year, log.Month, 1,23,59,59);
            for (int i = 1; i <= DateTime.DaysInMonth(aDay.Year, aDay.Month) && aDay<today ; i++)
            {
                bool aFound = false;
                bool[] aStatus = new bool[5];
                for (int j = Waqt.SUBUH; j <= Waqt.ISHA; j++)
                {
                    aStatus[j] = log.IsCompleted(i, j);
                    aFound = aFound || !aStatus[j];
                }

                if (aFound)
                {
                    SalahDate.Add(aDay);
                    for (int j = Waqt.SUBUH; j <= Waqt.ISHA; j++)
                    {
                        SalahStatus.Add(aStatus[j]);
                    }
                    if (SalahDate.Count >= MAX_MISSING_PRAYER)
                        return true;
                }
                aDay = aDay.AddDays(1);
            }
            return false;
        }
        public void SaveData()
        {
            int aCurrentUserId = WebSecurity.CurrentUserId;
            // Save the status for the week
            PrayerLog aLog = null;

            for (int i = 0; i < SalahDate.Count; i++)
            {
                if (aLog == null)
                {
                    aLog = Repository.Find(SalahDate[i].Year, SalahDate[i].Month, aCurrentUserId);
                }

                if (SalahDate[i].Month != aLog.Month)
                {
                    //commit the current log and fetch next
                    Repository.AddOrUpdate(aLog);
                    aLog = Repository.Find(SalahDate[i].Year, SalahDate[i].Month, aCurrentUserId);
                }

                for (int j = Waqt.SUBUH; j <= Waqt.ISHA; j++)
                {
                    aLog.SetCompleted(SalahDate[i].Day, j, SalahStatus[i * 5 + j]);
                }
            }
            // commit the active log
            Repository.AddOrUpdate(aLog);
            Repository.Save();
        }

        private const int MAX_MISSING_PRAYER = 7;
    }
}