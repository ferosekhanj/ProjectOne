using ProjectOne.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Web;
using WebMatrix.WebData;

namespace ProjectOne
{
    public class MembershipConfig
    {
        public static void RegisterDatabase()
        {
            Database.SetInitializer<DatabaseContext>(null);

            try
            {
                using (var context = new DatabaseContext())
                {
                    if (!context.Database.Exists())
                    {
                        // Create the SimpleMembership database without Entity Framework migration schema
                        ((IObjectContextAdapter)context).ObjectContext.CreateDatabase();
                    }
                }

                WebSecurity.InitializeDatabaseConnection("ProjectOneDatabase", "UserProfiles", "Id", "Name", autoCreateTables: true);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("The ASP.NET Simple Membership database could not be initialized. ", ex);
            }
        }
    }
}