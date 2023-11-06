using Microsoft.EntityFrameworkCore;
using Project.Entity.Models;
using Project.Repositories.EfCore.Configurations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace Project.Repositories.EfCore
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext(DbContextOptions options) : base(options)
        {

        }
        // fonksiyonu geçersiz kılıp kendi işlemlerimizi çalıştıracağız.
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // migrationlara uygulanması için gerekli
            modelBuilder.ApplyConfiguration(new ProductConfig());
        }
        public DbSet<Product> Products { get; set; }
    }
}
