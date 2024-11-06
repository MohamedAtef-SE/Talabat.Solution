using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.APIs.Controllers.Mapping;

namespace Talabat.APIs.Controllers
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddControllersServices(this IServiceCollection services)
        {
            services.AddAutoMapper(typeof(MappingProfile));

            return services;
        }
    }
}
