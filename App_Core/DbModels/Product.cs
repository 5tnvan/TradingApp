using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Application.DbModels
{
    /// <summary>
    /// Products Table
    /// </summary>
    public class Product : Base
    {
        [Required]
        [StringLength(80)]
        public string Name { get; set; }
        public string Description { get; set; }
        public long SubCategoryId { get; set; }
        public long UserId { get; set; }        
        public bool IsLive { get; set; }

        // navigation
        [ForeignKey("SubCategoryId")]
        public virtual SubCategory SubCategory { get; set; }
        public virtual User User { get; set; }
        [ForeignKey("ProductId")]
        public virtual ICollection<ProductFile> ProductFiles { get; set; }
    }
}