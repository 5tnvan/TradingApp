using Application.DbModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Application.Database.Contexts
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext() : base("Application")
        {

        }

        public DbSet<User> Users { get; set; }
        public DbSet<Account> Accounts { get; set; }
        public DbSet<Phone> Phones { get; set; }
        public DbSet<Address> Addresses { get; set; }        
        public DbSet<City> Cities { get; set; }
        public DbSet<Country> Countries { get; set; }
        public DbSet<File> Files { get; set; }
        public DbSet<FileExtention> FileExtentions { get; set; }
        public DbSet<UserFile> UserFiles { get; set; }
        public DbSet<UserAddress> UserAddresses { get; set; }
        public DbSet<UserPhone> UserPhones { get; set; }

        public DbSet<Product> Products { get; set; }
        public DbSet<ProductFile> ProductFiles { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<SubCategory> SubCategories { get; set; }

        public DbSet<Rating> Ratings { get; set; }
        public DbSet<BuyRequest> BuyRequests { get; set; }
        public DbSet<TransactionBuyRequest> TransactionBuyRequests { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<HandoverMethod> Methods { get; set; }
        public DbSet<Handover> Handovers { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            //one to many
            modelBuilder.Entity<User>().HasRequired(c => c.Account).WithMany().WillCascadeOnDelete(false);
            modelBuilder.Entity<UserFile>().HasRequired(c => c.User).WithMany().WillCascadeOnDelete(false);
            modelBuilder.Entity<UserFile>().HasRequired(c => c.File).WithMany().WillCascadeOnDelete(false);
            modelBuilder.Entity<UserAddress>().HasRequired(c => c.User).WithMany().WillCascadeOnDelete(false);
            modelBuilder.Entity<UserAddress>().HasRequired(c => c.Address).WithMany().WillCascadeOnDelete(false);
            modelBuilder.Entity<UserPhone>().HasRequired(c => c.User).WithMany().WillCascadeOnDelete(false);
            modelBuilder.Entity<UserPhone>().HasRequired(c => c.Phone).WithMany().WillCascadeOnDelete(false);
            modelBuilder.Entity<Address>().HasRequired(c => c.City).WithMany().WillCascadeOnDelete(false);
            modelBuilder.Entity<City>().HasRequired(c => c.Country).WithMany().WillCascadeOnDelete(false);
            modelBuilder.Entity<File>().HasRequired(c => c.FileExtention).WithMany().WillCascadeOnDelete(false);
            modelBuilder.Entity<SubCategory>().HasRequired(c => c.Category).WithMany().WillCascadeOnDelete(false);
            modelBuilder.Entity<Product>().HasRequired(c => c.User).WithMany().WillCascadeOnDelete(false);
            modelBuilder.Entity<Product>().HasRequired(c => c.SubCategory).WithMany().WillCascadeOnDelete(false);
            modelBuilder.Entity<ProductFile>().HasRequired(c => c.Product).WithMany().WillCascadeOnDelete(false);
            modelBuilder.Entity<ProductFile>().HasRequired(c => c.File).WithMany().WillCascadeOnDelete(false);
            modelBuilder.Entity<BuyRequest>().HasRequired(c => c.User).WithMany().WillCascadeOnDelete(false);
            modelBuilder.Entity<BuyRequest>().HasRequired(c => c.Product).WithMany().WillCascadeOnDelete(false);
            modelBuilder.Entity<TransactionBuyRequest>().HasRequired(c => c.GiveBuyRequest).WithMany().WillCascadeOnDelete(false);
            modelBuilder.Entity<TransactionBuyRequest>().HasRequired(c => c.GetBuyRequest).WithMany().WillCascadeOnDelete(false);
            modelBuilder.Entity<TransactionBuyRequest>().HasRequired(c => c.Transaction).WithMany().WillCascadeOnDelete(false);
            modelBuilder.Entity<Handover>().HasRequired(c => c.HandoverMethod).WithMany().WillCascadeOnDelete(false);

            base.OnModelCreating(modelBuilder);
        }
    }
}