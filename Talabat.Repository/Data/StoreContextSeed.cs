using System.Text.Json;
using Talabat.Core.Application.Entities.Products;

namespace Talabat.Repository.Data
{
    public class StoreContextSeed
    {
        private readonly StoreContext _dbContext;

        public StoreContextSeed(StoreContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task UploadDataSeeds<TEntity>(string fileName) where TEntity : BaseEntity
        {
            if (!_dbContext.Set<TEntity>().Any())
            {
                var filePath = Path.Combine("../Talabat.Repository","Data","DataSeeds", fileName);

                if (File.Exists(filePath))
                {
                    var datasAsJSON = await File.ReadAllTextAsync(filePath);
                    var data = JsonSerializer.Deserialize<List<TEntity>>(datasAsJSON);

                    if (data?.Count > 0)
                    {
                        await _dbContext.Set<TEntity>().AddRangeAsync(data);

                        await _dbContext.SaveChangesAsync();
                    }

                }
            }

        }

    }
}
