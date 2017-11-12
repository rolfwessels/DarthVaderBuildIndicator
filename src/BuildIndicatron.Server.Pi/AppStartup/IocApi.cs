using System.Web.Http.Dependencies;
using Autofac;
using Autofac.Integration.WebApi;
using MainSolutionTemplate.Api.WebApi.Controllers;

namespace MainSolutionTemplate.Api.AppStartup
{
    public class IocApi 
	{
		private static bool _isInitialized;
		private static readonly object _locker = new object();
		private static IocApi _instance;
		private readonly IContainer _container;

	    public IocApi()
		{
		    
	        var builder = new ContainerBuilder();
		    SetupTools(builder);
			WebApi(builder);
			_container = builder.Build();
		    
		}



        private void SetupTools(ContainerBuilder builder)
		{
            
		}

		#region Instance

		public static IocApi Instance
		{
			get
			{
				if (_isInitialized) return _instance;
				lock (_locker)
				{
					if (!_isInitialized)
					{
						_instance = new IocApi();
						_isInitialized = true;
					}
				}
				return _instance;
			}
		}

		public IContainer Container
		{
			get { return _container; }
		}


		public T Resolve<T>()
		{
			return _container.Resolve<T>();
		}

		#endregion

		

		#region Private Methods

		private void WebApi(ContainerBuilder builder)
		{
			builder.RegisterApiControllers(typeof(ProjectController).Assembly);
			builder.Register(t => new AutofacWebApiDependencyResolver(_container)).As<IDependencyResolver>();
		}


		#endregion
	}
}