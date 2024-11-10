using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;
using Talabat.Core.Contracts;
using Talabat.Core.Entities;
using Talabat.Core.Entities.Products;
using Talabat.Repository.Data;
using Talabat.Repository.Repositories;

namespace Talabat.Repository
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddRepositoryServices(this IServiceCollection services, IConfiguration configuration)
        {

            services.AddDbContext<StoreContext>(options => 
            
            options.UseLazyLoadingProxies()
                   .UseSqlServer(configuration.GetConnectionString("DefaultConnection"))
            
            );

            services.AddSingleton<IConnectionMultiplexer>( _ => 
            {
                var connection = configuration.GetConnectionString("RedisConnection");

                return ConnectionMultiplexer.Connect(connection);
            });

            services.AddScoped(typeof(IBasketRepository),typeof(BasketRepository));

            services.AddScoped(typeof(IGenericRepository<>),typeof(GenericRepository<>));
            return services;
        }
    }
}
