using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Application.DbModels
{
    /// <summary>
    /// TransactionBuyRequests Table
    /// </summary>
    public class TransactionBuyRequest : Base
    {
        public long TransactionId { get; set; }
        public long GiveBuyRequestId { get; set; }
        public long GetBuyRequestId { get; set; }
        public long? RatingId { get; set; }
        public long? HandoverId { get; set; }
        public string CheckoutStatus { get; set; }
        public string Note { get; set; }

        //navigation
        public virtual Handover Handover { get; set; }
        public virtual Rating Rating { get; set; }
        public virtual Transaction Transaction { get; set; }
        [ForeignKey("GiveBuyRequestId")]
        public virtual BuyRequest GiveBuyRequest { get; set; }
        [ForeignKey("GetBuyRequestId")]
        public virtual BuyRequest GetBuyRequest { get; set; }
    }
}