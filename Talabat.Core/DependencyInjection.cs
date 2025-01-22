using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Talabat.Core.Application.Abstractions.Services;
using Talabat.Core.Application.Mapping;
using Talabat.Core.Application.Services;
using Talabat.Core.Application.Services.Auth;
using Talabat.Core.Application.Services.Basket;
using Talabat.Core.Application.Services.Orders;
using Talabat.Core.Application.Services.Payments;
using Talabat.Core.Application.Services.Products;
using Talabat.Core.Application.UnitOfWorks;
using Talabat.Core.Domain.Contracts;

namespace Talabat.Core.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAutoMapper(typeof(MappingProfile));

            services.Configure<JwtSettings>(configuration.GetSection("JwtSettings"));

            services.AddScoped<IUnitOfWork, UnitOfWork>();

            services.AddScoped<IServiceManager, ServiceManager>();

            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<Func<IAuthService>>((serviceProvider) =>
            {
                return () => serviceProvider.GetRequiredService<IAuthService>();
            });

            services.AddScoped<IOrderService, OrderServices>();
            services.AddScoped<Func<IOrderService>>((serviceProvider) =>
            {
                return () => serviceProvider.GetRequiredService<IOrderService>();
            });

            services.AddScoped<IBasketService, BasketServices>();
            services.AddScoped<Func<IBasketService>>((serviceProvider) =>
            {
                return () => serviceProvider.GetRequiredService<IBasketService>();
            });

            services.AddScoped<IPaymentService, PaymentService>();
            services.AddScoped<Func<IPaymentService>>(provider =>
            {
                return () => provider.GetRequiredService<IPaymentService>();
            });


            services.AddScoped<IProductService, ProductServices>();
            services.AddScoped<Func<IProductService>>((provider) =>
            {
                return () => provider.GetRequiredService<IProductService>();
            });



            return services;
        }
    }
}
