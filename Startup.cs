using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace CarWebsiteBackend
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
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
            app.UseCors("AllowAnyOrigin");

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
