using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProjectOne.Models
{
    /// <summary>
    /// This class represents the user of the website.
    /// </summary>
    public class UserProfile
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public bool Active { get; set; }
        public string Timezone { get; set; }
    }
}