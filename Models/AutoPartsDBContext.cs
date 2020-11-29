using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoPartsCompany.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace AutoPartsCompany.Models
{
    public class AutoPartsDBContext: IdentityDbContext
    {
        public AutoPartsDBContext(DbContextOptions<AutoPartsDBContext> options):base(options)
        {
            //Database.EnsureCreated();
        }

        public DbSet<AutoPartsModel> AutoPartsModel { get; set; }

        public DbSet<AutoPartsCompany.Models.CategoryModel> CategoryModel { get; set; }

        public DbSet<AutoPartsCompany.Models.OfferModel> OfferModel { get; set; }
        public DbSet<AutoPartsCompany.Models.InboxModel> InboxModel { get; set; }
        public DbSet<AutoPartsCompany.Models.MessageModel> MessageModel { get; set; }
    }
}
