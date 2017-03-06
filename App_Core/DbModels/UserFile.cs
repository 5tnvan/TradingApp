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
    public class UserFile : Base
    {
        public long UserId { get; set; }
        public long FileId { get; set; }
        public bool IsPrimary { get; set; }

        // navigation
        public virtual User User { get; set; }
        public virtual File File { get; set; }
    }
}