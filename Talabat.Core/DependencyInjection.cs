using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Talabat.Core.Application.Abstractions.Services;
using Talabat.Core.Application.Mapping;
using Talabat.Core.Application.Services.Auth;

namespace Talabat.Core.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services,IConfiguration configuration)
        {
            services.AddScoped(typeof(IAuthService),typeof(AuthService));

            services.AddHttpContextAccessor();

            services.Configure<JwtSettings>(configuration.GetSection("JwtSettings"));

            services.AddAutoMapper(typeof(MappingProfile));

            return services;
        }
    }
}
