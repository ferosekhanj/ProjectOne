using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

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
        bool myOwnDB;
        DatabaseContext myDB;

        /// <summary>
        /// 
        /// </summary>
        public PrayerLogRepository()
        {
            myDB = new DatabaseContext();
            myOwnDB = true;
        }

        /// <summary>
        /// 
        /// </summary>
        public PrayerLogRepository(DatabaseContext theContext)
        {
            myDB = theContext;
        }
        
        public PrayerLog Find(int theYear, int theMonth, int theUserId)
        {
            IList<PrayerLog> aLogs = (from b in myDB.PrayerLogs.Include(t=>t.Details) where b.UserId == theUserId && b.Year == theYear && b.Month == theMonth select b).ToList<PrayerLog>();
            return (aLogs.Count > 0) ? aLogs[0] : new PrayerLog(theYear, theMonth, theUserId);
        }

        public IEnumerable<PrayerLog> Find(int theYear, int theUserId)
        {
            return from b in myDB.PrayerLogs.Include(t => t.Details) where b.UserId == theUserId && b.Year == theYear select b;
        }

        public IEnumerable<PrayerLog> Find(int theUserId)
        {
            return from b in myDB.PrayerLogs.Include(t => t.Details) where b.UserId == theUserId orderby b.Year, b.Month select b;
        }

        public void AddOrUpdate(PrayerLog theLog)
        {
            theLog.RecalculateStats();

            if (theLog.Id == -1)
            {
                // Add the log to db only if it contains atleast one prayer set
                if (theLog.Count > 0)
                    myDB.PrayerLogs.Add(theLog);
            }
            //else
            //{
            //    // EF does the change tracking for us. The following is not necessary.
            //    //myDB.Entry<PrayerLog>(theLog).State = System.Data.EntityState.
            //}
        }

        public void Delete(PrayerLog theLog)
        {
            throw new NotImplementedException("Delete is not yet implemented!!");
        }

        public void Save()
        {
            myDB.SaveChanges();
        }

        public void Dispose()
        {
            if (myOwnDB && myDB != null)
                myDB.Dispose();
            myDB = null;
        }
    }
}