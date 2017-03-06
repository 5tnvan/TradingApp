using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Application.DbModels
{
    /// <summary>
    /// UserAddresses Table
    /// </summary>
    public class UserAddress : Base
    {
        
        public long UserId { get; set; }
        public long AddressId { get; set; }
        public bool IsPrimary { get; set; }

        // navigation        
        public virtual User User { get; set; }        
        public virtual Address Address { get; set; }
    }
}