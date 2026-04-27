using E_Commerce.Domain.Entityes;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Persistance.Data.Configurations
{
    public class ProductConfigurations : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.Property(T => T.Name).HasMaxLength(200);
            builder.Property(T => T.Description).HasMaxLength(500);
            builder.Property(T => T.PictureURL).HasMaxLength(200);
            builder.Property(T => T.Price).HasPrecision(18,2);
            builder.HasOne(T => T.ProductBrand)
                .WithMany()
                .HasForeignKey(T => T.ProductBrandId);
            builder.HasOne(T => T.ProductType)
                    .WithMany()
                    .HasForeignKey(T => T.ProductTypeId);
        }
    }
}
