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

    public class UserProfileRepository : IUserProfileRepository
    {
        bool myOwnDB;
        DatabaseContext myDB;
        int myUserId;

        public UserProfileRepository(int theUserId)
        {
            myDB = new DatabaseContext();
            myOwnDB = true;
            myUserId = theUserId;
        }

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

        public void Save()
        {
            myDB.SaveChanges();
        }

        public void Dispose()
        {
            if(myOwnDB && myDB !=null)
                myDB.Dispose();
            myDB = null;
        }

    }
}