using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.Data;
namespace ProjectOne.Models
{
    public class DatabaseContext:DbContext
    {
        public DatabaseContext():base("ProjectOneDatabase")
        {
                
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PrayerLogDetail>()
                .HasKey(p => p.PrayerLogId);

            modelBuilder.Entity<PrayerLog>()
                .HasOptional(s => s.Details)
                .WithRequired(p => p.MyLog);
        }

        public DbSet<UserProfile> UserProfiles { get; set; }
        public DbSet<PrayerLog> PrayerLogs { get; set; }

    }
}