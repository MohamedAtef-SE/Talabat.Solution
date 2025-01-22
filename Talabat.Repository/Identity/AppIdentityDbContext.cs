using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using Talabat.Core.Domain.Entities.Identity;
using Talabat.Infrastructure.Persistence._Common;

namespace Talabat.Infrastructure.Persistence.Identity
{
    public class AppIdentityDbContext : IdentityDbContext<ApplicationUser>
    {
        public AppIdentityDbContext(DbContextOptions<AppIdentityDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            //builder.ApplyConfiguration(new ApplicationUserConfigurations());
            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly(),
                                                    type => type.GetCustomAttribute<DBContextTypeAttribute>()?.DBType == typeof(AppIdentityDbContext));
        }
    }
}
