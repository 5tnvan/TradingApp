using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Application.DbModels
{
    /// <summary>
    /// BuyRequests Table
    /// </summary>
    public class BuyRequest : Base
    {
        public long UserId { get; set; }
        public long ProductId { get; set; }
        public bool IsLive { get; set; }

        // navigation
        public virtual User User { get; set; }
        public virtual Product Product { get; set; }
    }
}