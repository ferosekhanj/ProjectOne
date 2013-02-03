using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProjectOne.Models
{
    public interface IPrayerLogRepository:IDisposable
    {
        PrayerLog Find(int theYear,int theMonth,int theUserId);
        IEnumerable<PrayerLog> Find(int theYear, int theUserId);
        IEnumerable<PrayerLog> Find(int theUserId);
        void AddOrUpdate(PrayerLog theLog);
        void Delete(PrayerLog theLog);
        void Save();
    }

    /// <summary>
    /// This exposes the model objects. It abstracts the persistence details.
    /// </summary>
    public class PrayerLogRepository :IPrayerLogRepository
    {
        List<PrayerLog> myLogs;

        /// <summary>
        /// 
        /// </summary>
        public PrayerLogRepository()
        {
            myLogs = HttpContext.Current.Session["CurrentLog"] as List<PrayerLog>;

            if (myLogs == null)
            {
                myLogs = new List<PrayerLog>()
                { 
                    new PrayerLog{
                            Id=0,
                            Month=12,
                            Year=2012,
                            Count=155,
                            UserId =0,
                            Details = new PrayerLogDetail
                            {
                                Id=0,
                                Subuh=0x7fffffff,
                                Zohar=0x7fffffff,
                                Asr=0x7fffffff,
                                Magrib=0x7fffffff,
                                Isha=0x7fffffff
                            }
                    },
                    new PrayerLog{
                            Id=1,
                            Month=2,
                            Year=2013,
                            Count=10,
                            UserId =0,
                            Details = new PrayerLogDetail
                            {
                                Id=1,
                                Subuh=0x6,
                                Zohar=0x6,
                                Asr=0x6,
                                Magrib=0x6,
                                Isha=0x6
                            }
                    },
                    new PrayerLog{
                            Id=2,
                            Month=1,
                            Year=2013,
                            Count=0,
                            UserId =0,
                            Details = new PrayerLogDetail
                            {
                                Id=2,
                                Subuh=0,
                                Zohar=0,
                                Asr=0,
                                Magrib=0,
                                Isha=0
                            }
                    }
                };
                HttpContext.Current.Session["CurrentLog"] = myLogs;
            }
        }

        
        public PrayerLog Find(int theYear, int theMonth, int theUserId)
        {
            IEnumerable<PrayerLog> aLogs = from b in myLogs where b.UserId == theUserId && b.Year == theYear && b.Month == theMonth select b;
            var aLogsEnumerator = aLogs.GetEnumerator();
            return (aLogsEnumerator.MoveNext())?aLogsEnumerator.Current:new PrayerLog(theYear,theMonth,theUserId);
        }

        public IEnumerable<PrayerLog> Find(int theYear, int theUserId)
        {
            return from b in myLogs where b.UserId == theUserId && b.Year == theYear select b;
        }

        public IEnumerable<PrayerLog> Find(int theUserId)
        {
            return from b in myLogs where b.UserId == theUserId orderby b.Year,b.Month select b;
        }

        public void AddOrUpdate(PrayerLog theLog)
        {
            //TODO: Remove this when db comesup
            if (theLog.Id == -1)
            {
                theLog.Id = myLogs.Last().Id + 1;
                theLog.Details.Id = theLog.Id;
                theLog.RecalculateStats();
                // Add the log to db only if it contains atleast one prayer set
                if (theLog.Count > 0)
                    myLogs.Add(theLog);
            }
            else
            {
                // mark as update
                theLog.RecalculateStats();
            }
        }

        public void Delete(PrayerLog theLog)
        {
        }

        public void Save()
        {
        }

        public void Dispose()
        {
        }
    }
}