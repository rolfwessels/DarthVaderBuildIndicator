using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Autofac;
using BuildIndicatron.Core.Api;
using BuildIndicatron.Core.Chat;
using BuildIndicatron.Core.Helpers;
using BuildIndicatron.Core.Processes;
using BuildIndicatron.Server.Properties;
using BuildIndicatron.Core.Settings;
using BuildIndicatron.Server.Fakes;
using BuildIndicatron.Shared.Enums;

namespace BuildIndicatron.Server.Setup
{
	public static class IocContainer
	{
		private static bool _isInitialized;
		private static readonly object _locker = new object();
		private static IContainer _container;

		public static IContainer Instance
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
					    var container = builder.Build();
                        
					    return _container = container;
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
			builder.RegisterType<Stage>().As<IStage>().SingleInstance();
            
            builder.RegisterAssemblyTypes(typeof(IFactory).Assembly)
                   .Where(t => t.GetInterfaces()
                    .Any(i => i.IsAssignableFrom(typeof(IReposonseFlow))))
                   .AsSelf().SingleInstance();
			builder.RegisterType<SequencesFactory>();
            builder.RegisterType<DeployCoreContext>();
            builder.RegisterType<ChatBot>().As<IChatBot>();
            builder.RegisterType<JenkinsFactory>().As<IJenkinsFactory>();
            builder.RegisterType<MonitorJenkins>().As<IMonitorJenkins>();
            builder.RegisterType<HttpLookup>().As<IHttpLookup>();
            builder.RegisterType<VolumeSetter>().As<IVolumeSetter>();
            builder.Register(context => new SettingsManager(SettingFile())).As<ISettingsManager>().SingleInstance();
            builder.Register(context => JenkensApi.GetJenkins(context.Resolve<ISettingsManager>())).As<IJenkensApi>();
            builder.Register(context => new AutofacInjector(_container)).As<IFactory>();
			builder.RegisterType<SequencePlayer>().As<ISequencePlayer>();
			builder.Register((t) => new SoundFilePicker(Settings.Default.SoundFileLocation)).As<ISoundFilePicker>();
		}

	    private static string SettingFile()
	    {
            return Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)??"","setting.json");
	    }

	    private static void RegisterControllers(ContainerBuilder builder)
		{
			//builder.RegisterApiControllers(Assembly.GetExecutingAssembly());
		}

		private static void SetupConcrete(ContainerBuilder builder)
		{
			builder.RegisterType<Mp3Player>().As<IMp3Player>();
			builder.Register(context => new VoiceRss(context.Resolve<IDownloadToFile>(),context.Resolve<IMp3Player>(), context.Resolve<ISettingsManager>().Get("voice_rss_key")) ).As<ITextToSpeech>();
//            builder.Register(t => new VoiceEnhancer(@"resources/sounds/Funny/R2D2c.wav", "speed 1.3 echo 0.8 0.88 6.0 0.4"))
            builder.Register(t => new VoiceEnhancer(t.Resolve<ISettingsManager>().Get("voice_bg_file", @"resources/sounds/Funny/R2D2c.wav"), "speed 1"))
			       .As<IVoiceEnhancer>();
			builder.RegisterType<FakePinManager>()
				.WithParameter("configuration", ConfigurationRobot)
				.As<IPinManager>().SingleInstance();
		}								  

		private static void SetupFakes(ContainerBuilder builder)
		{
			builder.RegisterType<FakePlayer>().As<IMp3Player>();
			builder.RegisterType<FakeTextToSpeech>().As<ITextToSpeech>();
			builder.RegisterType<FakePlayer>().As<IVoiceEnhancer>();
			builder.RegisterType<FakePinManager>()
				.As<IPinManager>();
		}

		public static GpioConfiguration ConfigurationRobot
		{
			get
			{
				return new GpioConfiguration(
					new GpioConfiguration.Target(PinName.SecondaryLightRed, GpIO.GPIO7, true),
					new GpioConfiguration.Target(PinName.SecondaryLightGreen, GpIO.GPIO8, true),
//					new GpioConfiguration.Target(PinName.SecondaryLightBlue, GpIO.GPIO24, true),
					new GpioConfiguration.Target(PinName.MainLightRed, GpIO.GPIO11, true),
					new GpioConfiguration.Target(PinName.MainLightGreen, GpIO.GPIO25, true),
					new GpioConfiguration.Target(PinName.MainLightBlue, GpIO.GPIO9, true),
					new GpioConfiguration.Target(PinName.Spinner, GpIO.GPIO22),
					new GpioConfiguration.Target(PinName.Fire, GpIO.GPIO10)
					)
					{
						Buttons =
							new List<GpioConfiguration.Button>() {new GpioConfiguration.Button() {Pin = GpIO.GPIO24, IsToggle = false}}
					};
			}
		}

		public static GpioConfiguration ConfigurationDarth2
		{
			get
			{
				return new GpioConfiguration(
					new GpioConfiguration.Target(PinName.SecondaryLightRed, GpIO.GPIO7, true),
					new GpioConfiguration.Target(PinName.SecondaryLightGreen, GpIO.GPIO8, true),
					new GpioConfiguration.Target(PinName.MainLightRed, GpIO.GPIO11, true),
					new GpioConfiguration.Target(PinName.MainLightGreen, GpIO.GPIO25, true),
					new GpioConfiguration.Target(PinName.MainLightBlue, GpIO.GPIO9, true)
					)
				{
					Buttons =
						new List<GpioConfiguration.Button>() { new GpioConfiguration.Button() { Pin = GpIO.GPIO24, IsToggle = false } }
				};
			}
		}

		public static GpioConfiguration ConfigurationDarth
		{
			get
			{


				return new GpioConfiguration(
					new GpioConfiguration.Target(PinName.SecondaryLightRed, GpIO.GPIO7, true),
					new GpioConfiguration.Target(PinName.SecondaryLightGreen, GpIO.GPIO8, true),
					new GpioConfiguration.Target(PinName.MainLightRed, GpIO.GPIO11, true),
					new GpioConfiguration.Target(PinName.MainLightGreen, GpIO.GPIO25, true),
					new GpioConfiguration.Target(PinName.MainLightBlue, GpIO.GPIO9, true),
					new GpioConfiguration.Target(PinName.Spinner, GpIO.GPIO22),
					new GpioConfiguration.Target(PinName.Fire, GpIO.GPIO10)
					)
					{
						Buttons =
							new List<GpioConfiguration.Button>() {new GpioConfiguration.Button() {Pin = GpIO.GPIO24, IsToggle = false}}
					};
			}
		}

		#endregion


	   
	}

    
}