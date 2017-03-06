using Application.DbModels;
using Application.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace Application.Controllers
{
    public class MyAccountController : BaseController
    {
        // GET: MyAccount
        public ActionResult Index()
        {
            return View();
        }

        // GET: MyAccount/Login
        public ActionResult Login()
        {
            return View();
        }

        //GET: Accounts/Register
        public ActionResult Register()
        {
            return View();
        }

        //POST: Accounts/Register
        [HttpPost]
        public ActionResult Register(RegisterViewModel viewmodel)
        {
            if (ModelState.IsValid)
            {
                var account = new Account
                {
                    Email = viewmodel.Email,
                    Password = viewmodel.Password,
                };

                var user = new User
                {
                    FirstName = viewmodel.FirstName,
                    LastName = viewmodel.LastName,
                    Account = account,
                };

                Db.Accounts.Add(account);
                Db.Users.Add(user);
                Db.SaveChanges();

                return RedirectToAction("Login");
            }
            else if (Db.Accounts.Any(x => x.Email == viewmodel.Email))
            {
                ViewBag.Message = "Email is already in use.";
                return View();
            }

            ViewBag.Message = null;
            return View();

        }

        //POST: MyAccount/Login
        [HttpPost]
        public ActionResult Login(Account account)
        {
            if (Db.Accounts.Any(db => db.Email == account.Email && db.Password == account.Password))
            {
                FormsAuthentication.SetAuthCookie(account.Email, false);
                return RedirectToAction("Index", "Home"); // change this
            }
            else
            {
                ViewBag.Message = "Login failed. Email and password don't match or credentials don' exist.";
                return View();
            }
        }

        //GET: MyAccount/Logout
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Home");
        }

    }
}