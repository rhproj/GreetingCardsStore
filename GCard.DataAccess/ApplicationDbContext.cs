using GCard.Model;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GCard.DataAccess
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<ItemType> ItemType { get; set; }
        public DbSet<Occasion> Occasion { get; set; }
        public DbSet<ProductItem> ProductItem { get; set; }
        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
    }
}
