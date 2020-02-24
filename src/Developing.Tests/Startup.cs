using System;
using Developing.API.Authorization;
using Developing.API.Filters;
using Developing.API.Infrastructure.Database.DataModel;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Developing.Tests
{
    public sealed class Startup
    {
        public readonly IConfiguration _configuration;

        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ApiDbContext>(options =>
            {
                options.UseInMemoryDatabase(Guid.NewGuid().ToString());
            });

            services.AddControllers(options =>
            {
                options.Filters.Add(new RequestValidationFilter());
            })
            .AddApplicationPart(typeof(Developing.API.Program).Assembly)
            .AddNewtonsoftJson();

            services.AddDefaultCorsPolicy();

            services.AddSingleton<ApiDbContext>();
        }

        public void Configure(IApplicationBuilder app)
        {
            app.UseCors();
            app.UseRouting();
            app.UseEndpoints(endpoints => endpoints.MapControllers());
        }
    }
}
