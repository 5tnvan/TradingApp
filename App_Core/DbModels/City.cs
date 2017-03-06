using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Application.DbModels
{
    public class City : Base
    {
        /// <summary>
        /// Cities Table
        /// </summary>
        public string Name { get; set; }
        public long CountryId { get; set; }

        //navigation
        public virtual Country Country { get; set; }
    }
}