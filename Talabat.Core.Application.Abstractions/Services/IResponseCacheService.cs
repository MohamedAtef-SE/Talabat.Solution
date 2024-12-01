namespace Talabat.Core.Application.Abstractions.Services
{
    public interface IResponseCacheService
    {
        Task CacheResponseAsync(string CasheKey, object Response, TimeSpan ExpireTime);

        Task<string?> GetCachedResponseAsync(string CacheKey);
    }
}