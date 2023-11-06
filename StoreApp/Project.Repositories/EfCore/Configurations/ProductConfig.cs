using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Project.Entity.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Repositories.EfCore.Configurations
{
    public class ProductConfig : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.HasData(
                new Product { Id = 1, Name = "Lenovo Gaming Pad 3", Price = 17500 },
                new Product { Id = 2, Name = "Rampage Oyuncu Koltuğu", Price = 3850 },
                new Product { Id = 3, Name = "Samsung LCD Ekran 21 hz", Price = 4200 }
                );
        }
    }
}
