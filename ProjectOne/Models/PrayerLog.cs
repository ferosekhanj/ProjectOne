using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProjectOne.Models
{
    public class PrayerLog
    {
        public int Id { get; set; }
        public int Year { get; set; }
        public int Month { get; set; }
        public int Count { get; set; }
        public int UserId { get; set; }
        public bool Completed { get; set; }
        public PrayerLogDetail Details { get; set; }

        public PrayerLog()
        {
        }

        public PrayerLog(int theYear,int theMonth,int theUserId)
        {
            Id = -1;
            Year = theYear;
            Month = theMonth;
            UserId = theUserId;
            Completed = false;
            Details = new PrayerLogDetail { Id = -1 };
        }

        public bool IsCompleted(int theDay, int theSalah)
        {
            return Completed || Details.GetStatus(theDay,theSalah);
        }

        public int GetPrayerCount(int theSalah)
        {
            return Completed ? DateTime.DaysInMonth(Year, Month) : Details.GetCount(theSalah);
        }

        public void SetCompleted(int theDay, int theSalah,bool theIsCompleted)
        {
            if(!Completed || !theIsCompleted)
            {
                Details.SetStatus(theDay, theSalah,theIsCompleted);
            }
        }

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