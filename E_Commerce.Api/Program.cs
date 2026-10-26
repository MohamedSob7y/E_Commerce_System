
using E_Commerce.Api.Extentions;
using E_Commerce.Api.Factories;
using E_Commerce.Api.Middleware;
using E_Commerce.Domain.Constracts;
using E_Commerce.Domain.Entityes.Identity_Module;
using E_Commerce.Persistance.Data.DataSeeding;
using E_Commerce.Persistance.Data.DBContext;
using E_Commerce.Persistance.Identity_Data.DbContext;
using E_Commerce.Persistance.IdentityData.Data_Seed;
using E_Commerce.Persistance.Repositories;
using E_Commerce.Services;
using E_Commerce.Services.Abstraction;
using E_Commerce.Services.Mapping_Profiles;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Writers;
using StackExchange.Redis;
using System.Reflection.Metadata.Ecma335;
using System.Text;
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
            #region Inject object From DbContext For Security
            builder.Services.AddDbContext<StoreIdentityDbContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("IdentityConnection"));
            });

            #endregion
            //============================================
            #region Inject Object From IUniteofWork
            builder.Services.AddScoped<IUniteofWork, UniteofWork>();
            #endregion
            //============================================
            #region Inject Object From IConnectionMultiplexer using Redis For Basket Module

            builder.Services.AddSingleton<IConnectionMultiplexer>(P =>
            {
                return ConnectionMultiplexer.Connect(builder.Configuration.GetConnectionString("RedisConnection")!);
            });//لانى عايز الObject All Life Cycle of Application 
            #endregion
            //============================================
            #region Inject Object From IBasketRepo in BasketService
            builder.Services.AddScoped<IBasketRepository, BasketRepository>();
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
            //builder.Services.AddScoped<IDataSeeding, DataSeeding>();//Inject Object From ISeeding Data
            //هنا المفروض كمان اسجل انه لما يطلب object From IDataSeeding اديه Object From IdentityDataInitializer
            //builder.Services.AddScoped<IDataSeeding, IdentityDataInitializer>();//بس لما انا بعمل الطريقة دى كدة انا بعمل override على الطريقة الاولى والمفروض انا عايز اعمل الTwo Implementation 
            //يبقى الحل انى استخدم الaddKeys عشان لما يطلب object from IDataSeeding  هشوف الimplementation بتاعه على حسب الImplmentation بتاعه هديله الobject المعين اللى هو طالبه 

            builder.Services.AddKeyedScoped<IDataSeeding, IdentityDataInitializer>("Identity");
            builder.Services.AddKeyedScoped<IDataSeeding, DataSeeding>("Default");
            //يبقى اديك الobject على حسب انت بتعتلى ال  Key اية هو على حسب الimplementation
            #endregion
            //============================================
            #region Inject Object From IProductService
            builder.Services.AddScoped<IProductService, ProductService>();
            #endregion
            //============================================
            #region Inject Object from IBasketService
            builder.Services.AddScoped<IBasketService, BasketService>();
            #endregion
            //============================================
            #region Inject Object From IAuthenticationService
            builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();
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
            #region Set Setting Of Message Validation Error اللى راجعة 
            builder.Services.Configure<ApiBehaviorOptions>
                   (options =>
                   {
                       options.InvalidModelStateResponseFactory =
                       ApiResponseFactory.GenerateApiValidationResponse;
                   });
            #endregion
            //============================================
            #region Ask CLr to inject Object From 2 Service RoleManger+ UserManger لازم اعمله 
            builder.Services.AddIdentityCore<ApplicationUser>().AddRoles<IdentityRole>().AddEntityFrameworkStores<StoreIdentityDbContext>();//دى الطريقة الاحسن  


            //builder.Services.AddIdentityCore<ApplicationUser>(options =>
            //{
            //    options.User.RequireUniqueEmail = true;
            //   // options.User.AllowedUserNameCharacters = new[]//حط الحروف اللى متاحة 
            //   //من غير ماتحط اى حاجة بيتعمل Default    
            //}).AddRoles<IdentityRole>().AddEntityFrameworkStores<StoreIdentityDbContext>();//دى الطريقة الاحسن  
            //الطريقة دى بقا بتنزلى الحجات اللى انا محتاجها فقط من Service Identity يعنى انا دلؤقتى استخدمت الIdentityCode فبتنزل الUserManger اللى انا محتاجه +  زودت ان انزل عليهم IdentityRole بزود اللى انا عايزه براحتى عشان مينزلشى اى حاجة انا مش محتاجها عشان دا يبقى مش احسن حاجة فى الPerformance
            //ولو انا محتاج اعمل Conatrin when Createing UserManger اعمل Options

            //builder.Services.AddIdentity<ApplicationUser, IdentityRole>().AddEntityFrameworkStores<StoreIdentityDbContext>();//دى الطريقة الاولى 
            //الطريقة دى لما بستخدمها بتنزل الservice Of Identity Automatic like UserManger داانا محتاجه + IdentityRole ودا محتاجه + SignInManger وانا مش محتاجه بس بينزل تلقائى لما بستخدم الطريقة دى عشان كدة الطريقة دى مش احسن حاجة لانها بتنزلى جات او service From Identity انا مش محتاجها 
            //اخر جزئية اللى هى AddEntity انا بقؤل ان الComponent اللى بتنزل هيكون ليها access for Tables اللى نزلت مع الPackage Identity
            //So انا بقؤله الManger اللى نزلتها هيبقى ليها access on IdentityDbContextStore عشان الtables اللى نزلت مع الPackage Identity 
            #endregion

            //بتحدد  Default Authentication Schema + 
            builder.Services.AddAuthentication(options =>
            {
                //default AuthenitcationSchema == the Same Schema اللى عملت بيها الToken بتاعى => JWT Beare
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;//دى فى حالة ان Token invalida Call Use Authorization Middleware 
            }).AddJwtBearer(options =>//عشان اعلمه ازاى يعمل Validate For Token
            {
                options.SaveToken = true;//Save Token in httpcontext ولو احتاجته بجيبه عن طريق Method GetToken لو تمم اخزنه ولو مش تمم مش هخزنه
                //بقؤله يبص على اية عشان يعرف هو صح ولا غلط 
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    //هنا بقؤله مكان الValid Value يقراها من appsetting عشان يقارن بين اللى موجودين عنده واللى User باعتها فى الToken
                    ValidIssuer = builder.Configuration["JwtOptions:Issuer"],
                    ValidAudience = builder.Configuration["JwtOptions:Audience"],
                    IssuerSigningKey=new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JwtOptions:SecretKey"]!))//Check For Secret Key
                    //كل دى حجات بيبص عليها عشان يعمل Validate for Token 
                };
            });

            //============================================
            #region Build Application on server
            var app = builder.Build();
            #endregion
            //============================================
            #region Call Method Seeding Data
            //عشان اعرف انادى الMethod اللى جوه الSeedingData هى محتاجه DbContext object وهنا مش هعرف ask CLR To Inject this Object By implicitlyعن طريق Constructor لان مشهينفع اعمله هنا فى Main بالتالى هطلبه بطريقة غير مباشرة Explcitily From Clr
            //Explcicit Injection Fom DbContext
            await app.MigrateDatabaseAsync();//Call Extention Method
            await app.MigrateIdentityDatabaseAsync();//this Method For Applying Pending Migration For Security IdentitytDb
            await app.SeedDataAsync();//Call ExtentionMethods
            await app.SeedIdentityDataAsync();
            #endregion
            //============================================
            #region Configuration using MiddleWare
            //this First Middleware يعدى عليها الrequest
            // Configure the HTTP request pipeline.
            //app.Use(async (context, next) =>
            //{
            //    try
            //    {
            //        await next(); //دى معناها لو محصلشى اى مشكلة فى تنفيذ الmiddlware ادخل على اللى بعدها ونادى عليها 
            //    }//طب فى حالة ان فى مشكلة كدة المفروض اعمل logic ومش هكمل باقى الflow
            //    catch (Exception ex)
            //    {
            //        Console.WriteLine(ex.Message);//write Exception in Console
            //        context.Response.StatusCode=StatusCodes.Status500InternalServerError;
            //        await context.Response.WriteAsJsonAsync(new
            //       {
            //           StatusCode=StatusCodes.Status500InternalServerError,
            //           Error=$"An Expected Error : {ex.Message}"
            //       });//write Expcetion in Json
            //    }
            //});
            app.UseMiddleware<ExceptionHandlerMiddleware>();//this Method take Any Class will be represented as middlware 
            //طب ازاى الclass اللى باعتهوله يتعمل كانه middleware=> must implement interface IMiddlware
            //حل تانى Make Constructor take RequestDelegate => Call Next Middlware then implement method Invoke take HttpContext عشان اعرف انادى على الMiddlware اللى بعدى وميكنشى فى اى مشاكل 
            //لان لو بعتله اى class وخلاص هيضرب منى exception in Runtime Not in Compilation Time
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseAuthorization();//دول عشان الserver فى كل request بيبعته الClient يتاكدة من الJWT اللى باعته + Validate عليه 
            //دى المرحلة بعد ما الserverعمل الtoken For User ورجعوله 
            //فالuser send Request Using this Token and Server Validat For this Reuqest by Pipline Use Authentication + Use Authorization

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
