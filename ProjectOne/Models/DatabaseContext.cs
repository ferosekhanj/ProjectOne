using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.Data;
namespace ProjectOne.Models
{
    /// <summary>
    /// The EF databse context
    /// </summary>
    public class DatabaseContext:DbContext
    {
        /// <summary>
        /// Default constructor
        /// </summary>
        public DatabaseContext():base("ProjectOneDatabase")
        {
                
        }

        /// <summary>
        /// The domain model to persistence model conversion
        /// </summary>
        /// <param name="modelBuilder"></param>
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PrayerLogDetail>()
                .HasKey(p => p.PrayerLogId);

            modelBuilder.Entity<PrayerLog>()
                .HasOptional(s => s.Details)
                .WithRequired(p => p.MyLog);
        }

        /// <summary>
        /// The user profiles
        /// </summary>
        public DbSet<UserProfile> UserProfiles { get; set; }

        /// <summary>
        /// The prayer logs
        /// </summary>
        public DbSet<PrayerLog> PrayerLogs { get; set; }

    }
}