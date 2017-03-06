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
    static class ProductFactory
    {
        public static ProductViewModel GetProduct(Product product)
        {
            var pvm = new ProductViewModel();
            var dbProductFiles = AppCore.Db.ProductFiles.Where(x => x.ProductId == product.Id).ToList();
            var dbUserInterested = AppCore.Db.BuyRequests.Where(x => x.ProductId == product.Id && x.IsLive == true).Select(x => x.User).ToList();
            var userInterested = new List<UserViewModel>();
            var dbCategoryList = AppCore.Db.Categories.Select(c => new SelectListItem
            {
                Value = c.Id.ToString(),
                Text = c.Name,
                Selected = c.Id.Equals(product.SubCategory.CategoryId)
            });
            var dbSubCategoryList = AppCore.Db.SubCategories.Select(c => new SelectListItem
            {
                Value = c.Id.ToString(),
                Text = c.Name,
                Selected = c.Id.Equals(product.SubCategoryId)
            });

            pvm.Id = product.Id;
            pvm.Name = product.Name;
            pvm.Description = product.Description;            
            pvm.SubCategory = product.SubCategory;
            pvm.UserViewModel = UserFactory.GetUser(product.User);
            pvm.Wanted = AppCore.Db.BuyRequests.Any(x => x.UserId == AppCore.User.Id && x.ProductId == product.Id && x.IsLive == true);
            pvm.UserInterested = userInterested;
            pvm.InterestCount = dbUserInterested.Count();
            pvm.IsPrimaryFileExist = false;
            pvm.PrimaryFile = new ProductFile();
            pvm.ProductFiles = new List<ProductFile>();
            pvm.CategoryList = dbCategoryList.AsEnumerable();
            pvm.SubCategoryList = dbSubCategoryList.AsEnumerable();
            pvm.IsLive = product.IsLive;


            if (dbProductFiles != null)
            {
                var pf = dbProductFiles.Where(x => x.IsPrimary == true).FirstOrDefault();
                if (pf != null)
                {
                    pvm.IsPrimaryFileExist = true;
                    pvm.PrimaryFile = pf;
                }
                pvm.ProductFiles = dbProductFiles;                
            }

            foreach (var user in dbUserInterested)
            {
                var uvm = UserFactory.GetUser(user);
                userInterested.Add(uvm);
            }

            return pvm;
        }
    }
}