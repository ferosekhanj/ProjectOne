using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ProjectOne.Models;

namespace ProjectOne.ViewModels
{
    /// <summary>
    /// This is the view model for the logging prayer
    /// </summary>
    public class LogViewModel
    {
        /// <summary>
        /// The starting day of the week
        /// </summary>
        public DateTime StartDate
        {
            get
            {
                return myStartDate;
            }
            set
            {
                myStartDate = value;
                myDate = myStartDate;
            }
        }

        /// <summary>
        /// The end date of the week
        /// </summary>
        public DateTime EndDate
        {
            get
            {
                return myEndDate;
            }
            set
            {
                myEndDate = value;
            }
        }

        /// <summary>
        /// The status of each prayer for the seven days days as rows
        /// </summary>
        public bool[] SalahStatus
        {
            get
            {
                return mySalahStatus;
            }
            set
            {
                mySalahStatus = value;
            }
        }

        /// <summary>
        /// The repository to use for saving and loading data.
        /// </summary>
        public IPrayerLogRepository Repository { get; set; }

        /// <summary>
        /// Empty constructor
        /// </summary>
        public LogViewModel()
        {
            
        }

        /// <summary>
        /// Constructor. This will findout the week from the provided date.
        /// </summary>
        /// <param name="theDate">Any date.</param>
        public LogViewModel(DateTime theDate)
        {
            if (theDate == null)
            {
                throw new ArgumentNullException("theDate");
            }
            myDate = theDate;
            CalculateWeek();
        }

        /// <summary>
        /// Calculates the week.
        /// </summary>
        private void CalculateWeek()
        {
            // Calculate the week range
            myStartDate = myDate.AddDays(-(double)myDate.DayOfWeek);
            myEndDate = myStartDate.AddDays(7.0);
        }

        /// <summary>
        /// Fetch the data from the database
        /// </summary>
        public void LoadData()
        {
            mySalahStatus = new bool[35];

            //TODO: Fetch this from the login session
            int aCurrentUserId = 0;
            // Fetch the status for the week

            PrayerLog aLog = Repository.Find(StartDate.Year, StartDate.Month, aCurrentUserId);
            int i = 0;
            for (DateTime d = myStartDate; d < myEndDate; d = d.AddDays(1), i++)
            {
                if (d.Month != aLog.Month)
                {
                    aLog = Repository.Find(d.Year, d.Month, aCurrentUserId);
                }

                for (int j = Waqt.SUBUH; j <= Waqt.ISHA; j++)
                {
                    mySalahStatus[i * 5 + j] = aLog.IsCompleted(d.Day, j);
                }
            }
        }
        public void SaveData()
        {
            //TODO: Fetch this from the login session
            int aCurrentUserId = 0;
            // Save the status for the week
            PrayerLog aLog = Repository.Find(StartDate.Year, StartDate.Month, aCurrentUserId);
            int i = 0;
            for (DateTime d = myStartDate; d < myEndDate; d = d.AddDays(1), i++)
            {
                if (d.Month != aLog.Month)
                {
                    Repository.AddOrUpdate(aLog);
                    aLog = Repository.Find(d.Year, d.Month, aCurrentUserId);
                }

                for (int j = Waqt.SUBUH; j <= Waqt.ISHA; j++)
                {
                    aLog.SetCompleted(d.Day, j, mySalahStatus[i * 5 + j]);
                }
            }
            Repository.AddOrUpdate(aLog);
        }
        bool[] mySalahStatus;

        DateTime myDate;
        DateTime myStartDate;
        DateTime myEndDate;

    }
}