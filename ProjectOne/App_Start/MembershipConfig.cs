using System;
using WebMatrix.WebData;

namespace ProjectOne
{
    public class MembershipConfig
    {
        public static void RegisterDatabase()
        {
            try
            {
                WebSecurity.InitializeDatabaseConnection("ProjectOneDatabase", "UserProfiles", "Id", "Name", autoCreateTables: true);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("The ASP.NET Simple Membership database could not be initialized. ", ex);
            }
        }
    }
}