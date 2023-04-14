using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Azure.Storage.Blobs;
using CarWebsiteBackend.Configuration;
using CarWebsiteBackend.Controllers;
using CarWebsiteBackend.Data;
using CarWebsiteBackend.Interfaces;
using CarWebsiteBackend.Storage;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace CarWebsiteBackend
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            // add CORS policy to allow requests from any origin
            services.AddCors(options =>
            {
                options.AddPolicy("AllowAnyOrigin",
                    builder => builder.AllowAnyOrigin()
                                      .AllowAnyHeader()
                                      .AllowAnyMethod());
            });

            // configure your other services here
            // ...
        }

        public void Configure(IApplicationBuilder app)
        {
            app.UseRouting();

            app.UseCors("AllowAnyOrigin");

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
