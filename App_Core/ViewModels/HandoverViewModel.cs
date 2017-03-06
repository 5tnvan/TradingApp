using Application.DbModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Application.ViewModels
{
    public class HandoverViewModel
    {
        public long TransactionBuyRequestId { get; set; }

        [Required(ErrorMessage = "Handover Method is required")]
        [Display(Name = "Handover Method")]
        public long HandoverMethodId { get; set; }

        public IEnumerable<SelectListItem> HandoverMethodList { get; set; }

        [StringLength(200, ErrorMessage = "Note cannot exceed 200 characters")]
        public string Note { get; set; }

        [MustBeTrue(ErrorMessage = "You must confirm the handover")]
        public bool Agree { get; set; }

        public string ShippingAddress { get; set; }
        public string ShippingName { get; set; }
        
        public TransactionBuyRequest TransactionBuyRequest { get; set; }
        public HandoverMethod HandoverMethod { get; set; }
    }

    /// <summary>
    /// Validation attribute that demands that a boolean value must be true.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
    public class MustBeTrueAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            return value != null && value is bool && (bool)value;
        }
    }
}