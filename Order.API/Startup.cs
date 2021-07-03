using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Order.API.Common.Converters;
using Order.API.Interfaces;
using Order.API.Repositories;

namespace Order.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Register the Order Respository service
            services.AddScoped<IOrderRepository, OrderRepository>();

            services.AddApiVersioning(options => {
                // Will provide the different api version which is available for the client
                options.ReportApiVersions = true;
                // This configuration will allow the api to automaticaly take api_version=1.0 in case it was not specified
                options.AssumeDefaultVersionWhenUnspecified = true;
                // Setting default version of API to 1.0 
                options.DefaultApiVersion = ApiVersion.Default;
            });

            services.AddControllers()
                    .AddJsonOptions(options =>
                        {
                            options.JsonSerializerOptions.Converters.Add(new DateTimeConverter());
                        }
                    );

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Order API", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env )
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage(); 
            }
            else
            {
                // Route to the exception handler for prod environmenr
                app.UseExceptionHandler("/error");
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Order API v1");
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }

    
}
