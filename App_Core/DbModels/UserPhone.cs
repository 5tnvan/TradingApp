using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Application.DbModels
{
    /// <summary>
    /// UserFiles Table
    /// </summary>
    public class UserPhone : Base
    {
        public long UserId { get; set; }
        public long PhoneId { get; set; }
        public bool IsPrimary { get; set; }

        // navigation
        public virtual User User { get; set; }
        public virtual Phone Phone { get; set; }
    }
}