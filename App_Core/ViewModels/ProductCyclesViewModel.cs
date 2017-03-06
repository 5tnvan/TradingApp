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
    public class ProductCyclesViewModel
    {
        public Product Product { get; set; } //my product involved
        public List<List<BuyRequestViewModel>> Options { get; set; } // barter options
        public ProductFile PrimaryFile { get; set; }
    }

    public class BuyRequestViewModel
    {
        public long GiveBuyRequestId { get; set; }
        public long GetBuyRequestId { get; set; }

        public ProductFile PrimaryFile { get; set; }
        public BuyRequest GiveBuyRequest { get; set; }
        public BuyRequest GetBuyRequest { get; set; }
        public User User { get; set; }
    }

    public class BuyRequestAjaxViewModel
    {
        public long GiveBuyRequestId { get; set; }
        public long GetBuyRequestId { get; set; }
    }    
}