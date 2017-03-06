using Application.Database.Contexts;
using Application.DbModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Application.Database.Initializers
{
    public class ApplicationDbInitializer : DropCreateDatabaseIfModelChanges<ApplicationDbContext>
    {
        protected override void Seed(ApplicationDbContext db)
        {
            GetAccounts().ForEach(c => db.Accounts.Add(c));
            GetUsers().ForEach(c => db.Users.Add(c));
            GetCountries().ForEach(c => db.Countries.Add(c));
            GetCities().ForEach(c => db.Cities.Add(c));
            GetCategories().ForEach(c => db.Categories.Add(c));
            GetSubCategories().ForEach(c => db.SubCategories.Add(c));
            GetFileExtentions().ForEach(c => db.FileExtentions.Add(c));
            GetHandoverMethods().ForEach(c => db.Methods.Add(c));

            db.SaveChanges();
        }

        public List<Account> GetAccounts()
        {
            return new List<Account>() {
                new Account {
                    Id = 1,
                    Email = "test1@test.com",
                    Password = "12345678"
                },
                new Account {
                    Id = 2,
                    Email = "test2@test.com",
                    Password = "12345678"
                },
                new Account {
                    Id = 3,
                    Email = "test3@test.com",
                    Password = "12345678"
                },
                new Account {
                    Id = 4,
                    Email = "test4@test.com",
                    Password = "12345678"
                },
                new Account {
                    Id = 5,
                    Email = "test5@test.com",
                    Password = "12345678"
                },
                new Account {
                    Id = 6,
                    Email = "test6@test.com",
                    Password = "12345678"
                }
            };
        }

        public List<User> GetUsers()
        {
            var users = new List<User>() {
                new User {
                    Id = 1,
                    FirstName = "Van",
                    LastName = "Tran",
                    Birthday = new DateTime(1991, 6, 3),
                    Gender = true,
                    AccountId = 1,
                },
                new User {
                    Id = 2,
                    FirstName = "Dustin",
                    LastName = "Brooks",
                    Birthday = new DateTime(1988, 1, 1),
                    Gender = false,
                    AccountId = 2,
                },
                new User {
                    Id = 3,
                    FirstName = "Stephanie",
                    LastName = "George",
                    Birthday = new DateTime(1982, 3, 12),
                    Gender = true,
                    AccountId = 3,
                }
                ,
                new User {
                    Id = 3,
                    FirstName = "Scott",
                    LastName = "Allen",
                    Birthday = new DateTime(1977, 10, 3),
                    Gender = false,
                    AccountId = 4,
                }
                ,
                new User {
                    Id = 3,
                    FirstName = "Lucille",
                    LastName = "Reyes",
                    Birthday = new DateTime(1978, 5, 22),
                    Gender = true,
                    AccountId = 5,
                }
                ,
                new User {
                    Id = 3,
                    FirstName = "Carmen",
                    LastName = "Rhodes",
                    Birthday = new DateTime(1991, 8, 12),
                    Gender = true,
                    AccountId = 6,
                }
            };

            return users;
        }

        public List<Country> GetCountries()
        {
            return new List<Country>() {
                new Country {
                    Id = 1,
                    Name = "United States"
                },
                new Country {
                    Id = 2,
                    Name = "United Kingdom"
                }
            };
        }

        public List<City> GetCities()
        {
            return new List<City>() {
                new City {
                    Id = 1,
                    Name = "Chicago",
                    CountryId = 1
                },
                new City {
                    Id = 2,
                    Name = "New York",
                    CountryId = 1
                },
                new City {
                    Id = 3,
                    Name = "London",
                    CountryId = 2
                },
                new City {
                    Id = 4,
                    Name = "Leed",
                    CountryId = 2
                }
            };
        }

        public List<Category> GetCategories()
        {
            return new List<Category>() {
                new Category {
                    Id = 1,
                    Name = "Fashion",
                }
            };
        }

        public List<SubCategory> GetSubCategories()
        {
            return new List<SubCategory>() {
                new SubCategory {
                    Id = 1,
                    Name = "Accessories",
                    CategoryId = 1,
                }
            };
        }

        public List<FileExtention> GetFileExtentions()
        {
            return new List<FileExtention>() {
                new FileExtention {
                    Id = 1,
                    Name = "jpg"
                },
                new FileExtention {
                    Id = 2,
                    Name = "pdf"
                },
            };
        }

        public List<HandoverMethod> GetHandoverMethods()
        {
            return new List<HandoverMethod>() {
                new HandoverMethod {
                    Id = 1,
                    Name = "In person"
                },
                new HandoverMethod {
                    Id = 2,
                    Name = "Shipping"
                },
                new HandoverMethod {
                    Id = 3,
                    Name = "Electronically"
                },
            };
        }
    }
}