using Application.DbModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Application.ViewModels
{
    public class UserViewModel
    {
        public long Id { get; set; }

        [Required(ErrorMessage = "First Name is required")]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Last Name is required")]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime Birthday { get; set; }

        public bool Gender { get; set; }

        [Display(Name = "Phone Number")]
        public int? PhoneNumber { get; set; }

        public Account Account { get; set; }
        public ICollection<Address> Addresses { get; set;}      

        //files
        public bool IsPrimaryFileExist { get; set; }
        public UserFile PrimaryFile { get; set; }
        public List<UserFile> UserFiles { get; set; }        
    }
}