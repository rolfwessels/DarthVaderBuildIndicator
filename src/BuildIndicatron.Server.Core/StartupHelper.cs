using System;
using System.IO;
using System.Reflection;
using Autofac.Extensions.DependencyInjection;
using BuildIndicatron.Core.Helpers;
using BuildIndicatron.Core.Properties;
using BuildIndicatron.Server.Core.AppStartup;
using BuildIndicatron.Server.Core.Setup;
using BuildIndicatron.Server.Core.Swagger;
using BuildIndicatron.Server.Core.WebApi;
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
            var file = Path.Combine(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location) ?? "",
                loggingsettingsXml);
            var configFile = new FileInfo(file);
            XmlConfigurator.Configure(logRepository, configFile);
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