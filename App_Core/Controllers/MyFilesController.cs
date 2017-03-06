using Application.DbModels;
using Application.Factories;
using Application.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using System.Web.Security;

namespace Application.Controllers
{
    public class MyFilesController : BaseController
    {
        // GET: MyFiles
        public ActionResult Index()
        {            
            return View();
        }

        //GET: MyFiles/Create
        [Authorize]
        public ActionResult Create()
        {
            //user Id for link
            var userId = User.Id;
            ViewBag.userId = userId.ToString();

            return View();
        }

        // POST: MyFiles/Create
        [HttpPost]
        [Authorize]
        public ActionResult Create(HttpPostedFileBase file)
        {
            if (file != null && file.ContentLength > 0)
            {                               
                //mark current primary as non-primary
                var userPrimaryFile = Db.UserFiles.Where(x => x.UserId == User.Id && x.IsPrimary == true).FirstOrDefault();
                if (userPrimaryFile != null) { userPrimaryFile.IsPrimary = false; Db.SaveChanges(); }


                //if user has more file, take last index of name
                var userFiles = Db.UserFiles.Where(x => x.UserId == User.Id).Select(x => x.File.FileName).ToList();
                var z = 0;
                foreach (var ufile in userFiles)
                {
                    var ufIndex = Int32.Parse(ufile.Substring(ufile.LastIndexOf('_') + 1));
                    if (ufIndex > z) z = ufIndex;
                }
                if (userFiles.Count() > 0) z++;

                //save image in filesystem
                var ext = Path.GetExtension(file.FileName).ToLower().Substring(1);                
                var path = Path.Combine(Server.MapPath("~/Files/Images/UserImages"),
                                        Path.GetFileName("person_" + User.Id.ToString() + "_" + z + "." + ext));
                var thumbnailPath = Path.Combine(Server.MapPath("~/Files/Images/UserImages"),
                                        Path.GetFileName("person_" + User.Id.ToString() + "_" + z + "_small." + ext));

                file.SaveAs(path);

                //save thumbnail in filesystem
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
                thumbnail.Resize(150, 150);
                thumbnail.Save(thumbnailPath, ext, true);

                //create file in db
                var f = new DbModels.File();
                f.FileName = "person_" + User.Id.ToString() + "_" + z;

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

                //create new userfile
                var uf = new UserFile();
                uf.UserId = User.Id;
                uf.FileId = f.Id;
                uf.IsPrimary = true;
                Db.UserFiles.Add(uf);
                Db.SaveChanges();

                return RedirectToAction("Details", "MyUsers", new { id = User.Id });
            }
            if (file != null) return Content("Error: File can't be empty");
            return Content("Error: Something went wrong");
        }

    }
}