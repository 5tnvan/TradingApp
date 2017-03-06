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
    public class MyUsersController : BaseController
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
            var viewmodel = new UserViewModel();
            if (user != null && user.Id == User.Id)
            {
                viewmodel = UserFactory.GetUser(user);
                return View(viewmodel);
            }
            if (user == null)
            {
                return Content("Error: User not found");
            }
            return Content("Error: Something went wrong");            
        }

        // GET: MyAccountUsers/Edit
        [Authorize]
        public ActionResult Edit(int id)
        {
            var user = Db.Users.Where(x => x.Id == id).FirstOrDefault();
            var viewmodel = new UserViewModel();
            if (user != null && user.Id == User.Id)
            {
                viewmodel = UserFactory.GetUser(user);
                return View(viewmodel);
            }
            if (user == null)
            {
                return Content("Error: User not found");
            }
            return Content("Error: Something went wrong");
        }
        //POST: MyUsers/Edit
        [HttpPost]
        [Authorize]
        public ActionResult Edit(int id, UserViewModel uvm)
        {
            var dbUser = Db.Users.Where(x => x.Id == id).FirstOrDefault();

            if (ModelState.IsValid && dbUser != null && dbUser.Id == User.Id)
            {
                dbUser.FirstName = uvm.FirstName;
                dbUser.LastName = uvm.LastName;
                dbUser.Birthday = uvm.Birthday;
                dbUser.Gender = uvm.Gender;
                dbUser.UserPhones.Clear();
                Db.SaveChanges();

                //phone
                var existingPhone = Db.Phones.Where(x => x.Number == uvm.PhoneNumber).FirstOrDefault();
                if (existingPhone != null && uvm.PhoneNumber != null)
                {
                    var up = new UserPhone {
                        UserId = User.Id,
                        PhoneId = existingPhone.Id
                    };
                    dbUser.UserPhones.Add(up);
                    Db.SaveChanges();
                }
                if (existingPhone == null && uvm.PhoneNumber != null)
                {
                    var p = new Phone();
                    p.Number = uvm.PhoneNumber ?? default(int);
                    Db.Phones.Add(p);
                    Db.SaveChanges();

                    var up = new UserPhone {
                        UserId = User.Id,
                        PhoneId = p.Id
                    };
                    dbUser.UserPhones.Add(up);
                    Db.SaveChanges();
                }
                
                return RedirectToAction("Details", new { id = id });
            }            

            return View(uvm);
        }

    }
}