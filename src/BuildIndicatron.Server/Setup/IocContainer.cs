using System;
using System.Reflection;
using Autofac;
using Autofac.Integration.WebApi;
using BuildIndicatron.Core.Helpers;
using BuildIndicatron.Core.Processes;
using BuildIndicatron.Server.Controllers;
using BuildIndicatron.Server.Fakes;
using BuildIndicatron.Server.Properties;
using log4net;

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
						if (PlatformHelper.IsLinux)
						{
							SetupConcrete(builder);
						}
						else
						{
							SetupFakes(builder);
						}
						SetupTools(builder);
						RegisterControllers(builder);

						return _container = builder.Build();
					}
					return _container;
				}
			}
		}

		private static void SetupTools(ContainerBuilder builder)
		{
			builder.RegisterType<DownloadToFile>().As<IDownloadToFile>().WithParameter("tempPath",Settings.Default.SpeachTempFileLocation);
		}
		private static void SetupConcrete(ContainerBuilder builder)
		{
			builder.RegisterType<Mp3Player>().As<IMp3Player>();
			builder.RegisterType<GoogleTextToSpeach>().As<ITextToSpeech>();
		}

		private static void RegisterControllers(ContainerBuilder builder)
		{
			builder.RegisterApiControllers(Assembly.GetExecutingAssembly());
		}

		private static void SetupFakes(ContainerBuilder builder)
		{
			builder.RegisterType<FakePlayer>().As<IMp3Player>();
			builder.RegisterType<FakeTextToSpeech>().As<ITextToSpeech>();
		}
	}

}