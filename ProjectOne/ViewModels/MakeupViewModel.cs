using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ProjectOne.Models;

namespace ProjectOne.ViewModels
{
    /// <summary>
    /// This is the view model for the logging missed prayer
    /// </summary>
    public class MakeupViewModel
    {
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
        public void LoadData()
        {
            //TODO: Fetch this from the login session
            int aCurrentUserId = 0;
            IEnumerable<PrayerLog> aLogs = Repository.Find(aCurrentUserId);

            bool bCheckMissingMonth = false;
            DateTime aNextMonth = DateTime.Now;
            foreach (var log in aLogs)
            {
                PrayerLog aLog = log;

                // Check if there is a break in the db records.
                // we dont store the records for months which doesn't have atleast one completed prayer
                while (bCheckMissingMonth && (aNextMonth.Year != aLog.Year || aNextMonth.Month != aLog.Month))
                {
                    // a record is missing for the month
                    aLog = new PrayerLog(aNextMonth.Year, aNextMonth.Month, aCurrentUserId);
                    if (FindOpenPrayers(aLog))
                    {
                        return;
                    }
                    // move to next month
                    aNextMonth = new DateTime(aLog.Year, aLog.Month, 1).AddMonths(1);
                    // set the db record back
                    aLog = log;
                }

                if (FindOpenPrayers(aLog))
                {
                    break;
                }
                aNextMonth = new DateTime(aLog.Year, aLog.Month, 1).AddMonths(1);
                bCheckMissingMonth = true;
            }

        }
        private bool FindOpenPrayers(PrayerLog log)
        {
            if (log.Completed)
            {
                return false;
            }
            DateTime aDay = new DateTime(log.Year, log.Month, 1);
            for (int i = 1; i <= DateTime.DaysInMonth(aDay.Year, aDay.Month); i++)
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
                    if (SalahDate.Count >= 7)
                        return true;
                }
                aDay = aDay.AddDays(1);
            }
            return false;
        }
        public void SaveData()
        {
            //TODO: Fetch this from the login session
            int aCurrentUserId = 0;
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
        }
    }
}