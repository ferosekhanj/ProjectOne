using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProjectOne.Models
{
    /// <summary>
    /// This class represents the prayer log for a particular month
    /// </summary>
    public class PrayerLog
    {

        public int Id { get; set; }
        public int Year { get; set; }
        public int Month { get; set; }
        public int Count { get; set; }
        public int UserId { get; set; }
        public bool Completed { get; set; }
        public PrayerLogDetail Details { get; set; }

        /// <summary>
        /// Default ctor
        /// </summary>
        public PrayerLog()
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="theYear"></param>
        /// <param name="theMonth"></param>
        /// <param name="theUserId"></param>
        public PrayerLog(int theYear,int theMonth,int theUserId)
        {
            Id = -1;
            Year = theYear;
            Month = theMonth;
            UserId = theUserId;
            Completed = false;
            Details = new PrayerLogDetail { PrayerLogId = -1 };
        }

        /// <summary>
        /// Returns whether a particular prayer was completed for a particular day.
        /// </summary>
        /// <param name="theDay">The day</param>
        /// <param name="theSalah">The salah</param>
        /// <returns></returns>
        public bool IsCompleted(int theDay, int theSalah)
        {
            return Completed || Details.GetStatus(theDay,theSalah);
        }

        /// <summary>
        /// Counts the number of days a particular prayer was completed within a month
        /// </summary>
        /// <param name="theSalah">the salah</param>
        /// <returns>Count of the salah</returns>
        public int GetPrayerCount(int theSalah)
        {
            return Completed ? DateTime.DaysInMonth(Year, Month) : Details.GetCount(theSalah);
        }

        /// <summary>
        /// Mark the status of a particular prayer as for a particular day.
        /// </summary>
        /// <param name="theDay">Day</param>
        /// <param name="theSalah">Salah</param>
        /// <param name="theIsCompleted">Status</param>
        public void SetCompleted(int theDay, int theSalah,bool theIsCompleted)
        {
            if(!Completed || !theIsCompleted)
            {
                Details.SetStatus(theDay, theSalah,theIsCompleted);
            }
        }

        /// <summary>
        /// Synchronize the statistics with the prayer details
        /// </summary>
        public void RecalculateStats()
        {
            int aDaysInMonth = DateTime.DaysInMonth(Year, Month);
            Completed = Details.IsAllCompleted(aDaysInMonth);
            if (Completed)
            {
                Count = aDaysInMonth * 5;
            }
            else
            {
                Count =0;
                for (int i = Waqt.SUBUH; i <= Waqt.ISHA; i++) Count += Details.GetCount(i);
            }
        }
    }
}