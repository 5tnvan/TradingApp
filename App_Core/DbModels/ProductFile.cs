using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Application.DbModels
{
    /// <summary>
    /// ProductFiles Table
    /// </summary>
    public class ProductFile : Base
    {
        public long ProductId { get; set; }
        public long FileId { get; set; }
        public bool IsPrimary { get; set; }

        // navigation
        public virtual Product Product { get; set; }
        public virtual File File { get; set; }
    }
}