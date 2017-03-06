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
    public class MyCartController : BaseController
    {
        // GET: MyCart
        [Authorize]
        public ActionResult Index()
        {
            var cyclesOfUsers = Matches.GetCyclesOfUsers();
            var cyclesOfBuyRequests = Matches.GetCyclesOfBuyRequests(cyclesOfUsers);
            var list = new List<ProductCyclesViewModel>();

            //take option
            foreach (var cycle in cyclesOfBuyRequests)
            {
                //add product to list
                var product = new ProductCyclesViewModel
                {
                    Product = cycle.Last().Product,
                    PrimaryFile = new ProductFile(),
                };
                var pf = product.Product.ProductFiles.Where(x => x.IsPrimary == true).FirstOrDefault();
                if (pf != null) product.PrimaryFile = pf;
                product.Options = new List<List<BuyRequestViewModel>>();
                if(!list.Any(x => x.Product.Id == product.Product.Id)) list.Add(product);

                //get option
                cycle.Reverse();
                var option = new List<BuyRequestViewModel>();
                var i = 0;
                var give = cycle.First();
                var get = cycle.Last();
                var optionIsValid = true;                                
                var myTransactionExist = Db.TransactionBuyRequests.Any(tbr => tbr.CheckoutStatus == "Accepted" && tbr.Transaction.CheckoutStatus != "Rejected" && (tbr.GiveBuyRequest.ProductId == give.ProductId || tbr.GetBuyRequest.ProductId == get.ProductId));

                //save option only if it is not already checked out by me
                if (myTransactionExist == false)
                {
                    foreach (var buyRequest in cycle)
                    {
                        var brviewmodel = new BuyRequestViewModel();
                        brviewmodel.PrimaryFile = new ProductFile();
                        var pf1 = buyRequest.Product.ProductFiles.Where(x => x.IsPrimary == true).FirstOrDefault();
                        if (pf1 != null) brviewmodel.PrimaryFile = pf1;

                        if (i == 0)
                        {                            
                            brviewmodel.GiveBuyRequestId = give.Id;
                            brviewmodel.GiveBuyRequest = give;
                            brviewmodel.GetBuyRequestId = get.Id;
                            brviewmodel.GetBuyRequest = get;
                            option.Add(brviewmodel);
                        }                       
                        if (i > 0) 
                        {
                            brviewmodel.GiveBuyRequestId = buyRequest.Id;
                            brviewmodel.GiveBuyRequest = buyRequest;
                            brviewmodel.GetBuyRequestId = get.Id;
                            brviewmodel.GetBuyRequest = get;

                            //check if br (that is not mine) is checked out by other
                            var checkedout = Db.TransactionBuyRequests.Where(x => (x.GiveBuyRequest.ProductId == buyRequest.ProductId || x.GetBuyRequest.ProductId == get.ProductId) && x.CheckoutStatus == "Accepted" && x.Transaction.CheckoutStatus != "Rejected").FirstOrDefault();
                            if (checkedout == null) option.Add(brviewmodel);
                            if (checkedout != null)
                            {
                                //count                                
                                var products1 = checkedout.Transaction.TransactionBuyRequests.Where(x => x.TransactionId == checkedout.TransactionId).Select(x => x.GiveBuyRequest.ProductId);
                                var products2 = cycle.Select(x => x.ProductId).ToList();
                                var participants1 = checkedout.Transaction.TransactionBuyRequests.Where(x => x.TransactionId == checkedout.TransactionId).Select(x => x.GiveBuyRequest.Product.UserId);
                                var participants2 = cycle.Select(x => x.Product.UserId).ToList();
                                var productEqual = new HashSet<long>(products1).SetEquals(products2);
                                var participantEqual = new HashSet<long>(participants1).SetEquals(participants2);

                                // the participants are the same => products are the same =====> show check mark
                                if (productEqual == true)
                                {                                    
                                    brviewmodel.User = buyRequest.Product.User;
                                    option.Add(brviewmodel);
                                }
                                // the participants are the same => products are NOT the same =====> show without check mark
                                if (participantEqual == true && productEqual == false) option.Add(brviewmodel);

                                // the participants are NOT the same ===> hide 
                                if (participantEqual == false)
                                {
                                    optionIsValid = false;
                                    break;
                                }
                            }                            
                        }
                        get = buyRequest;
                        i++;                                                                        
                    }
                    if (optionIsValid == true)
                    {
                        //add option to product options
                        var pc = list.Where(x => x.Product.Id == product.Product.Id).FirstOrDefault();
                        if (pc != null) pc.Options.Add(option);
                    }                    
                }
            }

            // review list
            // if a product in a list doesn't have options (due to the fact that all options have pending transactions)
            var productsWithNoOptions = list.Where(x => x.Options.Count() == 0).ToList();
            if (productsWithNoOptions.Count() > 0)
            {
                foreach (var product in productsWithNoOptions)
                {
                    list.Remove(product);
                }                
            }

            return View(list);
        }

        public ActionResult Test()
        {
            return View();
        }
    }
}
