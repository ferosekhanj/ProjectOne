using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProjectOne.Models
{
    public interface IUserProfileRepository : IDisposable
    {
        UserProfile UserProfile { get; set; }
        void Save();
    }

    /// <summary>
    /// Finds a particular user data stored in the app db.
    /// </summary>
    public class UserProfileRepository : IUserProfileRepository
    {
        bool myOwnDB;
        DatabaseContext myDB;
        int myUserId;

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="theUserId"></param>
        public UserProfileRepository(int theUserId)
        {
            myDB = new DatabaseContext();
            myOwnDB = true;
            myUserId = theUserId;
        }

        /// <summary>
        /// Construct using an external DbContext
        /// </summary>
        /// <param name="theUserId"></param>
        /// <param name="theDB"></param>
        public UserProfileRepository(int theUserId, DatabaseContext theDB)
        {
            myDB = theDB;
            myUserId = theUserId;
        }

        UserProfile myUserProfile;
        public UserProfile UserProfile 
        {
            get
            {
                if (myUserProfile == null)
                {
                    myUserProfile = myDB.UserProfiles.Find(myUserId);
                }

                return myUserProfile;
            }
            set
            {
                myUserProfile = value;
            }
        }

        /// <summary>
        /// Save the changes to db
        /// </summary>
        public void Save()
        {
            myDB.SaveChanges();
        }

        /// <summary>
        /// Dispose if we own the db
        /// </summary>
        public void Dispose()
        {
            if(myOwnDB && myDB !=null)
                myDB.Dispose();
            myDB = null;
        }

    }
}