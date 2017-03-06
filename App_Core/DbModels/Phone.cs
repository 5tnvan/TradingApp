using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Application.DbModels
{
    public class Phone : Base
    {        
        public int Number { get; set; }

        //navigation
        [ForeignKey("PhoneId")]
        public virtual ICollection<UserPhone> UserPhones { get; set; }
    }
}