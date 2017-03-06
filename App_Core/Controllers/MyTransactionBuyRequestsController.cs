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
    public class MyTransactionBuyRequestsController : BaseController
    {
        //GET: MyTransactionBuyRequests/Details
        [Authorize]
        public ActionResult Details(int id)
        {
            var tbr = Db.TransactionBuyRequests.Where(x => x.Id == id).FirstOrDefault();

            if (tbr != null && tbr.GiveBuyRequest.Product.UserId == User.Id)
            {
                return View(tbr);
            }
            if (tbr == null)
            {
                return Content("Error: Handover not found");
            }
            return Content("Error: Something went wrong");
        }

        // GET: MyTransactionBuyRequests/Handover
        [Authorize]
        public ActionResult Handover(int id)
        {
            var tbr = Db.TransactionBuyRequests.Where(x => x.Id == id).FirstOrDefault();            

            if (tbr != null && tbr.GiveBuyRequest.Product.UserId == User.Id)
            {
                var list = new List<SelectListItem> {
                    new SelectListItem { Value = "1", Text = "In person"},
                    new SelectListItem { Value = "2", Text = "Shipping"},
                    new SelectListItem { Value = "3", Text = "Electronically"},
                };

                var ad = tbr.GiveBuyRequest.User.UserAddresses.FirstOrDefault();

                var viewmodel = new HandoverViewModel() {
                    TransactionBuyRequestId = tbr.Id,
                    TransactionBuyRequest = tbr,
                    HandoverMethodList = list.AsEnumerable(),
                    ShippingName = tbr.GiveBuyRequest.User.FirstName + " " + tbr.GiveBuyRequest.User.LastName,
                };

                if (ad != null) viewmodel.ShippingAddress = ad.Address.StreetName + ad.Address.StreetNumber + ", " + ad.Address.City.Name + "," + ad.Address.City.Country.Name;

                return View(viewmodel);                    
            }
            if (tbr == null)
            {
                return Content("Error: Handover not found");
            }
            return Content("Error: Something went wrong");
        }

        // POST: MyTransactionBuyRequests/Handover
        [Authorize]
        [HttpPost]
        public ActionResult Handover(HandoverViewModel hvm)
        {
            var dbTbr = Db.TransactionBuyRequests.Where(x => x.Id == hvm.TransactionBuyRequestId).FirstOrDefault();
            if (ModelState.IsValid && dbTbr != null)
            {                 
                var handover = new Handover();
                handover.HandoverMethodId = hvm.HandoverMethodId;
                handover.ShippingAddress = hvm.ShippingAddress;
                handover.ShippingName = hvm.ShippingName;
                handover.Note = hvm.Note;
                Db.Handovers.Add(handover);
                dbTbr.HandoverId = handover.Id;
                Db.SaveChanges();

                return RedirectToAction("Details", "MyTransactions", new { id = dbTbr.Transaction.Id });
            }
            if (!ModelState.IsValid && dbTbr != null)
            {
                var list = new List<SelectListItem> {
                    new SelectListItem { Value = "1", Text = "In person"},
                    new SelectListItem { Value = "2", Text = "Shipping"},
                    new SelectListItem { Value = "3", Text = "Electronically"},
                };
                hvm.TransactionBuyRequest = dbTbr;
                hvm.HandoverMethodList = list.AsEnumerable();
                return View(hvm);
            }
            return Content("Something went wrong");
        }

        //GET: MyTransactionBuyRequests/Rate
        //Ajax
        [Authorize]
        [HttpPost]
        public ActionResult Rate(string transactionbrId, string scorebr)
        {
            if (transactionbrId != null && scorebr != null)
            {
                var id = Int32.Parse(transactionbrId);
                var score = Int32.Parse(scorebr);
                var dbtbr = Db.TransactionBuyRequests.Where(x => x.Id == id).FirstOrDefault();

                if (dbtbr != null && dbtbr.RatingId == null)
                {
                    var rating = new Rating { Score = score };
                    Db.Ratings.Add(rating);
                    Db.SaveChanges();
                    dbtbr.RatingId = rating.Id;
                    Db.SaveChanges();
                    return Content("Ok");
                }
                if (dbtbr != null && dbtbr.RatingId != null)
                {
                    dbtbr.Rating.Score = score;
                    Db.SaveChanges();
                    return Content("Ok");
                }
            }           
            
            return Content("Error");
        }
    }
}

