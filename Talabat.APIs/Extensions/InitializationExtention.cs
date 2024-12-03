﻿using Microsoft.EntityFrameworkCore;
using Talabat.Core.Domain.Entities.Orders;
using Talabat.Core.Domain.Entities.Products;
using Talabat.Infrastructure.Persistence.Data;
using Talabat.Infrastructure.Persistence.Identity;

namespace Talabat.APIs.Extensions
{
    public static class InitializationExtention
    {
        public async static Task InitializeAsync(this WebApplication app)
        {

            using var scope = app.Services.CreateAsyncScope(); // Select all Services with Lifetime Scoped Only.

            var service = scope.ServiceProvider; // Services itself

            try
            {
                var dbContext = service.GetRequiredService<StoreContext>(); // ASK CLR For Creating Object from DbContext Explicitly
                var contextSeed = service.GetRequiredService<StoreContextSeed>();
                var identityDbContext = service.GetRequiredService<AppIdentityDbContext>();
                var identitySeed = service.GetRequiredService<AppIdentitySeed>();
                //var userManager = service.GetRequiredService<UserManager<ApplicationUser>>();


                // Update-Database if there is Any Migration pending found.
                if (dbContext.Database.GetPendingMigrations().Any())
                {
                    await dbContext.Database.MigrateAsync();
                }

                if (identityDbContext.Database.GetPendingMigrations().Any())
                {
                    await identityDbContext.Database.MigrateAsync();
                }

                // Uplaod DataSeeds after Updating Database.
                //await contextSeed.UploadDataSeeds<ProductCategory,string>("categories.json");
                //await contextSeed.UploadDataSeeds<ProductBrand,string>("brands.json");
                //await contextSeed.UploadDataSeeds<Product,string>("products.json");

                //await contextSeed.UploadDataSeeds<DeliveryMethod,string>("deliveryMethods.json");

                //Uplaod IdentitySeeds
                await identitySeed.UploadUsersAsync();

            }
            catch (Exception ex)
            {
                var loggerFactory = service.GetRequiredService<ILoggerFactory>();

                var logger = loggerFactory.CreateLogger(typeof(InitializationExtention));

                logger.LogError(ex, ex.Message);
            }
        }
    }
}
