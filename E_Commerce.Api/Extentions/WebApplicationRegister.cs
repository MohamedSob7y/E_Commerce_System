using E_Commerce.Domain.Constracts;
using E_Commerce.Persistance.Data.DBContext;
using Microsoft.EntityFrameworkCore;

namespace E_Commerce.Api.Extentions
{
    public static class WebApplicationRegister
    {
        //Has Two Method For Migration Data + Seeding 
        public static WebApplication MigrateDatabase(this WebApplication app)
        {
            using var Scope = app.Services.CreateScope();
            var dbContext = Scope.ServiceProvider.GetService<StoreDBContext>();
            var PendingMigration = dbContext.Database.GetPendingMigrations().Any();
            if (PendingMigration)
            {
                dbContext.Database.Migrate();
            }
            return app;
        }
        public static WebApplication SeedData(this WebApplication app)
        {
            using var Scope = app.Services.CreateScope();
            var DataIntializer = Scope.ServiceProvider.GetRequiredService<IDataSeeding>();
            DataIntializer.Intialize();
            return app;
        }
    }
}
