using DinderMVC.Authentication;
using DinderMVC.Controllers;
using DinderMVC.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System;
using System.IO;
using System.Reflection;

namespace DinderMVC
{
#pragma warning disable CS1591
    public class Startup
    {
        public Startup(IConfiguration config)
        {
            configuration = config;
        }

        private static IConfiguration configuration;
        public static IConfiguration Configuration { get { return configuration; } }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {



            services.AddMvc(option => option.EnableEndpointRouting = false).SetCompatibilityVersion(CompatibilityVersion.Latest);


            // Add configuration for DbContext
            // Use connection string from appsettings.json file
            services.AddDbContext<DinderContext>(options =>
            {
                options.UseSqlServer(Configuration["AppSettings:ConnectionString"]);
            });

            // Set up dependency injection for controller's logger
            services.AddScoped<ILogger, Logger<UsersController>>();
            services.AddScoped<ILogger, Logger<GlobalMealsController>>();
            services.AddScoped<ILogger, Logger<PartiesController>>();
            services.AddScoped<ILogger, Logger<RootController>>();
            services.AddScoped<ILogger, Logger<TokenController>>();
            services.AddScoped<TokenValidationService>();

            services.AddAuthentication("BasicAuthentication").AddScheme<BasicAuthenticationOptions, BasicAuthenticationHandler>("BasicAuthentication", null);
            services.AddAuthentication("Bearer").AddScheme<BearerAuthenticationOptions, BearerAuthenticationHandler>("Bearer", null);

            // Register the Swagger generator, defining 1 or more Swagger documents
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo { Title = "Dinder API", Version = "v1" });

                // Get xml comments path
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);

                // Set xml path
                options.IncludeXmlComments(xmlPath);
            });


        }



        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
                app.UseDeveloperExceptionPage();

            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.), specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Dinder API V1.1");
            });

            app.UseAuthentication();
            app.UseAuthorization();


            app.UseMvc();

        }
    }
#pragma warning restore CS1591
}
