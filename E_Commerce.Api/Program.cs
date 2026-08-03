
using E_Commerce.Api.Extentions;
using E_Commerce.Domain.Constracts;
using E_Commerce.Persistance.Data.DataSeeding;
using E_Commerce.Persistance.Data.DBContext;
using E_Commerce.Persistance.Repositories;
using E_Commerce.Services;
using E_Commerce.Services.Abstraction;
using E_Commerce.Services.Mapping_Profiles;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Writers;
using StackExchange.Redis;
using System.Reflection.Metadata.Ecma335;
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
            #region Inject Dependency For DbContext
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
            #region Inject Object From IConnectionMultiplexer using Redis For Basket Module

            builder.Services.AddSingleton<IConnectionMultiplexer>(P=>
            {
                return ConnectionMultiplexer.Connect(builder.Configuration.GetConnectionString("RedisConnection")!);
            });//لانى عايز الObject All Life Cycle of Application 
            #endregion
            //============================================
            #region Inject Object From IBasketRepo in BasketService
            builder.Services.AddScoped<IBasketRepository,BasketRepository>();
            #endregion
            //============================================
            #region Inject Object From Mapping Profile
            builder.Services.AddAutoMapper(typeof(ServiceAssemblyReference).Assembly);//Use Assembly For Licence => So Download Version Package Automapper اللى هى 14.0.0
            //Make Dummy Class عشان مش عايز main.cs يوصل للMapping Profile لان دا مش احسن فى security بالتالى بخليه يوصل للDummy class واصلا هو بيعمل Assembly in Runtime for all Class in the Same Project اللى مع الclass دا فهيقدر يوصل للMapping Profile
            //كدة قدرت اوصله بس مش Direct على طول لاء انا روحت استخدمت الDummy class يعنى Empty Class is public وهو بيعمل Resolve for all Classes in the same Project Assembly
            #endregion
            //============================================
            #region Inject Object From ProductPictureUrlResolver
            builder.Services.AddTransient<ProductPictureUrlResolver>();
            #endregion
            //============================================
            #region Inject Object From ISedding Data
            builder.Services.AddScoped<IDataSeeding, DataSeeding>();//Inject Object From ISeeding Data
            #endregion
            //============================================
            #region Inject Object From IProductService
            builder.Services.AddScoped<IProductService,ProductService>();
            #endregion
            //============================================
            #region Inject Object from IBasketService
            builder.Services.AddScoped<IBasketService, BasketService>();
            #endregion
            //============================================
            #region Inject Object From ICashRepository
            builder.Services.AddScoped<ICashRepository, CashRepository>();
            #endregion
            //============================================
            #region Inject Object From ICashService
            builder.Services.AddScoped<ICashService, CashSerivce>();
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
            app.UseStaticFiles();

            app.MapControllers();
            #endregion
            //============================================
            #region Run Application
            app.Run(); 
            #endregion
        }
    }
}
