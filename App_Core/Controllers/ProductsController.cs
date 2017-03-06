using Application.DbModels;
using Application.Factories;
using Application.Helpers.Matches;
using Application.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Application.Controllers
{
    public class ProductsController : BaseController
    {
        // GET: Products
        [Authorize]
        public ActionResult Index(string filter, string id)
        {
            var viewModel = new List<ProductViewModel>();
            var productsDb = Db.Products.Where(x => x.UserId != User.Id && x.IsLive == true);

            //if (id == "wanted")
            //{
            //    var wanted = Db.BuyRequests.Where(x => x.UserId == User.Id && x.IsLive == true).Select(x => x.Product.Id).ToList();
            //    var list = productsDb.Where(x => wanted.Contains(x.Id) && x.IsLive == true).ToList();
            //    list.ForEach(p => viewModel.Add(ProductFactory.GetProduct(p)));
            //}
            //else
            //{
                         
            //}

            productsDb.ToList().ForEach(p => viewModel.Add(ProductFactory.GetProduct(p)));
            return View(viewModel);
        }

        // GET: Products/Details/5
        public ActionResult Details(int id)
        {
            var viewmodel = new ProductViewModel();
            var productDb = Db.Products.Where(x => x.Id == id).FirstOrDefault();

            if (productDb != null && productDb.UserId != User.Id)
            {
                viewmodel = ProductFactory.GetProduct(productDb);
                return View(viewmodel);
            }
            if (productDb == null || productDb.UserId == User.Id)
            {
                return Content("Error: Product not found");
            }

            return Content("Something went wrong");
        }
    }
}
