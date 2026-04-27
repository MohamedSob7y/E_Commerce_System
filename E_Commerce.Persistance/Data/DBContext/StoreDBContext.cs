using E_Commerce.Domain.Entityes;
using E_Commerce.Persistance.Data.Configurations;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Persistance.Data.DBContext
{
    public class StoreDBContext:DbContext
    {
        public StoreDBContext(DbContextOptions<StoreDBContext> options)
            :base(options)
        {
            
        }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductBrand> productBrands {  get; set; }
        public DbSet<ProductType> productTypes { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
             modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());//بيدور على اى objects implementent interface Ientitytypeconfiguration Folder Configurations عن طريقdepedency
            //Anthore Way لو هو مش عارف يدور او يوصل للLayer اللى بتعمل Configuration 
            //modelBuilder.ApplyConfigurationsFromAssembly(typeof(ProductConfigurations).Assembly);//بيدور على اى objects implementent interface Ientitytypeconfiguration Folder Configurations عن طريقdepedency
            //كدة هو هيروح للclass دا فعيرف يوصل للFolder Configuration
        }
    }
}
