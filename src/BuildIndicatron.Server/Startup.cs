using System;
using Autofac.Extensions.DependencyInjection;
using BuildIndicatron.Server.Api.Controllers;
using BuildIndicatron.Server.Properties;
using BuildIndicatron.Server.Setup;
using CoreDocker.Api.AppStartup;
using CoreDocker.Api.Swagger;
using CoreDocker.Api.WebApi;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;

namespace CoreDocker.Api
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            Configuration = ReadAppSettings(env);
            BuildIndicatron.Core.Properties.Settings.Initialize(Configuration);
            Settings.Initialize(Configuration);
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            BootStrap.Initialize(services);
            services.AddMvc(options => WebApiSetup.Setup(options));
            SwaggerSetup.Setup(services);

            return new AutofacServiceProvider(IocContainer.Instance);
        }


        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(Configuration)
                .CreateLogger();

            //LogManager.SetLogger(loggerFactory);

            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();
            loggerFactory.AddSerilog();

            app.UseMvc();
            SwaggerSetup.AddUi(app);
            SimpleFileServer.Initialize(app);
        }

        #region Private Methods

        private IConfigurationRoot ReadAppSettings(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", true, true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", true);

            PingController.Env = env.EnvironmentName;
            if (env.IsEnvironment("Development"))
            {
                // This will push telemetry data through Application Insights pipeline faster, allowing you to view results immediately.
//                builder.AddApplicationInsightsSettings(true);
            }


            builder.AddEnvironmentVariables();
            return builder.Build();
        }

        #endregion
    }
}