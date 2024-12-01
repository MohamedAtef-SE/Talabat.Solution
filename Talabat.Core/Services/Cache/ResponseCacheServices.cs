using StackExchange.Redis;
using System.Text.Json;
using Talabat.Core.Application.Abstractions.Services;

namespace Talabat.Core.Application.Services.Cache
{
    public class ResponseCacheServices(IConnectionMultiplexer redis) : IResponseCacheService
    {
        private readonly IDatabase _database = redis.GetDatabase();
        public async Task CacheResponseAsync(string CasheKey, object Response, TimeSpan ExpireTime)
        {
            var jsonSerializerOptions = new JsonSerializerOptions()
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };

           var SerilizedResponse = JsonSerializer.Serialize(Response, jsonSerializerOptions);

            await _database.StringSetAsync(CasheKey, SerilizedResponse, ExpireTime);
        }
        
        public async Task<string?> GetCachedResponseAsync(string CacheKey)
        {
           return await _database.StringGetAsync(CacheKey);
        }
    }
}
