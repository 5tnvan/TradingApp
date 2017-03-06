using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Application.DbModels
{
    /// <summary>
    /// Transactions Table
    /// </summary>
    public class Transaction : Base
    {
        public string CheckoutStatus { get; set; }

        [ForeignKey("TransactionId")]
        public virtual ICollection<TransactionBuyRequest> TransactionBuyRequests { get; set; }
    }
}