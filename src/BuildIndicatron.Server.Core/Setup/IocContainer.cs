using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using BuildIndicatron.Core.Api;
using BuildIndicatron.Core.Chat;
using BuildIndicatron.Core.Helpers;
using BuildIndicatron.Core.Processes;
using BuildIndicatron.Core.Settings;
using BuildIndicatron.Server.Core.Fakes;
using BuildIndicatron.Server.Core.Properties;
using BuildIndicatron.Shared.Enums;
using Microsoft.Extensions.DependencyInjection;

namespace BuildIndicatron.Server.Core.Setup
{
    public class IocContainer
    {
        private static Lazy<IocContainer> _instance =
            new Lazy<IocContainer>(() => throw new Exception("Call Initialize first."));

        private IocContainer(IServiceCollection services)
        {
            var builder = new ContainerBuilder();
            if (PlatformHelper.IsLinux)
                SetupConcrete(builder);
            else
                SetupFakes(builder);
            SetupTools(builder);
            if (services != null) builder.Populate(services);

            RegisterControllers(builder);
            Container = builder.Build();
        }

        public IContainer Container { get; }

        #region singleton

        public static IocContainer Instance => _instance.Value;

        #endregion

        public static GpioConfiguration ConfigurationRobot => new GpioConfiguration(
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
                new List<GpioConfiguration.Button> {new GpioConfiguration.Button {Pin = GpIO.GPIO24, IsToggle = false}}
        };

        public static GpioConfiguration ConfigurationDarth2 => new GpioConfiguration(
            new GpioConfiguration.Target(PinName.SecondaryLightRed, GpIO.GPIO7, true),
            new GpioConfiguration.Target(PinName.SecondaryLightGreen, GpIO.GPIO8, true),
            new GpioConfiguration.Target(PinName.MainLightRed, GpIO.GPIO11, true),
            new GpioConfiguration.Target(PinName.MainLightGreen, GpIO.GPIO25, true),
            new GpioConfiguration.Target(PinName.MainLightBlue, GpIO.GPIO9, true)
        )
        {
            Buttons =
                new List<GpioConfiguration.Button> {new GpioConfiguration.Button {Pin = GpIO.GPIO24, IsToggle = false}}
        };


        public static GpioConfiguration ConfigurationDarth => new GpioConfiguration(
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
                new List<GpioConfiguration.Button> {new GpioConfiguration.Button {Pin = GpIO.GPIO24, IsToggle = false}}
        };

        public static void Initialize(IServiceCollection services)
        {
            _instance = new Lazy<IocContainer>(() => new IocContainer(services));
        }

        public T Resolve<T>()
        {
            return Container.Resolve<T>();
        }

        #region Private Methods

        private void SetupTools(ContainerBuilder builder)
        {
            builder.RegisterType<DownloadToFile>()
                .As<IDownloadToFile>()
                .WithParameter("tempPath", ServerSettings.Default.SpeachTempFileLocation);
            builder.RegisterType<Stage>().As<IStage>().SingleInstance();

            builder.RegisterAssemblyTypes(typeof(IFactory).Assembly)
                .Where(t => t.GetInterfaces()
                    .Any(i => i.IsAssignableFrom(typeof(IReposonseFlow))))
                .AsSelf().SingleInstance();
            builder.RegisterType<SequencesFactory>();
            builder.RegisterType<DeployCoreContext>();
            builder.RegisterType<ChatBot>().As<IChatBot>();
            builder.Register(x=>new JenkinsFactory(ServerSettings.Default)).As<IJenkinsFactory>().SingleInstance();
            builder.RegisterType<MonitorJenkins>().As<IMonitorJenkins>();
            builder.RegisterType<HttpLookup>().As<IHttpLookup>();
            builder.RegisterType<VolumeSetter>().As<IVolumeSetter>();
            builder.Register(context => new SettingsManager(SettingFile()))
                .As<ISettingsManager>().SingleInstance();

            builder.Register(context => new AutofacInjector(Container)).As<IFactory>();
            builder.RegisterType<SequencePlayer>().As<ISequencePlayer>();
            builder.Register(t => new SoundFilePicker(ServerSettings.Default.SoundFileLocation)).As<ISoundFilePicker>();
        }

        private string SettingFile()
        {
            return Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) ?? "", "setting.json");
        }

        private void RegisterControllers(ContainerBuilder builder)
        {
            //builder.RegisterApiControllers(Assembly.GetExecutingAssembly());
        }

        private void SetupConcrete(ContainerBuilder builder)
        {
            builder.RegisterType<Mp3Player>().As<IMp3Player>();
            builder.Register(context => new VoiceRss(context.Resolve<IDownloadToFile>(), context.Resolve<IMp3Player>(),
                context.Resolve<ISettingsManager>().Get("voice_rss_key"))).As<ITextToSpeech>();
//            builder.Register(t => new VoiceEnhancer(@"resources/sounds/Funny/R2D2c.wav", "speed 1.3 echo 0.8 0.88 6.0 0.4"))
            builder.Register(t =>
                    new VoiceEnhancer(
                        t.Resolve<ISettingsManager>().Get("voice_bg_file", @"resources/sounds/Funny/R2D2c.wav"),
                        "speed 1"))
                .As<IVoiceEnhancer>();
            builder.RegisterType<FakePinManager>()
                .WithParameter("configuration", ConfigurationRobot)
                .As<IPinManager>().SingleInstance();
        }

        private void SetupFakes(ContainerBuilder builder)
        {
            builder.RegisterType<FakePlayer>().As<IMp3Player>();
            builder.RegisterType<FakeTextToSpeech>().As<ITextToSpeech>();
            builder.RegisterType<FakePlayer>().As<IVoiceEnhancer>();
            builder.RegisterType<FakePinManager>()
                .As<IPinManager>();
        }

        #endregion
    }
}