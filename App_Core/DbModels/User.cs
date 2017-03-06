using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Application.DbModels
{
    /// <summary>
    /// Users Table
    /// </summary>
    public class User : Base
    {
        [StringLength(255)]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [StringLength(255)]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Column(TypeName = "DateTime2")]
        public DateTime Birthday { get; set; }

        public bool Gender { get; set; }

        [Required]
        public long AccountId { get; set; }

        // navigation
        public virtual Account Account { get; set; }
        [ForeignKey("UserId")]
        public virtual ICollection<Product> Products { get; set; }
        [ForeignKey("UserId")]
        public virtual ICollection<UserFile> UserFiles { get; set; }
        [ForeignKey("UserId")]
        public virtual ICollection<UserAddress> UserAddresses { get; set; }
        [ForeignKey("UserId")]
        public virtual ICollection<UserPhone> UserPhones { get; set; }
    }
}