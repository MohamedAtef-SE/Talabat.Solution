using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;
using Talabat.Core.Domain.Contracts;
using Talabat.Infrastructure.Persistence.Data;
using Talabat.Infrastructure.Persistence.Data.Interceptors;
using Talabat.Infrastructure.Persistence.Identity;
using Talabat.Infrastructure.Persistence.Repositories;
using Talabat.Infrastructure.Redis.Basket;
using Talabat.Repository.Data;

namespace Talabat.Infrastructure.Persistence
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddRepositoryServices(this IServiceCollection services, IConfiguration configuration)
        {

            services.AddScoped<AuditableInterceptor>();
            services.AddDbContext<StoreContext>((provider,options) =>
            
            options.UseLazyLoadingProxies()
                   .UseSqlServer(configuration.GetConnectionString("DefaultConnection"))
                   .AddInterceptors(provider.GetRequiredService<AuditableInterceptor>())

            );

            services.AddDbContext<AppIdentityDbContext>(options =>

            options.UseSqlServer(configuration.GetConnectionString("IdentityConnection"))

            );

            services.AddSingleton<IConnectionMultiplexer>(_ =>
            {
                var connection = configuration.GetConnectionString("RedisConnection");
                if (connection is null) throw new Exception("Check Redis-server...");
                return ConnectionMultiplexer.Connect(connection!);
            });

            services.AddScoped(typeof(IBasketRepository), typeof(BasketRepository));

            services.AddScoped<StoreContextSeed>();
            services.AddScoped<AppIdentitySeed>();

            return services;
        }
    }
}
