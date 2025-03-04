using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.FileProviders;
using Newtonsoft.Json;
using Talabat.APIs.Extensions;
using Talabat.APIs.Middlewares;
using Talabat.APIs.Services;
using Talabat.Core.Application;
using Talabat.Core.Application.Abstractions.Services;
using Talabat.Infrastructure;
using Talabat.Infrastructure.Persistence;
using Talabat.Shared.Errors;

namespace Talabat.APIs
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            #region ConfigureServices

            // Add services to the container.

            builder.Services.AddControllers()
                 .AddNewtonsoftJson(options =>
                 {
                     options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                 })
                            .ConfigureApiBehaviorOptions(O =>
            {
                O.SuppressModelStateInvalidFilter = false; // Default
                O.InvalidModelStateResponseFactory = actionContext =>
                {
                    // ModelState is a Dic [KeyValuePear]
                    // Key Name Of Param

                    var errors = actionContext.ModelState.Where(param => param.Value!.Errors.Count > 0)
                                                    .SelectMany(param => param.Value!.Errors)
                                                    .Select(E => E.ErrorMessage)
                                                    .ToList();

                    var apiValidationResponse = new ApiValidationResponse(400, null) { Errors = errors };
                    return new BadRequestObjectResult(apiValidationResponse);

                };

            }).AddApplicationPart(typeof(Controllers.AssemblyInformation).Assembly);

            builder.Services.AddCors(CorsOptions =>
            {
                CorsOptions.AddPolicy("TalabatPolicy", configurePolicy =>
                {
                    configurePolicy.AllowAnyHeader();
                    configurePolicy.AllowAnyMethod();
                    configurePolicy.WithOrigins(builder.Configuration["FrontEndURLs:baseURL"]!);
                });
            });

            #region Swagger

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            #endregion

            builder.Services.AddHttpContextAccessor();
            builder.Services.AddScoped<ILoggedInUserService, LoggedInUserService>();


            #region AddCustomServices

            builder.Services.AddRepositoryServices(builder.Configuration);

            builder.Services.AddIdentityServices(builder.Configuration);

            builder.Services.AddApplicationServices(builder.Configuration);

            builder.Services.AddInfrastructureServices();

            #endregion

            #endregion


            var app = builder.Build();

            #region Kestrell

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseMiddleware<CustomExceptionMiddleware>();
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseStatusCodePagesWithReExecute("/errors/{0}");
            app.UseHttpsRedirection();
            app.UseCors("TalabatPolicy");
            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();
           
            await app.UpdateDBAsync(); // Update-Database

            #endregion
            var dashboardWWWRoot = Path.Combine(Directory.GetCurrentDirectory(),"..", "Dashboard","wwwroot");
            app.UseStaticFiles(new StaticFileOptions()
            {
                FileProvider = new PhysicalFileProvider(dashboardWWWRoot),
                RequestPath = "/static"
            });
            app.Run();
        }
    }
}
