using Application.Core;
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
    public class ProductViewModel
    {
        public long Id { get; set; }

        [Required(ErrorMessage = "Name is required")]
        [StringLength(80, ErrorMessage = "Name cannot exceed 80 characters")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Description is required")]
        public string Description { get; set; }

        // user (UserViewModel has images of user)
        public UserViewModel UserViewModel { get; set; }

        // category - subcategories
        [Display(Name = "Category")]
        public long? SelectedCategoryId { get; set; }

        [Required(ErrorMessage = "Categorization is required")]
        [Display(Name = "SubCategory")]
        public long? SelectedSubCategoryId { get; set; }

        public IEnumerable<SelectListItem> CategoryList { get; set; }
        public IEnumerable<SelectListItem> SubCategoryList { get; set; }
        public SubCategory SubCategory { get; set; }

        // files        
        public bool IsPrimaryFileExist { get; set; }
        public ProductFile PrimaryFile { get; set; }
        public List<ProductFile> ProductFiles { get; set; }

        // number of interests
        public int InterestCount { get; set; }

        // users interested in item
        public List<UserViewModel> UserInterested { get; set; }

        // product is marked as wanted
        public bool Wanted { get; set; }

        //is live
        public bool IsLive { get; set; }
    }
}