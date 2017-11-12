using System;
using System.IO;
using System.Reflection;
using Autofac.Extensions.DependencyInjection;
using BuildIndicatron.Server.Api.Controllers;
using BuildIndicatron.Server.Properties;
using BuildIndicatron.Server.Setup;
using CoreDocker.Api.AppStartup;
using CoreDocker.Api.Swagger;
using CoreDocker.Api.WebApi;
using log4net;
using log4net.Config;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;

namespace BuildIndicatron.Server
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var logRepository = LogManager.GetRepository(Assembly.GetEntryAssembly());
            XmlConfigurator.Configure(logRepository, new FileInfo("loggingSettings.xml"));
            Configuration = ReadAppSettings(env);
            BuildIndicatron.Core.Properties.Settings.Initialize(Configuration);
            Settings.Initialize(Configuration);
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            IocContainer.Initialize(services);
            services.AddMvc(WebApiSetup.Setup);
            SwaggerSetup.Setup(services);
            var autofacServiceProvider = new AutofacServiceProvider(IocContainer.Instance.Container);
            BootStrap.Initialize(services);
            return autofacServiceProvider;
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