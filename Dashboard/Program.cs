using Dashboard.DAL.Data;
using Dashboard.DAL.Identity;
using Dashboard.Middlewares;
using Dashboard.PL.Helpers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Route.Talabat.Dashboard.Helpers;
using System.Text;
using Talabat.APIs.Services;
using Talabat.Core.Application.Abstractions.Services;
using Talabat.Core.Application.Services.Auth;
using Talabat.Core.Application.UnitOfWorks;
using Talabat.Core.Domain.Contracts;
using Talabat.Core.Domain.Entities.Identity;
using Talabat.Infrastructure.Persistence.Data;
using Talabat.Infrastructure.Persistence.Data.Interceptors;
using Talabat.Infrastructure.Persistence.Identity;

namespace Dashboard
{
    public class Program
    {
        public async static Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews(options =>
            {
                // this recommended way but i will keep implement action-filter manually in each Action 
                // using [ValidateAntiforgeryToken] for readablity.
                // but this approach auto-generated it in each POST | PUT | Delete Action

                // options.Filters.Add(new AutoValidateAntiforgeryTokenAttribute());
            });

            // Configure Identity Package for Dashboard Project.
            builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
                            .AddEntityFrameworkStores<AppIdentityDbContext>();

            // Register DbContext in Dashboard project
            builder.Services.AddScoped<ILoggedInUserService, LoggedInUserService>();
            builder.Services.AddScoped<AuditableInterceptor>();

            builder.Services.AddDbContext<StoreContext>((provider, options) =>

            options.UseLazyLoadingProxies()
                   .UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
                   .AddInterceptors(provider.GetRequiredService<AuditableInterceptor>())

            );

            // Register IdentityDbContext in Dashboard project
            builder.Services.AddDbContext<AppIdentityDbContext>(options =>

            // Configure Entity Framework Core with In-Memory Database
            options.UseSqlServer(builder.Configuration.GetConnectionString("IdentityConnection"))

            );

            builder.Services.AddScoped<IAuthService, AuthService>();
            builder.Services.AddScoped(typeof(IUnitOfWork), typeof(UnitOfWork));
            builder.Services.AddAutoMapper(typeof(MapProfile));
            builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("JwtSettings"));
            builder.Services.AddScoped<StoreContextSeed>();
            builder.Services.AddScoped<AppIdentitySeed>();


            // Configure JWT Authentication
            var jwtSettings = builder.Configuration.GetSection("JwtSettings");
            var key = Encoding.UTF8.GetBytes(jwtSettings["key"]!);

            builder.Services.AddAuthentication(authenticationOptions =>
            {
                authenticationOptions.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                authenticationOptions.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                .AddJwtBearer(Options =>
                    {
                        Options.RequireHttpsMetadata = false; // Set to true in production
                        Options.SaveToken = true;
                        Options.TokenValidationParameters = new TokenValidationParameters()
                        {
                            ValidateIssuer = true,
                            ValidateIssuerSigningKey = true,
                            ValidateAudience = true,
                            ValidateLifetime = true,
                            ValidIssuer = jwtSettings["Issuer"]!,
                            ValidAudience = jwtSettings["Audience"],
                            IssuerSigningKey = new SymmetricSecurityKey(key),
                            ClockSkew = TimeSpan.Zero // Eliminate default clock skew
                        };
                    });

            // Add Authorization
            builder.Services.AddAuthorization();

            // Build application after sets all ConfigureServices
            var app = builder.Build();

            #region Configure the HTTP request pipeline

            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();

            
            app.UseStaticFiles();

            app.UseRouting();

            // Add JWTCookie Middleware to app
            app.UseMiddleware<JWTCookieMiddleware>(); //
            
            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Admin}/{action=Login}/{id?}");

            #endregion

            await app.InitializeAsync(); // Upload Initial Data Seeds

            app.Run();
        }
    }
}
