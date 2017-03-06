using Application.DbModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Application.Controllers
{
    public class BuyRequestsController : BaseController
    {
        // GET: BuyRequest
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Create(int id)
        {
            var br = new BuyRequest
            {
                UserId = User.Id,
                ProductId = id,
                IsLive = true
            };

            var dbBuyRequest = Db.BuyRequests.Where(z => z.UserId == br.UserId && z.ProductId == br.ProductId).FirstOrDefault();
            var dbProduct = Db.Products.Where(x => x.Id == id).FirstOrDefault();

            if (dbBuyRequest != null && dbBuyRequest.Product.UserId != User.Id && dbBuyRequest.IsLive == false)
            {
                dbBuyRequest.IsLive = true;
                Db.SaveChanges();
                return RedirectToAction("Details", "Products", new { id = id });
            }
            if (dbBuyRequest == null && dbProduct != null && dbProduct.UserId != User.Id)
            {
                Db.BuyRequests.Add(br);
                Db.SaveChanges();
                return RedirectToAction("Details", "Products", new { id = id });
            }

            return Content("Something went wrong");            
        }

        public ActionResult Delete(int id)
        {
            var dbBuyRequest = Db.BuyRequests.Where(x => x.UserId == User.Id && x.ProductId == id).FirstOrDefault();            
            if (dbBuyRequest != null)
            {
                var transactionExist = Db.TransactionBuyRequests.Any(x => x.GiveBuyRequestId == dbBuyRequest.Id && x.Transaction.CheckoutStatus != "Rejected");
                if (transactionExist == false)
                {
                    dbBuyRequest.IsLive = false;
                    Db.SaveChanges();
                    return RedirectToAction("Details", "Products", new { id = id });
                }
                if (transactionExist == true)
                {
                    return Content("Error: You or someone has a pending transaction involving this buy request. Resolve by rejecting your transaction or checking out one of the product in your shopping cart.");
                }
            }
            return Content("Error: Something went wrong");            
        }
    }
}