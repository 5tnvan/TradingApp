using Application.DbModels;
using Application.Factories;
using Application.ViewModels;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;

namespace Application.Controllers
{
    public class MyProductsController : BaseController
    {
        // GET: MyProducts
        [Authorize]
        public ActionResult Index()
        {
            var viewModel = new List<ProductViewModel>();
            var productsDb = Db.Products.Where(x => x.UserId == User.Id && x.IsLive == true).ToList();
            int interestCount = 0;

            if (productsDb.Count() > 0)
            {
                productsDb.ForEach(p => viewModel.Add(ProductFactory.GetProduct(p)));

                foreach (var prod in viewModel)
                {
                    interestCount += prod.InterestCount;
                }
            }

            ViewBag.InterestCount = interestCount;

            return View(viewModel);
        }

        // GET: MyProducts/Details/5
        [Authorize]
        public ActionResult Details(int id)
        {
            var viewmodel = new ProductViewModel();
            var productDb = Db.Products.Where(x => x.Id == id).FirstOrDefault();

            if (productDb != null && productDb.UserId == User.Id)
            {
                viewmodel = ProductFactory.GetProduct(productDb);
                return View(viewmodel);
            }

            if (productDb == null || productDb.UserId != User.Id)
            {
                return Content("Error: Product not found");
            }

            return Content("Error: Something went wrong");
        }

        // GET: MyProducts/Create
        [Authorize]
        public ActionResult Create()
        {
            var query = Db.Categories.Select(c => new SelectListItem
            {
                Value = c.Id.ToString(),
                Text = c.Name,
                Selected = c.Id.Equals(1)
            });

            var viewmodel = new ProductViewModel
            {
                CategoryList = query.AsEnumerable(),
            };

            return View(viewmodel);
        }

        // POST: MyProducts/Create
        [HttpPost]
        [Authorize]
        public ActionResult Create(ProductViewModel pvm, IEnumerable<HttpPostedFileBase> files)
        {
            if (ModelState.IsValid)
            {
                //save product
                var product = new Product();
                product.Name = pvm.Name;
                product.Description = pvm.Description;
                product.SubCategoryId = pvm.SelectedSubCategoryId ?? default(long);
                product.UserId = User.Id;
                product.IsLive = true;

                Db.Products.Add(product);
                Db.SaveChanges();

                var z = 0;
                pvm.ProductFiles = new List<ProductFile>();

                foreach (var file in files)
                {                    
                    if (file != null && file.ContentLength > 0)
                    {   
                        var ext = Path.GetExtension(file.FileName).ToLower().Substring(1);

                        //save image in filesystem
                        var path = Path.Combine(Server.MapPath("~/Files/Images/ProductImages"),
                                                Path.GetFileName(product.Id.ToString() + "_" + z + "." + ext));

                        var thumbnailPath = Path.Combine(Server.MapPath("~/Files/Images/ProductImages"),
                                                Path.GetFileName(product.Id.ToString() + "_" + z + "_small." + ext));

                        file.SaveAs(path);

                        //save thumbnail if first
                        //if (z == 0)
                        
                        var thumbnail = new WebImage(path);
                        var width = thumbnail.Width;
                        var height = thumbnail.Height;

                        if (width > height)
                        {
                            var leftRightCrop = (width - height) / 2;
                            thumbnail.Crop(0, leftRightCrop, 0, leftRightCrop);
                        }
                        else if (height > width)
                        {
                            var topBottomCrop = (height - width) / 2;
                            thumbnail.Crop(topBottomCrop, 0, topBottomCrop, 0);
                        }
                        thumbnail.Resize(600, 600);
                        thumbnail.Save(thumbnailPath, ext, true);
                        
                        

                        //create file in db
                        var f = new DbModels.File();
                        f.FileName = product.Id.ToString() + "_" + z;                        

                        //create file extension in db or assign id
                        var dbExt = Db.FileExtentions.Where(x => x.Name == ext).FirstOrDefault();
                        if (dbExt != null)
                        {
                            f.FileExtentionId = dbExt.Id;
                        }
                        else
                        {
                            var fileExt = new FileExtention();
                            fileExt.Name = ext;
                            Db.FileExtentions.Add(fileExt);
                            Db.SaveChanges();

                            f.FileExtentionId = fileExt.Id;
                        }
                        Db.Files.Add(f);
                        Db.SaveChanges();

                        //create new productfile
                        var pf = new ProductFile();
                        pf.ProductId = product.Id;
                        pf.FileId = f.Id;
                        if (z == 0) pf.IsPrimary = true;
                        pvm.ProductFiles.Add(pf);

                        z++;
                    }
                }

                //save product files
                if (pvm.ProductFiles != null)
                {
                    foreach (var pf in pvm.ProductFiles)
                    {
                        Db.ProductFiles.Add(pf);
                        Db.SaveChanges();
                    }
                }

                return RedirectToAction("Index");
            }

            var query = Db.Categories.Select(c => new SelectListItem
            {
                Value = c.Id.ToString(),
                Text = c.Name,
                Selected = c.Id.Equals(1)
            });

            pvm.CategoryList = query.AsEnumerable();

            return View(pvm);
        }

        [Authorize]
        [HttpPost]
        public ActionResult GetSubCategories(string id)
        {
            var subcategories = new List<SubCategory>();
            if (id != "")
            {
                var categoryId = Int32.Parse(id);
                subcategories = Db.SubCategories.Where(x => x.CategoryId == categoryId).ToList();
            }
            return Json(subcategories);
        }

        // GET: MyProducts/Edit/5
        [Authorize]
        public ActionResult Edit(int id)
        {
            var viewmodel = new ProductViewModel();
            var productDb = Db.Products.Where(x => x.Id == id).FirstOrDefault();
            if (productDb != null && productDb.IsLive && productDb.UserId == User.Id)
            {
                viewmodel = ProductFactory.GetProduct(productDb);

                var dbSubCategoryList = Db.SubCategories.Where(x => x.CategoryId == productDb.SubCategory.CategoryId).Select(c => new SelectListItem
                {
                    Value = c.Id.ToString(),
                    Text = c.Name,
                    Selected = c.Id.Equals(productDb.SubCategoryId)
                });

                viewmodel.SubCategoryList = dbSubCategoryList.AsEnumerable();
                return View(viewmodel);
            }
            if (productDb == null || productDb.UserId != User.Id)
            {
                return Content("Error: Product not found");
            }
            if (productDb.IsLive == false)
            {
                var error = "Error: Product #" + productDb.Id + " is sold. Product can't be edited.";
                return Content(error);
            }

            return Content("Error: Something went wrong");
        }

        // POST: MyProducts/Edit/5
        [HttpPost]
        [Authorize]
        public ActionResult Edit(int id, ProductViewModel pvm, IEnumerable<HttpPostedFileBase> files)
        {
            if (ModelState.IsValid)
            {
                var productDb = Db.Products.Where(x => x.Id == pvm.Id).FirstOrDefault();
                if (productDb != null)
                {
                    productDb.Name = pvm.Name;
                    productDb.Description = pvm.Description;
                    productDb.SubCategoryId = pvm.SelectedSubCategoryId ?? default(long);
                    Db.SaveChanges();
                }

                //files
                var filenames = Db.ProductFiles.Where(x => x.ProductId == productDb.Id).Select(x => x.File.FileName).ToList();
                var z = 0;
                foreach (var fn in filenames)
                {
                    var fnIndex = Int32.Parse(fn.Substring(fn.LastIndexOf('_') + 1));
                    if (fnIndex > z) z = fnIndex;
                }
                z++;

                pvm.ProductFiles = new List<ProductFile>();
                if (files != null)
                {
                    foreach (var file in files)
                    {
                        if (file != null && file.ContentLength > 0)
                        {
                            var ext = Path.GetExtension(file.FileName).ToLower().Substring(1);

                            //save image in filesystem
                            var path = Path.Combine(Server.MapPath("~/Files/Images/ProductImages"),
                                                    Path.GetFileName(productDb.Id.ToString() + "_" + z + "." + ext));

                            var thumbnailPath = Path.Combine(Server.MapPath("~/Files/Images/ProductImages"),
                                                    Path.GetFileName(productDb.Id.ToString() + "_" + z + "_small." + ext));

                            file.SaveAs(path);

                            //save thumbnail if first
                            //if (z == 0)

                            var thumbnail = new WebImage(path);
                            var width = thumbnail.Width;
                            var height = thumbnail.Height;

                            if (width > height)
                            {
                                var leftRightCrop = (width - height) / 2;
                                thumbnail.Crop(0, leftRightCrop, 0, leftRightCrop);
                            }
                            else if (height > width)
                            {
                                var topBottomCrop = (height - width) / 2;
                                thumbnail.Crop(topBottomCrop, 0, topBottomCrop, 0);
                            }
                            thumbnail.Resize(300, 300);
                            thumbnail.Save(thumbnailPath, ext, true);



                            //create file in db
                            var f = new DbModels.File();
                            f.FileName = productDb.Id.ToString() + "_" + z;

                            //create file extension in db or assign id
                            var dbExt = Db.FileExtentions.Where(x => x.Name == ext).FirstOrDefault();
                            if (dbExt != null)
                            {
                                f.FileExtentionId = dbExt.Id;
                            }
                            else
                            {
                                var fileExt = new FileExtention();
                                fileExt.Name = ext;
                                Db.FileExtentions.Add(fileExt);
                                Db.SaveChanges();

                                f.FileExtentionId = fileExt.Id;
                            }
                            Db.Files.Add(f);
                            Db.SaveChanges();

                            //create new productfile
                            var pf = new ProductFile();
                            pf.ProductId = productDb.Id;
                            pf.FileId = f.Id;
                            if (z == 0) pf.IsPrimary = true;
                            pvm.ProductFiles.Add(pf);

                            z++;
                        }
                    }

                    //save product files
                    if (pvm.ProductFiles != null)
                    {
                        foreach (var pf in pvm.ProductFiles)
                        {
                            Db.ProductFiles.Add(pf);
                            Db.SaveChanges();
                        }
                    }
                }
                

                return RedirectToAction("Details", new { id = id });
            }

            var viewmodel = new ProductViewModel();
            var productDb1 = Db.Products.Where(x => x.Id == id).FirstOrDefault();
            if (productDb1 != null) viewmodel = ProductFactory.GetProduct(productDb1);
            return View(viewmodel);
        }        

        // GET: MyProducts/Delete/5
        [Authorize]
        public ActionResult Delete(int id)
        {
            var productDb = Db.Products.Where(x => x.Id == id).FirstOrDefault();
            var product = new Product();
            if (productDb != null && productDb.IsLive && productDb.UserId == User.Id)
            {
                product.Id = productDb.Id;
                product.Name = productDb.Name;
                product.Description = productDb.Description;
                return View(product);
            }
            if (productDb == null || productDb.UserId != User.Id)
            {
                return Content("Error: Product not found");
            }
            if (productDb != null && productDb.IsLive == false)
            {
                var error = "Error: Product #" + productDb.Id + " is sold. Product is already deleted.";
                return Content(error);
            }

            return Content("Error: Something went wrong");
        }

        // POST: MyProducts/Delete/5
        [HttpPost]
        [Authorize]
        public ActionResult Delete(Product product)
        {            
            var productDb = Db.Products.Where(x => x.Id == product.Id && x.UserId == User.Id).FirstOrDefault();
            if (productDb != null)
            {
                var transactionExist = Db.TransactionBuyRequests.Where(x => x.GiveBuyRequest.ProductId == product.Id && x.Transaction.CheckoutStatus == "Pending").ToList();
                if (transactionExist != null)
                {
                    foreach (var tbr in transactionExist)
                    {
                        tbr.CheckoutStatus = "Rejected";
                        tbr.Transaction.CheckoutStatus = "Rejected";                        
                    }
                    Db.SaveChanges();
                }
                productDb.IsLive = false;
                var brs = Db.BuyRequests.Where(x => x.ProductId == productDb.Id);
                foreach (var br in brs)
                {
                    br.IsLive = false;
                }
                Db.SaveChanges();
                return RedirectToAction("Index");
            }            
            if (productDb == null)
            {
                return Content("Product doesn't exist");
            }

            return Content("Error: Something went wrong");

        }

    }
}
