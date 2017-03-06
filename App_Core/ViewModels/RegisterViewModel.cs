using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Application.ViewModels
{
    public class RegisterViewModel
    {
        [StringLength(255, ErrorMessage = "Name cannot exceed 255 characters")]
        [Required(ErrorMessage = "First Name is required")]
        [Display(Name = "First Name")]        
        public string FirstName { get; set; }

        [StringLength(255, ErrorMessage = "Name cannot exceed 255 characters")]
        [Required(ErrorMessage = "Last Name is required")]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Email is required")]
        //[Index(IsUnique = true)]
        [StringLength(50)]
        [RegularExpression(@"\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*", ErrorMessage = "Invalid email")]
        //[Remote("IsEmailExist", "MyAccount", null, ErrorMessage = "Email is already in used")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]
        [StringLength(255, MinimumLength = 8, ErrorMessage = "Password must be in range of 8 - 255 characters")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Confirmed password is required")]
        [DataType(DataType.Password)]
        [System.ComponentModel.DataAnnotations.Compare("Password", ErrorMessage = "Password does not match")]
        public string ConfirmPassword { get; set; }
    }
}