using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
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

            services.AddScoped(typeof(IGenericRepository<>),typeof(GenericRepository<>));
            return services;
        }
    }
}
