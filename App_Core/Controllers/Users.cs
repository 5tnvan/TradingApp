using Application.DbModels;
using Application.Factories;
using Application.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace Application.Controllers
{
    public class UsersController : BaseController
    {
        // GET: MyUsers
        public ActionResult Index()
        {
            return View();
        }

        // GET: MyUsers/Details
        [Authorize]
        public ActionResult Details(int id)
        {
            var user = Db.Users.Where(x => x.Id == id).FirstOrDefault();
            if (user != null && user.Id != User.Id)
            {                
                return View(user);
            }
            if (user == null)
            {
                return Content("Error: User not found");
            }
            return Content("Error: Something went wrong");            
        }
    }
}