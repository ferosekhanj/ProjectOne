using ProjectOne.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProjectOne.ViewModels
{
    public class ReviewViewModel
    {
        public int Day { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }
        public int Count { get; set; }
        public float[] ItemPercentage { get; set; }
        public int[] ItemCount { get; set; }
        public float[] PrayerPercent { get; set; }
        public int[] PrayerCount { get; set; }
        public IPrayerLogRepository Repository { get; set; }

        public ReviewViewModel(IPrayerLogRepository theRepository, int theYear)
        {
            Repository = theRepository;

            Year = theYear;
            ItemPercentage = new float[12];
            ItemCount = new int[12];

            PrayerPercent = new float[5];
            PrayerCount = new int[5];

            FetchData(theYear);
        }
        public ReviewViewModel(IPrayerLogRepository theRepository, int theYear, int theMonth)
        {
            Repository = theRepository;

            Month = theMonth;
            Year = theYear;

            ItemPercentage = new float[DateTime.DaysInMonth(theYear, theMonth)];
            ItemCount = new int[ItemPercentage.Length];

            PrayerPercent = new float[5];
            PrayerCount = new int[5];

            FetchData(theYear, theMonth);
        }

        private void FetchData(int theYear)
        {
            //TODO: Fetch this from the login session
            int aCurrentUserId = 0;

            IEnumerable<PrayerLog> logs = Repository.Find(theYear, aCurrentUserId);
            foreach (var log in logs)
            {
                ItemPercentage[log.Month - 1] = ((float)log.Count) / (DateTime.DaysInMonth(log.Year, log.Month) * 5.0f);
                ItemCount[log.Month - 1] = log.Count;
                for (int i = Waqt.SUBUH; i <= Waqt.ISHA; i++)
                {
                    PrayerCount[i] += log.GetPrayerCount(i);
                }
            }
            float aMaxCount = (theYear % 4 == 0) ? 366.0f : 365.0f;
            for (int i = Waqt.SUBUH; i <= Waqt.ISHA; i++)
            {
                PrayerPercent[i] = (PrayerCount[i]) / aMaxCount;
            }
        }

        private void FetchData(int theYear, int theMonth)
        {
            //TODO: Fetch this from the login session
            int aCurrentUserId = 0;

            PrayerLog log = Repository.Find(theYear, theMonth, aCurrentUserId);
            for (int day = 0; day < ItemCount.Length; day++)
            {
                for (int i = Waqt.SUBUH; i <= Waqt.ISHA; i++)
                {
                    ItemCount[day] += log.IsCompleted(day + 1, i) ? 1 : 0;
                }
                ItemPercentage[day] = ItemCount[day] / 5.0f;
            }
            for (int i = Waqt.SUBUH; i <= Waqt.ISHA; i++)
            {
                PrayerCount[i] = log.GetPrayerCount(i);
                PrayerPercent[i] = ((float)PrayerCount[i]) / ItemCount.Length;
            }

        }
   }
}