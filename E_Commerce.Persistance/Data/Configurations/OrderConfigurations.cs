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
    public class OrderConfigurations : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.Property(x => x.SubTotal).HasColumnType("decimal(8,2)");

            builder.OwnsOne(x => x.Address, oE =>
            {
                oE.Property(x => x.FirstName).HasMaxLength(50);
                oE.Property(x => x.LastName).HasMaxLength(50);
                oE.Property(x => x.City).HasMaxLength(50);
                oE.Property(x => x.Street).HasMaxLength(50);
                oE.Property(x => x.Country).HasMaxLength(50);
               
            });


           


        }
    }
}
