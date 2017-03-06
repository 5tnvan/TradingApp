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
    public class AddressViewModel
    {
        public long Id { get; set; }

        [Display(Name = "Street Name")]
        [Required(ErrorMessage = "Street Name is required")]
        public string StreetName { get; set; }

        [Display(Name = "Street Number")]
        [Required(ErrorMessage = "Street Number is required")]
        public int StreetNumber { get; set; }

        [Display(Name = "City")]
        [Required(ErrorMessage = "City is required")]
        public long SelectedCityId { get; set; }

        [Display(Name = "Country")]
        [Required(ErrorMessage = "Country is required")]
        public long SelectedCountryId { get; set; }

        public IEnumerable<SelectListItem> Cities { get; set; }
        public IEnumerable<SelectListItem> Countries { get; set; }
    }
}