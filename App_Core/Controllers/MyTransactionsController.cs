using Application.Controllers;
using Application.DbModels;
using Application.Factories;
using Application.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Application.App_Core.Controllers
{
    public class MyTransactionsController : BaseController
    {
        // GET: MyTransactions
        [Authorize]
        public ActionResult Index()
        {
            //all my transactions to list
            var transactions = Db.TransactionBuyRequests.Where(x => x.GetBuyRequest.UserId == User.Id && (x.CheckoutStatus == "Accepted" || x.CheckoutStatus == "Rejected")).Select(x => x.Transaction).ToList();
            var transactionList = new List<Transaction>();

            //build viewmodellist 
            foreach (var transaction in transactions)
            {
                // add transactionbuyrequests (path) in correct order for current user
                var tbrsInCorrectOrder = new List<TransactionBuyRequest>();                
                var tbrs = transaction.TransactionBuyRequests;
                for (int i = 0; i < tbrs.Count(); i++)
                {
                    if (i == 0)
                    {
                        //add my transaction buy request
                        var myTbr = tbrs.Where(x => x.GetBuyRequest.UserId == User.Id).First(); // First because I know this transaction has it, see first line var transactions =...
                        tbrsInCorrectOrder.Add(myTbr);
                    }
                    else
                    {
                        //add transaction buy request containing prev id as user id
                        var theirTbr = tbrs.Where(x => x.GetBuyRequest.UserId == tbrsInCorrectOrder.Last().GiveBuyRequest.UserId).First(); // First because I know this transaction has it, see first line var transactions =...
                        tbrsInCorrectOrder.Add(theirTbr);
                    }
                }
                transaction.TransactionBuyRequests = tbrsInCorrectOrder;
                transactionList.Add(transaction);
            }

            return View(transactionList);
        }

        //GET: MyTransactions/Details
        [Authorize]
        public ActionResult Details(int id)
        {
            var transaction = Db.Transactions.Where(x => x.Id == id).FirstOrDefault();
            var authorize = false;
            if (transaction != null) authorize = Db.TransactionBuyRequests.Any(x => x.TransactionId == transaction.Id && x.GetBuyRequest.UserId == User.Id);

            if (transaction != null && authorize == true)
            {
                // add transactionbuyrequests (path) in correct order for current user
                var tbrsInCorrectOrder = new List<TransactionBuyRequest>();
                var tbrs = transaction.TransactionBuyRequests;
                for (int i = 0; i < tbrs.Count(); i++)
                {
                    if (i == 0)
                    {
                        //add my transaction buy request
                        var myTbr = tbrs.Where(x => x.GetBuyRequest.UserId == User.Id).First(); // First because I know this transaction has it, see first line var transactions =...
                        tbrsInCorrectOrder.Add(myTbr);
                    }
                    else
                    {
                        //add transaction buy request containing prev id as user id
                        var theirTbr = tbrs.Where(x => x.GetBuyRequest.UserId == tbrsInCorrectOrder.Last().GiveBuyRequest.UserId).First(); // First because I know this transaction has it, see first line var transactions =...
                        tbrsInCorrectOrder.Add(theirTbr);
                    }
                }
                transaction.TransactionBuyRequests = tbrsInCorrectOrder;
                return View(transaction);
            }
            if (transaction == null || authorize == false)
            {
                return Content("Error: Transaction not found");
            }

            return Content("Error: Something went wrong");
        }

        //Ajax: Create transaction or edit transaction
        [Authorize]
        [HttpPost]
        public ActionResult Checkout(List<BuyRequestViewModel> buyRequests)
        {
            //validate given buyrequests
            var validBr = false;
            var transactionExist = false;
            long transactionId = 0;
            var giveId = buyRequests.First().GiveBuyRequestId;
            var getId = buyRequests.First().GetBuyRequestId;
            var dbGiveBr = Db.BuyRequests.Where(x => x.Id == giveId).FirstOrDefault();
            var dbGetBr = Db.BuyRequests.Where(x => x.Id == getId).FirstOrDefault();
            if (dbGiveBr != null && dbGetBr != null) { if (dbGiveBr.Product.UserId == User.Id && dbGetBr.UserId == User.Id) validBr = true; }
                        
            if (validBr == true)
            {                
                //check if cycle is in a transaction
                foreach (var br in buyRequests)
                {
                    var isTransaction = Db.TransactionBuyRequests.Where(x => x.GiveBuyRequestId == br.GiveBuyRequestId && x.GetBuyRequestId == br.GetBuyRequestId && x.Transaction.CheckoutStatus == "Pending").FirstOrDefault();
                    if (isTransaction != null)
                    {
                        //check if it is trully the transaction
                        var products1 = isTransaction.Transaction.TransactionBuyRequests.Select(x => x.GiveBuyRequestId).ToList();
                        var products2 = buyRequests.Select(x => x.GiveBuyRequestId).ToList();
                        var productEqual = new HashSet<long>(products1).SetEquals(products2);

                        if (productEqual == true)
                        {
                            transactionExist = true;
                            transactionId = isTransaction.Transaction.Id;
                            break;
                        }                        
                    }
                }

                if (transactionExist == true)
                {
                    //edit transaction
                    var transaction = Db.Transactions.Where(x => x.Id == transactionId).First(); //first because transaction must exist if tbr exist
                    var myTbr = transaction.TransactionBuyRequests.Where(x => x.GiveBuyRequestId == giveId && x.GetBuyRequestId == getId).FirstOrDefault();
                    if (myTbr != null)
                    {
                        myTbr.CheckoutStatus = "Accepted";
                        Db.SaveChanges();                        
                    }
                    var notAccepted = transaction.TransactionBuyRequests.Where(x => x.TransactionId == transaction.Id).Any(x => x.CheckoutStatus == "Pending" || x.CheckoutStatus == "Rejected");
                    if (notAccepted == false)
                    {
                        transaction.CheckoutStatus = "Accepted";
                        var brs = transaction.TransactionBuyRequests.Select(x => x.GiveBuyRequest).ToList();
                        foreach (var br in brs)
                        {
                            br.IsLive = false;
                            br.Product.IsLive = false;
                            //remove all buy requests of the sold product
                            var z = Db.BuyRequests.Where(x => x.ProductId == br.ProductId);
                            foreach (var x in z)
                            {
                                x.IsLive = false;
                            }

                        }
                        Db.SaveChanges();                        
                    }
                }
                if (transactionExist == false)
                {
                    var p1 = Db.BuyRequests.Where(x => x.Id == giveId).First().ProductId; //first because input has been validated
                    var p2 = Db.BuyRequests.Where(x => x.Id == getId).First().ProductId; //first because input has been validated
                    //check if my tbr will reject existing transaction  
                    var tbrs = Db.TransactionBuyRequests.Where(x => (x.GiveBuyRequest.ProductId == p1 || x.GetBuyRequest.ProductId == p2) && x.CheckoutStatus == "Pending").ToList();
                    foreach (var tbr in tbrs)
                    {
                        tbr.CheckoutStatus = "Rejected";
                        if (tbr.Transaction.CheckoutStatus != "Rejected")
                        {
                            tbr.Transaction.CheckoutStatus = "Rejected";
                            Db.SaveChanges();
                        }
                    }

                    //create transaction
                    var i = 0;
                    var transaction = new Transaction { CheckoutStatus = "Pending" };
                    Db.Transactions.Add(transaction);
                    Db.SaveChanges();
                    transaction.TransactionBuyRequests = new List<TransactionBuyRequest>();
                    foreach (var br in buyRequests)
                    {
                        var tbr = new TransactionBuyRequest {
                            TransactionId = transaction.Id,
                            GiveBuyRequestId = br.GiveBuyRequestId,
                            GetBuyRequestId = br.GetBuyRequestId,
                            CheckoutStatus = "Pending"
                        };
                        if (i == 0) tbr.CheckoutStatus = "Accepted";
                        transaction.TransactionBuyRequests.Add(tbr);
                        i++;
                    }
                    Db.SaveChanges();                    
                }
            }
            return Content("error");
        }
    }
}

