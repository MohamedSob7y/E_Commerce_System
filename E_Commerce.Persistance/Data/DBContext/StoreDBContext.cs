using E_Commerce.Domain.Entityes;
using E_Commerce.Domain.Entityes.Identity_Module;
using E_Commerce.Persistance.Data.Configurations;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Persistance.Data.DBContext
{

    #region لو فصلت الSecurity Db عن System Db
    //يعنى لو عملت كل Db لوحدها مش كله مع بعض 
    public class StoreDBContext : DbContext
    {
        public StoreDBContext(DbContextOptions<StoreDBContext> options)
            : base(options)
        {

        }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductBrand> productBrands { get; set; }
        public DbSet<ProductType> productTypes { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());//بيدور على اى objects implementent interface Ientitytypeconfiguration Folder Configurations عن طريقdepedency
            //Anthore Way لو هو مش عارف يدور او يوصل للLayer اللى بتعمل Configuration 
            //modelBuilder.ApplyConfigurationsFromAssembly(typeof(ProductConfigurations).Assembly);//بيدور على اى objects implementent interface Ientitytypeconfiguration Folder Configurations عن طريقdepedency
            //كدة هو هيروح للclass دا فعيرف يوصل للFolder Configuration
        }
    } 
    #endregion


    //====================================================================
    #region Make Security + System in One DB
    //دا فى حالة انى عامل Security + System in Single Database 
    //After Asp.NetIdentity must Make inheritance From IdentityDbContext
    //public class StoreDBContext : IdentityDbContext<ApplicationUser>
    //{
    //    public StoreDBContext(DbContextOptions<StoreDBContext> options)
    //        : base(options)
    //    {

    //    }
    //    public DbSet<Product> Products { get; set; }
    //    public DbSet<ProductBrand> productBrands { get; set; }
    //    public DbSet<ProductType> productTypes { get; set; }
    //    protected override void OnModelCreating(ModelBuilder modelBuilder)
    //    {
    //        base.OnModelCreating(modelBuilder);//بينادى على Configuration اللى انا ورثتها من IdentityDbContext لما نزلت الPackage of Identity فلازم انا دى عليها الاول قبل مايعمل Configuration بتاعت الSystem بتاعته 
    //        //كدة انا ناديت على الحجات اللى ورثتها اللى بتخص الSecurity then الحجات بتاعت الsystem  بقا 
    //        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());//بيدور على اى objects implementent interface Ientitytypeconfiguration Folder Configurations عن طريقdepedency
    //        //Anthore Way لو هو مش عارف يدور او يوصل للLayer اللى بتعمل Configuration 
    //        //modelBuilder.ApplyConfigurationsFromAssembly(typeof(ProductConfigurations).Assembly);//بيدور على اى objects implementent interface Ientitytypeconfiguration Folder Configurations عن طريقdepedency
    //        //كدة هو هيروح للclass دا فعيرف يوصل للFolder Configuration
    //    }
    //} 
    #endregion
}
