using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Application.DbModels
{
    public class FileExtention : Base
    {
        /// <summary>
        /// FileExtentions Table
        /// </summary>

        [Display(Name ="Extention Name")]
        public string Name { get; set; }

        [Display(Name = "Extention Type")]
        public string Type { get; set; }

    }
}