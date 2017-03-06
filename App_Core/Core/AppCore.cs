using Application.Database.Contexts;
using Application.DbModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Application.Core
{
    public class AppCore
    {
        /// <summary>
        /// Return Website Database Context
        /// </summary>
        public static ApplicationDbContext Db
        {
            get
            {
                // HttpContext.Current.Items - Lifetime of Request
                var db = HttpContext.Current.Items["ApplicationDbContext"] as ApplicationDbContext;
                if (db == null)
                {
                    db = new ApplicationDbContext();
                    HttpContext.Current.Items["ApplicationDbContext"] = db;
                }

                return db;
            }
        }

        /// <summary>
        /// Returns current Account
        /// </summary>
        public static Account Account
        {
            get
            {
                if (HttpContext.Current.User.Identity.IsAuthenticated)
                {
                    var email = HttpContext.Current.User.Identity.Name;
                    return Db.Accounts.Where(x => x.Email == email).FirstOrDefault();
                }

                return null;
            }
        }

        /// <summary>
        /// Returns the current User
        /// </summary>
        public static User User
        {
            get
            {
                if (Account != null)
                {
                    return Db.Users.Where(x => x.AccountId == Account.Id).FirstOrDefault();
                }

                return null;
            }
        }

        /// <summary>
        /// Returns the current User
        /// </summary>
        public static bool IsAuthenticated
        {
            get
            {
                return HttpContext.Current.User.Identity.IsAuthenticated;
            }
        }
    }
}