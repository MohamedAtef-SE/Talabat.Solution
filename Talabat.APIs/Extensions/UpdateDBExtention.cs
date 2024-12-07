using Microsoft.EntityFrameworkCore;
using Talabat.Infrastructure.Persistence.Data;
using Talabat.Infrastructure.Persistence.Identity;

namespace Talabat.APIs.Extensions
{
    public static class UpdateDBExtention
    {
        public async static Task UpdateDBAsync(this WebApplication app)
        {

            using var scope = app.Services.CreateAsyncScope(); // Select all Services with Lifetime Scoped Only.

            var service = scope.ServiceProvider; // Services itself

            try
            {
                var dbContext = service.GetRequiredService<StoreContext>(); // ASK CLR For Creating Object from DbContext Explicitly
                var identityDbContext = service.GetRequiredService<AppIdentityDbContext>();
                
               
                // Update-Database if there is Any Migration pending found.
                if (dbContext.Database.GetPendingMigrations().Any())
                {
                    await dbContext.Database.MigrateAsync();
                }

                if (identityDbContext.Database.GetPendingMigrations().Any())
                {
                    await identityDbContext.Database.MigrateAsync();
                }

            }
            catch (Exception ex)
            {
                var loggerFactory = service.GetRequiredService<ILoggerFactory>();

                var logger = loggerFactory.CreateLogger(typeof(UpdateDBExtention));

                logger.LogError(ex, ex.Message);
            }
        }
    }
}
