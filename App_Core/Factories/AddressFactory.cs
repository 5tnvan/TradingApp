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
    static class AddressFactory
    {
        public static AddressViewModel GetAddress(Address Address)
        {
            var avm = new AddressViewModel();
            
            var defaultCountryList = AppCore.Db.Countries.Select(c => new SelectListItem
            {
                Value = c.Id.ToString(),
                Text = c.Name
            });
            var defaultCityList = new List<SelectListItem>();

            //default values
            avm.Id = Address.Id;
            avm.StreetName = Address.StreetName;
            avm.StreetNumber = Address.StreetNumber;
            avm.SelectedCityId = 0;
            avm.SelectedCountryId = 0;
            avm.Cities = defaultCityList.AsEnumerable();
            avm.Countries = defaultCountryList.AsEnumerable();

            //override if address exist
            if (Address.CityId != 0)
            {
                avm.SelectedCityId = Address.CityId;
                avm.SelectedCountryId = Address.City.CountryId;
                var dbCountryList = AppCore.Db.Countries.Select(c => new SelectListItem
                {
                    Value = c.Id.ToString(),
                    Text = c.Name,
                    Selected = c.Id.Equals(Address.City.CountryId)
                });
                var dbCityList = AppCore.Db.Cities.Where(c => c.CountryId == Address.City.CountryId).Select(c => new SelectListItem
                {
                    Value = c.Id.ToString(),
                    Text = c.Name,
                    Selected = c.Id.Equals(Address.CityId)
                });
                avm.Countries = dbCountryList.AsEnumerable();
                avm.Cities = dbCityList.AsEnumerable();
            }

            return avm;
        }
    }
}