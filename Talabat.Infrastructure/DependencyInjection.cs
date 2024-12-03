using Microsoft.Extensions.DependencyInjection;
using Talabat.Core.Application.Abstractions.Services;
using Talabat.Infrastructure.Redis.Cache;

namespace Talabat.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services)
        {
            services.AddSingleton(typeof(IResponseCacheService), typeof(ResponseCacheServices));
            
            return services;
        }
    }
}
