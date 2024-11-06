using Talabat.APIs.Controllers;
using Talabat.APIs.Extensions;
using Talabat.Repository;
using Talabat.Repository.Data;

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
                            .AddApplicationPart(typeof(Controllers.AssemblyInformation).Assembly);

            #region Swagger

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            #endregion

            #region AddCustomServices

            builder.Services.AddRepositoryServices(builder.Configuration);

            builder.Services.AddControllersServices();

            #endregion

            builder.Services.AddScoped<StoreContextSeed>();
            
            #endregion


            var app = builder.Build();

            #region Kestrell

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            #region Update-Database

            await app.InitializeAsync();

            #endregion

            #endregion
            app.UseStaticFiles();
            app.Run();
        }
    }
}
