using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Application.DbModels
{
    public class Handover : Base
    {        
        public string ShippingAddress { get; set; }
        public string ShippingName { get; set; }
        public string Note { get; set; }
        public long HandoverMethodId { get; set; }

        public virtual HandoverMethod HandoverMethod { get; set; }
    }
}