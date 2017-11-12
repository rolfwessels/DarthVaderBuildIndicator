using System;
using System.IO;
using System.Reflection;
using Autofac.Extensions.DependencyInjection;
using BuildIndicatron.Core.Properties;
using BuildIndicatron.Server.Setup;
using CoreDocker.Api.AppStartup;
using CoreDocker.Api.Swagger;
using CoreDocker.Api.WebApi;
using log4net;
using log4net.Config;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BuildIndicatron.Server.Core
{
    public static class StartupHelper
    {
        public static void AddSettingsWrappers(this IConfiguration configuration)
        {
            Settings.Initialize(configuration);
            Properties.ServerSettings.Initialize(configuration);
        }

        public static void AddLog4NetSetup(string loggingsettingsXml)
        {
            var logRepository = LogManager.GetRepository(Assembly.GetEntryAssembly());
            XmlConfigurator.Configure(logRepository, new FileInfo(loggingsettingsXml));
        }

        public static IServiceProvider AddBuildIndicatorServices(this IServiceCollection services)
        {
            IocContainer.Initialize(services);
            services.AddMvc(WebApiSetup.Setup);
            SwaggerSetup.Setup(services);
            var autofacServiceProvider = new AutofacServiceProvider(IocContainer.Instance.Container);
            BootStrap.StartSlackBot();
            BootStrap.StartMonitoringJenking();
            return autofacServiceProvider;
        }

        
        public static void AddSwaggerAndStaticSite(this IApplicationBuilder app)
        {
            app.UseMvc();
            SwaggerSetup.AddUi(app);
            SimpleFileServer.Initialize(app);
        }
    }
}