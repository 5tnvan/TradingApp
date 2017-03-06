using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Application.DbModels
{
    /// <summary>
    /// Ratings Table
    /// </summary>
    public class Rating : Base
    {
        public int Score { get; set; }
        public string Comment { get; set; }
    }
}