using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Application.DbModels
{
    public class SubCategory : Base
    {
        /// <summary>
        /// SubCategories Table
        /// </summary>
        public string Name { get; set; }
        public long CategoryId { get; set; }

        //navigation
        public virtual Category Category { get; set; }
    }
}