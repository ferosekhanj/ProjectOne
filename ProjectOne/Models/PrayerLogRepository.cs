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
        /// Ctor
        /// </summary>
        public PrayerLogRepository()
        {
            myDB = new DatabaseContext();
            myOwnDB = true;
        }

        /// <summary>
        /// Construct the repository from an external database context. This wont be disposed by this class
        /// </summary>
        public PrayerLogRepository(DatabaseContext theContext)
        {
            myDB = theContext;
        }
        
        /// <summary>
        /// Find a log for a particular day
        /// </summary>
        /// <param name="theYear"></param>
        /// <param name="theMonth"></param>
        /// <param name="theUserId"></param>
        /// <returns></returns>
        public PrayerLog Find(int theYear, int theMonth, int theUserId)
        {
            IList<PrayerLog> aLogs = (from b in myDB.PrayerLogs.Include(t=>t.Details) where b.UserId == theUserId && b.Year == theYear && b.Month == theMonth select b).ToList<PrayerLog>();
            return (aLogs.Count > 0) ? aLogs[0] : new PrayerLog(theYear, theMonth, theUserId);
        }

        /// <summary>
        /// Find all the logs for a year
        /// </summary>
        /// <param name="theYear"></param>
        /// <param name="theUserId"></param>
        /// <returns></returns>
        public IEnumerable<PrayerLog> Find(int theYear, int theUserId)
        {
            return from b in myDB.PrayerLogs.Include(t => t.Details) where b.UserId == theUserId && b.Year == theYear select b;
        }

        /// <summary>
        /// Find all the logs for a user and sort them by year , month
        /// </summary>
        /// <param name="theUserId"></param>
        /// <returns></returns>
        public IEnumerable<PrayerLog> Find(int theUserId)
        {
            return from b in myDB.PrayerLogs.Include(t => t.Details) where b.UserId == theUserId orderby b.Year, b.Month select b;
        }

        /// <summary>
        /// Add or update a log
        /// </summary>
        /// <param name="theLog"></param>
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

        /// <summary>
        /// Save the changes. 
        /// </summary>
        public void Save()
        {
            //TODO:This is against the UnitOfWork Pattern. Needs refactoring
            myDB.SaveChanges();
        }

        /// <summary>
        /// Dispose if we own the db
        /// </summary>
        public void Dispose()
        {
            if (myOwnDB && myDB != null)
                myDB.Dispose();
            myDB = null;
        }
    }
}