using System.Reflection;
using Autofac;
using Autofac.Integration.WebApi;
using BuildIndicatron.Core.Helpers;
using BuildIndicatron.Core.Processes;
using BuildIndicatron.Server.Fakes;
using BuildIndicatron.Server.Properties;

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

		#region Private Methods

		private static void SetupTools(ContainerBuilder builder)
		{
			builder.RegisterType<DownloadToFile>()
			       .As<IDownloadToFile>()
			       .WithParameter("tempPath", Settings.Default.SpeachTempFileLocation);
			builder.RegisterType<Stage>().As<IStage>();
			builder.RegisterType<SequencesFactory>();
		}

		private static void RegisterControllers(ContainerBuilder builder)
		{
			builder.RegisterApiControllers(Assembly.GetExecutingAssembly());
		}

		private static void SetupConcrete(ContainerBuilder builder)
		{
			builder.RegisterType<Mp3Player>().As<IMp3Player>();
			builder.RegisterType<GoogleTextToSpeach>().As<ITextToSpeech>();
			builder.Register(t => new VoiceEnhancer(@"resources/text2speach/Star-Wars-1391.mp3", "speed 0.65 echo 0.8 0.88 6.0 0.4"))
			       .As<IVoiceEnhancer>();
		}

		private static void SetupFakes(ContainerBuilder builder)
		{
			builder.RegisterType<FakePlayer>().As<IMp3Player>();
			builder.RegisterType<FakeTextToSpeech>().As<ITextToSpeech>();
			builder.RegisterType<FakePlayer>().As<IVoiceEnhancer>();
		}

		#endregion
	}
}