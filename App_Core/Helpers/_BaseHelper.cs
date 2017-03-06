using Application.Core;
using Application.Database.Contexts;
using Application.DbModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Application.Helpers
{
    public class BaseHelper
    {
        public ApplicationDbContext Db
        {
            get
            {
                return AppCore.Db;
            }
        }

        public User User
        {
            get
            {
                return AppCore.User;
            }
        }
    }
}