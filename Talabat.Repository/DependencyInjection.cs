using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;
using Talabat.Core.Domain.Contracts;
using Talabat.Infrastructure.Persistence.Data;
using Talabat.Infrastructure.Persistence.Identity;
using Talabat.Infrastructure.Persistence.Repositories;
using Talabat.Repository.Data;

namespace Talabat.Infrastructure.Persistence
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddRepositoryServices(this IServiceCollection services, IConfiguration configuration)
        {

            services.AddDbContext<StoreContext>(options =>

            options.UseLazyLoadingProxies()
                   .UseSqlServer(configuration.GetConnectionString("DefaultConnection"))

            );

            services.AddDbContext<AppIdentityDbContext>(options =>

            options.UseSqlServer(configuration.GetConnectionString("IdentityConnection"))

            );

            services.AddSingleton<IConnectionMultiplexer>(_ =>
            {
                var connection = configuration.GetConnectionString("RedisConnection");

                return ConnectionMultiplexer.Connect(connection!);
            });

            services.AddScoped(typeof(IBasketRepository), typeof(BasketRepository));

            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

            services.AddScoped<StoreContextSeed>();
            services.AddScoped<AppIdentitySeed>();

            return services;
        }
    }
}
