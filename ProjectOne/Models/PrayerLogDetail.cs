using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ProjectOne.Models
{
    public class Waqt
    {
        public static int SUBUH = 0;
        public static int ZOHAR = 1;
        public static int ASR = 2;
        public static int MAGRIB = 3;
        public static int ISHA = 4;
    }

    /// <summary>
    /// The status of individual parayer for a single day.
    /// The individual salh properties are required for storing the data to db.
    /// </summary>
    public class PrayerLogDetail
    {
        int[] myDetails = new int[5];

        /// <summary>
        /// This identifies the related prayer log.
        /// </summary>
        public int PrayerLogId { get; set; }

        public int Subuh 
        { 
            get
            { 
                return myDetails[Waqt.SUBUH];
            }
            set
            {
                myDetails[Waqt.SUBUH] = value;
            }
        }
        public int Zohar
        {
            get
            {
                return myDetails[Waqt.ZOHAR];
            }
            set
            {
                myDetails[Waqt.ZOHAR] = value;
            }
        }
        public int Asr
        {
            get
            {
                return myDetails[Waqt.ASR];
            }
            set
            {
                myDetails[Waqt.ASR] = value;
            }
        }
        public int Magrib
        {
            get
            {
                return myDetails[Waqt.MAGRIB];
            }
            set
            {
                myDetails[Waqt.MAGRIB] = value;
            }
        }
        public int Isha
        {
            get
            {
                return myDetails[Waqt.ISHA];
            }
            set
            {
                myDetails[Waqt.ISHA] = value;
            }
        }

        /// <summary>
        /// The related prayer log
        /// </summary>
        public PrayerLog MyLog { get; set; }

        /// <summary>
        /// Find the status of a particular prayer on a particular day.
        /// </summary>
        /// <param name="theDay">Day</param>
        /// <param name="theSalah">Salah</param>
        /// <returns>Completion Status</returns>
        public bool GetStatus(int theDay,int theSalah)
        {
            return (myDetails[theSalah] & (1 << (theDay - 1))) > 0;
        }

        /// <summary>
        /// Set the status of a particular prayer on a particular day.
        /// </summary>
        /// <param name="theDay">Day</param>
        /// <param name="theSalah">Salah</param>
        /// <param name="theStatus">Status</param>
        public void SetStatus(int theDay, int theSalah, bool theStatus)
        {
            if( theStatus) 
                myDetails[theSalah] |= (1 << (theDay - 1));
            else
                myDetails[theSalah] &= ~(1 << (theDay - 1));
        }

        /// <summary>
        /// Check whether all the prayers are marked complete
        /// </summary>
        /// <param name="theCount">Number of days</param>
        /// <returns>Status</returns>
        public bool IsAllCompleted(int theCount)
        {
            int aMaxValue = (theCount==31)? 0x7fffffff: (1<<theCount)-1;
            int i = Waqt.SUBUH;
            while(i<5 && myDetails[i++]==aMaxValue);
            return i > Waqt.ISHA;
        }

        /// <summary>
        /// Count the days a particular prayer hsa been completed
        /// </summary>
        /// <param name="theSalah"></param>
        /// <returns></returns>
        public int GetCount(int theSalah)
        {
            return NumberOfSetBits(myDetails[theSalah]);
        }

        /// <summary>
        /// Based on http://graphics.stanford.edu/~seander/bithacks.html#CountBitsSetParallel
        /// This just works
        /// Its majic ;-)
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        private int NumberOfSetBits(int i)
        {
            i = i - ((i >> 1) & 0x55555555);
            i = (i & 0x33333333) + ((i >> 2) & 0x33333333);
            return (((i + (i >> 4)) & 0x0F0F0F0F) * 0x01010101) >> 24;
        }
    }
}
