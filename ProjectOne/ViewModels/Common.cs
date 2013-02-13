using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProjectOne.ViewModels
{
    public class Common
    {
        public static string[] Waqt = new string[] { "Subuh", "Zohar", "Asr", "Magrib", "Isha" };
        public static string[] Weekdays = new string[] { "Sun", "Mon", "Tue", "Wed", "Thu", "Fri", "Sat" };
        public static string[] Month = new string[] { "Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec" };

        public static string CalculateTimezoneFromOffset(double theJavascriptTZOffset)
        {
            // The javascript and .net has the offsets calculated in the opposit direction
            foreach (var v in TimeZoneInfo.GetSystemTimeZones())
            {
                if (Common.IsEqual(v.BaseUtcOffset.TotalMinutes, -theJavascriptTZOffset))
                {
                    return v.Id;
                }
            }
            // If nothing is found return UTC. Let someone correct it later
            return "UTC";
        }

        public static DateTime GetTodayInTimezone(string theTimeZoneId)
        {
            return TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, TimeZoneInfo.FindSystemTimeZoneById(theTimeZoneId));
        }

        private static bool IsEqual(double double1, double double2)
        {
            // Compare the values 
            // The output to the console indicates that the two values are equal 
            return(Math.Abs(double1 - double2) <= Math.Abs(double1 * .000001));
        }
   }
}