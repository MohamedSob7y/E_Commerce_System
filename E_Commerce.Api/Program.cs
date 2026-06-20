
using E_Commerce.Api.Extentions;
using E_Commerce.Domain.Constracts;
using E_Commerce.Persistance.Data.DataSeeding;
using E_Commerce.Persistance.Data.DBContext;
using E_Commerce.Persistance.Repositories;
using E_Commerce.Services.Mapping_Profiles;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Writers;
using System.Threading.Tasks;

namespace E_Commerce.Api
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            #region Create Application
            var builder = WebApplication.CreateBuilder(args);
            #endregion
            //============================================
            #region Service For Application
            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            #endregion
            //============================================
            #region Inject Dependency
            builder.Services.AddDbContext<StoreDBContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("Default"));
            });

            #endregion
            //============================================
            #region Inject Object From IUniteofWork
            builder.Services.AddScoped<IUniteofWork, UniteofWork>();
            #endregion
            //============================================
            #region Inject Object From Mapping Profile
            builder.Services.AddAutoMapper(X=>X.AddProfile(typeof(ProductProfile)));
            #endregion
            //============================================
            #region Inject Object From ISedding Data
            builder.Services.AddScoped<IDataSeeding, DataSeeding>();//Inject Object From ISeeding Data
            #endregion
            //============================================
            #region Build Application on server
            var app = builder.Build();
            #endregion
            //============================================
            #region Call Method Seeding Data
            //عشان اعرف انادى الMethod اللى جوه الSeedingData هى محتاجه DbContext object وهنا مش هعرف ask CLR To Inject this Object By implicitlyعن طريق Constructor لان مشهينفع اعمله هنا فى Main بالتالى هطلبه بطريقة غير مباشرة Explcitily From Clr
            //Explcicit Injection Fom DbContext
            await app.MigrateDatabaseAsync();//Call Extention Method
            await app.SeedDataAsync();//Call ExtentionMethods
            #endregion
            //============================================
            #region Configuration using MiddleWare
            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();
            #endregion
            //============================================
            #region Run Application
            app.Run(); 
            #endregion
        }
    }
}
