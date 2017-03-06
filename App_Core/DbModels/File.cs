using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Application.DbModels
{
    public class File : Base
    {
        /// <summary>
        /// Files Table
        /// </summary>
       
        [Display(Name ="File Name")]
        public string FileName { get; set; }
        public long FileExtentionId { get; set; }

        //navigation
        public virtual FileExtention FileExtention { get; set; }
        [ForeignKey("FileId")]
        public virtual ICollection<UserFile> UserFiles { get; set; }
        [ForeignKey("FileId")]
        public virtual ICollection<ProductFile> ProductFiles { get; set; }
    }
}