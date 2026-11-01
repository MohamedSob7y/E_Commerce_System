using E_Commerce.Domain.Entityes.OrderModule;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Persistance.Data.Configurations
{
    public class OrderItemConfiguration : IEntityTypeConfiguration<OrderItems>
    {
        public void Configure(EntityTypeBuilder<OrderItems> builder)
        {
            builder.Property(x => x.Price).HasColumnType("decimal(8,2)");

            builder.OwnsOne(x => x.Product, oE =>
            {
                oE.Property(x => x.ProductName).HasMaxLength(100);
                oE.Property(x => x.PictureUrl).HasMaxLength(200);
            });


        }
    }
}
