using Application.Core;
using Application.Database.Contexts;
using Application.DbModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Application.Controllers
{
    public class BaseController : Controller
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