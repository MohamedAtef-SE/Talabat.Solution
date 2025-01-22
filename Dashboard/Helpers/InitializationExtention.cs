using Dashboard.DAL.Data;
using Dashboard.DAL.Identity;
using Talabat.Core.Domain.Entities.Orders;
using Talabat.Core.Domain.Entities.Products;

namespace Dashboard.PL.Helpers
{
    public static class InitializationExtention
    {
        public async static Task InitializeAsync(this WebApplication app)
        {

            using var scope = app.Services.CreateAsyncScope(); // Select all Services with Lifetime Scoped Only.

            var service = scope.ServiceProvider; // Services itself

            try
            {
                var contextSeed = service.GetRequiredService<StoreContextSeed>();
                var identitySeed = service.GetRequiredService<AppIdentitySeed>();

                // Uplaod DataSeeds after Updating Database.
                await contextSeed.UploadDataSeeds<ProductCategory,string>("categories.json");
                await contextSeed.UploadDataSeeds<ProductBrand,string>("brands.json");
                await contextSeed.UploadDataSeeds<Product,string>("products.json");
                await contextSeed.UploadDataSeeds<DeliveryMethod,string>("deliveryMethods.json");

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
