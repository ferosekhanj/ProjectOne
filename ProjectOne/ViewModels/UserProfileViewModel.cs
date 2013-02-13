using ProjectOne.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using WebMatrix.WebData;

namespace ProjectOne.ViewModels
{
    public class UserProfileViewModel
    {
        [DataType(DataType.Password)]
        [Display(Name = "Current password")]
        public string OldPassword { get; set; }

        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "New password")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm new password")]
        [Compare("NewPassword", ErrorMessage = "The new password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        [Display(Name = "Email")]
        public string Email { get; set; }

        [Display(Name = "Timezone")]
        public string Timezone { get; set; }

        /// <summary>
        /// The repository to use for saving and loading data.
        /// </summary>
        public IUserProfileRepository Repository { get; set; }

        public UserProfileViewModel()
        {

        }

        public void Load()
        {
            UserProfile aProfile =  Repository.UserProfile;
            Email       = aProfile.Email;
            Timezone    = aProfile.Timezone;
        }

        public void Save()
        {
            UserProfile aProfile =  Repository.UserProfile;
            aProfile.Email = Email;
            aProfile.Timezone = Timezone;
            Repository.Save();
        }
    }
}