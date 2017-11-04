using System;
using System.IO;
using System.Reflection;
using Autofac.Extensions.DependencyInjection;
using BuildIndicatron.Server.Api;
using BuildIndicatron.Server.Api.Controllers;
using BuildIndicatron.Server.Properties;
using BuildIndicatron.Server.Setup;
using log4net;
using log4net.Config;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace BuildIndicatron.Server
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            Configuration = ReadAppSettings(env);
            Settings.Initialize(Configuration);
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            //IocApi.Populate(services);
            BootStrap.Initialize(services);
            services.AddMvc(options => WebApiSetup.Setup(options));
            SwaggerSetup.Setup(services);

            return new AutofacServiceProvider(IocContainer.Instance);
        }


        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            var logRepository = LogManager.GetRepository(Assembly.GetEntryAssembly());
            XmlConfigurator.Configure(logRepository, new FileInfo("log4net.config"));
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
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", true)
                .AddEnvironmentVariables();
            PingController.Env = env.EnvironmentName;
            if (env.IsEnvironment("Development"))
            {
                // This will push telemetry data through Application Insights pipeline faster, allowing you to view results immediately.
//                builder.AddApplicationInsightsSettings(true);
            }
            
            return builder.Build();
        }

        #endregion
    }

   
}
