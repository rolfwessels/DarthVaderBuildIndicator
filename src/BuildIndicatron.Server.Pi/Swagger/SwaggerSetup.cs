using System;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Web.Http;
using System.Web.Http.Description;
using MainSolutionTemplate.Api.WebApi.Controllers;
using Swashbuckle.Application;
using Swashbuckle.Swagger;
using BuildIndicatron.Core;

namespace MainSolutionTemplate.Api.Swagger
{
    public class SwaggerSetup
    {
        private static bool _isInitialized;
        private static readonly object _locker = new object();
        private static SwaggerSetup _instance;

        protected SwaggerSetup(HttpConfiguration configuration)
        {
            string version = GetVersion();

            configuration.EnableSwagger(c =>
                {
                    c.SingleApiVersion(version, "MainSolutionTemplate API")
                        ;
                
                c.OperationFilter<AddAuthorizationResponseCodes>();
            }).EnableSwaggerUi();

        }

        #region Private Methods

        private static string GetVersion()
        {
            return typeof (ProjectController).Assembly.GetName().Version.ToString().Split('.').Take(3).StringJoin(".");
        }

        #endregion

        #region Instance

        public static void Initialize(HttpConfiguration configuration)
        {
            if (_isInitialized) return;
            lock (_locker)
            {
                if (!_isInitialized)
                {
                    _instance = new SwaggerSetup(configuration);
                    _isInitialized = true;
                }
            }
        }

        #endregion


        public class AddAuthorizationResponseCodes : IOperationFilter
        {
            #region IOperationFilter Members

            public void Apply(Operation operation, SchemaRegistry dataTypeRegistry, ApiDescription apiDescription)
            {
                if (apiDescription.ActionDescriptor.GetFilters().OfType<AuthorizeAttribute>().Any())
                {
                    operation.responses.Add(ToIntDisplayValue(HttpStatusCode.Unauthorized), new Response
                        {
                            description = "Authentication required"    
                        });

                }
            }

            private static string ToIntDisplayValue(HttpStatusCode httpStatusCode)
            {
                return ((int)httpStatusCode).ToString(CultureInfo.InvariantCulture);
            }

            #endregion   
        }
    }
}