using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Application.DbModels
{
    public class Address : Base
    {
        /// <summary>
        /// Addresses Table
        /// </summary>       

        public string StreetName { get; set; }
        public int StreetNumber { get; set; }
        public long CityId { get; set; }

        //navigation
        public virtual City City { get; set; }
        [ForeignKey("AddressId")]
        public virtual ICollection<UserAddress> UserAddresses { get; set; }
    }
}