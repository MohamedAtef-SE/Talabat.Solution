using Talabat.Core.Entities.Products;
using Talabat.Repository.Data;

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

                // Update-Database if there is Any Migration pending found.
                await contextSeed.UpdateDatabase(); 
                // Uplaod DataSeeds after Updating Database.
                await contextSeed.UploadDataSeeds<ProductCategory>("categories.json");
                await contextSeed.UploadDataSeeds<ProductBrand>("brands.json");
                await contextSeed.UploadDataSeeds<Product>("products.json");
            }
            catch (Exception ex)
            {
                var loggerFactory = service.GetRequiredService<ILoggerFactory>();

                var logger = loggerFactory.CreateLogger(typeof(InitializationExtention));

                logger.LogError(ex,ex.Message);
            }
        }
    }
}
