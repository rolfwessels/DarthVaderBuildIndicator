using System.Reflection;
using Autofac;
using Autofac.Integration.WebApi;
using BuildIndicatron.Core.Processes;
using BuildIndicatron.Server.Controllers;
using BuildIndicatron.Server.Fakes;

namespace BuildIndicatron.Server.Setup
{
	public static class IocContainer
	{
		private static bool _isInitialized;
		private static readonly object _locker = new object();
		private static IContainer _container;

		public static IContainer Initialize
		{
			get
			{
				if (_container != null) return _container;
				lock (_locker)
				{
					if (!_isInitialized)
					{
						_isInitialized = true;
						var builder = new ContainerBuilder();
						SetupFakes(builder);
						RegisterControllers(builder);

						return _container = builder.Build();
					}
					return _container;
				}
			}
		}

		private static void RegisterControllers(ContainerBuilder builder)
		{
			builder.RegisterApiControllers(Assembly.GetExecutingAssembly());
//			builder.RegisterType<PingController>().InstancePerApiRequest();
//			builder.RegisterType<SoundPlayerController>().InstancePerApiRequest();
		}

		private static void SetupFakes(ContainerBuilder builder)
		{
			builder.RegisterType<FakePlayer>().As<IMp3Player>();
		}
	}
}