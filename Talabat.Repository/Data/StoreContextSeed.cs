using System.Text.Json;
using Talabat.Core.Domain.Entities._Common;
using Talabat.Shared.Exceptions;

namespace Talabat.Infrastructure.Persistence.Data
{
    public class StoreContextSeed
    {
        private readonly StoreContext _dbContext;

        public StoreContextSeed(StoreContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task UploadDataSeeds<TEntity,TKey>(string fileName) where TEntity : BaseAuditableEntity<TKey> where TKey : IEquatable<TKey>
        {
            if (!_dbContext.Set<TEntity>().Any())
            {
                var filePath = Path.Combine("../Talabat.Repository", "Data","DataSeeds", fileName);

                if (File.Exists(filePath))
                {
                    var SerializedData = await File.ReadAllTextAsync(filePath);
                    var data = JsonSerializer.Deserialize<List<TEntity>>(SerializedData);

                    if (data?.Count > 0)
                    {
                        await _dbContext.Set<TEntity>().AddRangeAsync(data);

                        await _dbContext.SaveChangesAsync();
                    }

                }
                else
                {
                    throw new NotFoundException($"Incorrect path found < {Path.GetFullPath(filePath)} >");
                }
            }

        }

    }
}
