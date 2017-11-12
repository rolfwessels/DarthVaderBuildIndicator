using System;
using BuildIndicatron.Server.Core;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BuildIndicatron.Server.Chip
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            StartupHelper.AddLog4NetSetup("loggingSettings.xml");
            Configuration = configuration;
            configuration.AddSettingsWrappers();
           
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            return services.AddBuildIndicatorServices();
        }
        

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            
            app.AddSwaggerAndStaticSite();
        }
    }
}
