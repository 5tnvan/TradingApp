using Application.Core;
using Application.DbModels;
using Application.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Application.Factories
{
    static class UserFactory
    {
        public static UserViewModel GetUser(User user)
        {
            var uvm = new UserViewModel();
            var dbUserFiles = AppCore.Db.UserFiles.Where(x => x.User.Id == user.Id).ToList();

            //default values
            uvm.Id = user.Id;
            uvm.FirstName = user.FirstName;
            uvm.LastName = user.LastName;
            uvm.Birthday = user.Birthday;
            uvm.Gender = user.Gender;
            uvm.PhoneNumber = null;
            uvm.Addresses = user.UserAddresses.Select(x => x.Address).ToList();
            uvm.Account = user.Account; // possibly not needed

            uvm.IsPrimaryFileExist = false; //false is default
            uvm.PrimaryFile = new UserFile();
            uvm.UserFiles = new List<UserFile>();

            //override if phone exist
            if (user.UserPhones.Count() > 0)
            {
                var phone = user.UserPhones.Select(x => x.Phone).First();
                uvm.PhoneNumber = phone.Number;
            }

            //override if files exist
            if (dbUserFiles != null)
            {
                var pf = dbUserFiles.Where(x => x.IsPrimary == true).FirstOrDefault();
                if (pf != null)
                {
                    uvm.IsPrimaryFileExist = true;
                    uvm.PrimaryFile = pf;
                }
                uvm.UserFiles = dbUserFiles;
            }            

            return uvm;
        }
    }
}