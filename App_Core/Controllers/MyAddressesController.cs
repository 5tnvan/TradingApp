using Application.DbModels;
using Application.Factories;
using Application.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace Application.Controllers
{
    public class MyAddressesController : BaseController
    {
        // GET: MyAddresses
        public ActionResult Index()
        {
            return View(User.UserAddresses.ToList());
        }

        // GET: MyAddresses/Details
        [Authorize]
        public ActionResult Details(int id)
        {
            var userAddress = User.UserAddresses.Where(x => x.AddressId == id).FirstOrDefault();
            
            if (userAddress != null)
            {
                //user Id for link
                var userId = User.Id;
                ViewBag.userId = userId.ToString();

                return View(userAddress.Address);                
            }
            if (userAddress == null)
            {
                return Content("Error: Address not found");
            }
            return Content("Error: Something went wrong");            
        }

        //GET: MyAddresses/Create
        [Authorize]
        public ActionResult Create()
        {
            var viewmodel = new AddressViewModel();

            //set up dropdown
            var defaultCountryList = Db.Countries.Select(c => new SelectListItem
            {
                Value = c.Id.ToString(),
                Text = c.Name
            });
            var defaultCityList = new List<SelectListItem>();
            viewmodel.Cities = defaultCityList.AsEnumerable();
            viewmodel.Countries = defaultCountryList.AsEnumerable();
            
            //user Id for link
            var userId = User.Id;
            ViewBag.userId = userId.ToString();

            return View(viewmodel);
        }

        [Authorize]
        [HttpPost]
        public ActionResult Create(AddressViewModel avm)
        {
            if (ModelState.IsValid)
            {
                var address = new Address {
                    StreetName = avm.StreetName,
                    StreetNumber = avm.StreetNumber,
                    CityId = avm.SelectedCityId
                };

                var userAddress = new UserAddress {
                    UserId = User.Id,
                    Address = address
                };

                User.UserAddresses.Add(userAddress);
                Db.SaveChanges();

                return RedirectToAction("Details", "MyUsers", new { id = User.Id });
            }

            //populate dropdown list
            var defaultCountryList = Db.Countries.Select(c => new SelectListItem
            {
                Value = c.Id.ToString(),
                Text = c.Name
            });
            var defaultCityList = new List<SelectListItem>();
            avm.Cities = defaultCityList.AsEnumerable();
            avm.Countries = defaultCountryList.AsEnumerable();

            //user Id for link
            var userId = User.Id;
            ViewBag.userId = userId.ToString();

            return View(avm);
        }

        //GET: MyAddresses/Edit
        [Authorize]
        public ActionResult Edit(int id)
        {
            var userAddress = User.UserAddresses.Where(x => x.AddressId == id).FirstOrDefault();
            var viewmodel = new AddressViewModel();
            if (userAddress != null)
            {
                viewmodel = AddressFactory.GetAddress(userAddress.Address);

                //user Id for link
                var userId = User.Id;
                ViewBag.userId = userId.ToString();

                return View(viewmodel);
            }
            if (userAddress == null)
            {
                return Content("Error: User not found");
            }
            return Content("Error: Something went wrong");
        }

        // POST: MyAddresses/Edit
        [HttpPost]
        [Authorize]
        public ActionResult Edit(int id, AddressViewModel avm)
        {
            var userAddress = User.UserAddresses.Where(x => x.AddressId == avm.Id).FirstOrDefault();

            if (ModelState.IsValid && userAddress != null)
            {
                userAddress.Address.StreetName = avm.StreetName;
                userAddress.Address.StreetNumber = avm.StreetNumber;
                userAddress.Address.CityId = avm.SelectedCityId;
                Db.SaveChanges();                

                return RedirectToAction("Details", "MyUsers", new { id = User.Id });
            }

            //populate dropdown list
            var cities = new List<SelectListItem>();
            var countries = Db.Countries.Select(c => new SelectListItem
            {
                Value = c.Id.ToString(),
                Text = c.Name
            });
            avm.SelectedCityId = 0;
            avm.SelectedCountryId = 0;
            avm.Cities = cities.AsEnumerable();
            avm.Countries = countries.AsEnumerable();

            //user Id for link
            var userId = User.Id;
            ViewBag.userId = userId.ToString();

            return View(avm);
        }

        //GET: MyAddresses/Delete
        [Authorize]
        public ActionResult Delete(int id)
        {
            var userAddress = Db.UserAddresses.Where(x => x.AddressId == id).FirstOrDefault();
            if (userAddress != null)
            {
                //user Id for link
                var userId = User.Id;
                ViewBag.userId = userId.ToString();

                return View(userAddress.Address);
            }
            if (userAddress == null) return Content("Error: Address not found");
            return Content("Error: Something went wrong");
        }

        //POST: MyAddresses/Delete
        [Authorize]
        [HttpPost]
        public ActionResult Delete(int id, Address ad)
        {
            var userAddress = User.UserAddresses.Where(x => x.AddressId == ad.Id).FirstOrDefault();
            if (userAddress != null)
            {
                var address = userAddress.Address;
                Db.UserAddresses.Remove(userAddress);                
                Db.SaveChanges();
                Db.Addresses.Remove(address);
                Db.SaveChanges();

                //user Id for link
                var userId = User.Id;
                ViewBag.userId = userId.ToString();

                return RedirectToAction("Details", "MyUsers", new { id = User.Id });
            }
            return Content("Error: Something went wrong");
        }


        //Ajax helper
        [Authorize]
        [HttpPost]
        public ActionResult GetCities(string id)
        {
            var cities = new List<City>();
            if (id != "")
            {
                var countryId = Int32.Parse(id);
                cities = Db.Cities.Where(x => x.CountryId == countryId).ToList();
            }
            return Json(cities);
        }

    }
}