using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Application.Helpers.Matches;
using System.Web.Mvc;
using Application.DbModels;
using Application.ViewModels;

namespace Application.Controllers
{
    public class MyMatchesController : BaseController
    {
        // GET: MyMatches
        //[Authorize]
        //public ActionResult Index()
        //{
        //    var cyclesOfUsers = Matches.GetCyclesOfUsers();
        //    var cyclesOfBuyRequests = Matches.GetCyclesOfBuyRequests(cyclesOfUsers);
        //    var list = new List<ProductCyclesViewModel>();

        //    foreach (var cycle in cyclesOfBuyRequests)
        //    {
        //        //get product
        //        var productId = cycle.Last().ProductId;
        //        var product = Db.Products.Where(x => x.Id == productId).FirstOrDefault();

        //        //add product to list
        //        if (product != null && !list.Any(x => x.Product == product))
        //        {
        //            var mp = new ProductCyclesViewModel();

        //            mp.Product = product;
        //            mp.PrimaryFile = new ProductFile();
        //            var pf = Db.ProductFiles.Where(x => x.ProductId == product.Id && x.IsPrimary == true).FirstOrDefault();
        //            if (pf != null) mp.PrimaryFile = pf;
        //            mp.Options = new List<List<BuyRequestViewModel>>();

        //            list.Add(mp);
        //        }

        //        //get options
        //        cycle.Reverse();
        //        var option = new List<BuyRequestViewModel>();
        //        foreach (var buyRequest in cycle)
        //        {
        //            var dbBuyRequest = Db.BuyRequests.Where(x => x.Id == buyRequest.Id).FirstOrDefault();
        //            if (dbBuyRequest != null)
        //            {
        //                var brviewmodel = new BuyRequestViewModel();
        //                brviewmodel.BuyRequest = dbBuyRequest;
        //                brviewmodel.PrimaryFile = new ProductFile();
        //                var pf = Db.ProductFiles.Where(x => x.ProductId == dbBuyRequest.ProductId && x.IsPrimary == true).FirstOrDefault();
        //                if (pf != null) brviewmodel.PrimaryFile = pf;

        //                //check if a transaction in progress aldready exist                        
        //                var tbr = Db.TransactionBuyRequests.Where(x => x.BuyRequest.ProductId == brviewmodel.BuyRequest.ProductId && x.Transaction.Status == "Pending").FirstOrDefault();
        //                if (tbr != null && dbBuyRequest.Product.UserId == User.Id) // it is my product and it has a pending transaction
        //                {
        //                    brviewmodel.IsTransactionBuyRequest = true;
        //                    brviewmodel.IsMine = true;
        //                    brviewmodel.TransactionId = tbr.TransactionId;
        //                }
        //                else if (tbr != null && dbBuyRequest.Product.UserId != User.Id) // it is not my product and it has a pending transaction
        //                {
        //                    brviewmodel.IsTransactionBuyRequest = true;
        //                    brviewmodel.IsOther = true;
        //                }

        //                option.Add(brviewmodel);
        //            } 
        //        }                 

        //        //add options to list
        //        var productCycle = list.Where(x => x.Product.Id == productId).FirstOrDefault();
        //        if (productCycle != null)
        //        {
        //            var other = option.Any(x => x.IsOther == true);
        //            var mine = option.Any(x => x.IsMine == true);

        //            if (!other == true) // add option only if it doesn't contain product that already is in transaction pending
        //            {                        
        //                productCycle.Options.Add(option);                        
        //            }

        //            if (mine == true) // if option is mine, save transactionId for message
        //            {
        //                var transactionId = option.Where(x => x.IsMine == true).Select(x => x.TransactionId).FirstOrDefault();
        //                productCycle.TransactionId = transactionId;
        //            }
        //        }
        //    }

        //    // review list
        //    // if a product in a list doesn't have options (due to the fact that all options have pending transactions)
        //    var productsWithNoOptions = list.Where(x => x.Options.Count() == 0 && x.TransactionId == null).ToList();
        //    if (productsWithNoOptions.Count() > 0)
        //    {
        //        foreach (var product in productsWithNoOptions)
        //        {
        //            list.Remove(product);
        //        }                
        //    }

        //    return View(list);
        //}     

    }
}
