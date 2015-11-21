using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Builder;
using Microsoft.AspNet.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Ignite.Repositories.Infrastructure;
using Ignite.Repositories.Core;
using Ignite.Services.Infrastructure;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Ignite.Services.Core;
using Swashbuckle.Swagger;
using Ignite.Repositories;
using Microsoft.AspNet.Hosting;
using Microsoft.Data.Entity;
using Microsoft.Extensions.PlatformAbstractions;

namespace Ignite.Web
{
    public class Startup
    {
        private readonly IConfigurationBuilder configurationBuilder;
        public Startup(IHostingEnvironment env, IApplicationEnvironment appEnv)
        {
            configurationBuilder = new ConfigurationBuilder();
            configurationBuilder.SetBasePath(appEnv.ApplicationBasePath);
            configurationBuilder
                .AddJsonFile("config.json")
                .AddJsonFile($"config.{env.EnvironmentName}.json", optional: true);
        }

        // For more information on how to configure your application, visit http://go.microsoft.com/fwlink/?LinkID=398940
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.AddEntityFramework()
                .AddInMemoryDatabase()
                .AddDbContext<DatabaseContext>(
                    options => options.UseInMemoryDatabase());

            services.AddMvc();

            services.AddSwagger();
            services.ConfigureSwaggerDocument(options =>
            {
                options.IgnoreObsoleteActions = true;
                options.SingleApiVersion(new Info
                {
                    Version = "v1",
                    Title = "Ignite API",
                    Description = "The Ignite API",
                    TermsOfService = "",
                    Contact = new Contact
                    {
                        Name = "Oconics Pty Ltd",
                        Email = "jonathan.ruckert@oconics.com",
                        Url = "http://www.oconics.com"
                    },
                    License = new License { Name = "License Terms Here...", Url = "http://www.oconics.com" }
                });
            });
            services.ConfigureSwaggerSchema(options =>
            {
                options.DescribeAllEnumsAsStrings = true;
            });

            var serviceProvider = ConfigureDependencyInjection(services);
            return serviceProvider;
        }

        public IServiceProvider ConfigureDependencyInjection(IServiceCollection services)
        {
            services.AddScoped<IDatabaseContext>(provider => provider.GetService<DatabaseContext>());
            services.AddScoped<IDbSession, DbSession>();
            services.TryAdd(ServiceDescriptor.Scoped(typeof(IEntityRepository<>), typeof(EntityRepository<>)));
            services.TryAdd(ServiceDescriptor.Scoped(typeof(IBaseService<>), typeof(BaseService<>)));

            return services.BuildServiceProvider();
        }

        public void ConfigureDevelopment(IApplicationBuilder app, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(minLevel: LogLevel.Verbose);
            loggerFactory.AddDebug();
            app.UseDeveloperExceptionPage();
            app.UseRuntimeInfoPage();
            Configure(app, loggerFactory);
        }

        public void ConfigureStaging(IApplicationBuilder app, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(minLevel: LogLevel.Warning);
            Configure(app, loggerFactory);
        }

        public void ConfigureProduction(IApplicationBuilder app, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(minLevel: LogLevel.Error);
            Configure(app, loggerFactory);
        }

        public void Configure(IApplicationBuilder app, ILoggerFactory loggerFactory)
        {
            // Add the platform handler to the request pipeline.
            app.UseIISPlatformHandler();

            app.UseStaticFiles();

            app.UseMvc();

            app.UseSwagger();
            app.UseSwaggerUi();

            app.Run(async context =>
            {
                context.Response.Redirect("/swagger/ui/index.html");
            });
        }
    }
}
