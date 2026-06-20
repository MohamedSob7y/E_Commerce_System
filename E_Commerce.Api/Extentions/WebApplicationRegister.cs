using E_Commerce.Domain.Constracts;
using E_Commerce.Persistance.Data.DBContext;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace E_Commerce.Api.Extentions
{
    public static class WebApplicationRegister
    {
        //Has Two Method For Migration Data + Seeding 
        public static async Task<WebApplication> MigrateDatabaseAsync(this WebApplication app)
        {
           await using var Scope =app.Services.CreateAsyncScope();
            var dbContext = Scope.ServiceProvider.GetService<StoreDBContext>();
            var PendingMigration =await dbContext.Database.GetPendingMigrationsAsync();
            if (PendingMigration.Any())
            {
                dbContext.Database.Migrate();
            }
            return app;
        }
        public static async Task<WebApplication> SeedDataAsync(this WebApplication app)
        {
            using var Scope = app.Services.CreateScope();
            var DataIntializer = Scope.ServiceProvider.GetRequiredService<IDataSeeding>();
           await DataIntializer.IntializeAsync();
            return app;
        }
    }
}
