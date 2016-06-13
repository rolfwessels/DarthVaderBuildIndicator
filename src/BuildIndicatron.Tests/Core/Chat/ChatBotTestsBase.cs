using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Autofac;
using BuildIndicatron.Core.Api;
using BuildIndicatron.Core.Chat;
using BuildIndicatron.Core.Helpers;
using BuildIndicatron.Core.Processes;
using BuildIndicatron.Core.Settings;
using BuildIndicatron.Server.Setup;
using Moq;
using NUnit.Framework;

namespace BuildIndicatron.Tests.Core.Chat
{
    public class ChatBotTestsBase
    {
        protected ChatBot _chatBot;
        public IContainer _container;
        protected Mock<ITextToSpeech> _mockITextToSpeech;
        protected Mock<IPinManager> _mockIPinManager;
        protected Mock<IMp3Player> _mockIMp3Player;
        protected Mock<ISoundFilePicker> _mockISoundFilePicker;
        protected Mock<ISettingsManager> _mockISettingsManager;
        protected Mock<IJenkensApi> _mockIJenkensApi;
        protected Mock<IHttpLookup> _mockIHttpLookup;
        protected Mock<IVoiceEnhancer> _mockIVoiceEnhancer;
        protected Mock<IVolumeSetter> _mockIVolumeSetter;

        public virtual void Setup()
        {
            var builder = new ContainerBuilder();
            DefaultRegsters(builder);
            MockRegisters(builder);

            _container = builder.Build();
            _chatBot = new ChatBot(_container.Resolve<IFactory>());

        }

        private void MockRegisters(ContainerBuilder builder)
        {
            _mockITextToSpeech = new Mock<ITextToSpeech>();
            _mockIPinManager = new Mock<IPinManager>();
            _mockIMp3Player = new Mock<IMp3Player>();
            _mockISoundFilePicker = new Mock<ISoundFilePicker>(MockBehavior.Strict);
            _mockISettingsManager = new Mock<ISettingsManager>(MockBehavior.Strict);
            _mockIJenkensApi = new Mock<IJenkensApi>(MockBehavior.Strict);
            _mockIHttpLookup = new Mock<IHttpLookup>();
            _mockIVoiceEnhancer = new Mock<IVoiceEnhancer>(MockBehavior.Strict);

            _mockIVolumeSetter = new Mock<IVolumeSetter>(MockBehavior.Strict);
            
          
            builder.Register(context => _mockITextToSpeech.Object).As<ITextToSpeech>();
            builder.Register(context => _mockIPinManager.Object).As<IPinManager>();
            builder.Register(context => _mockIMp3Player.Object).As<IMp3Player>();
            builder.Register(context => _mockISoundFilePicker.Object).As<ISoundFilePicker>();
            builder.Register(context => _mockISettingsManager.Object).As<ISettingsManager>();
            builder.Register(context => _mockIJenkensApi.Object).As<IJenkensApi>();
            builder.Register(context => _mockIHttpLookup.Object).As<IHttpLookup>();
            builder.Register(context => _mockIVoiceEnhancer.Object);
            builder.Register(context => _mockIVolumeSetter.Object);
        }

        private void DefaultRegsters(ContainerBuilder builder)
        {
            builder.Register(context => new AutofacInjector(_container)).As<IFactory>().SingleInstance();
            builder.RegisterAssemblyTypes(typeof (IFactory).Assembly)
                .Where(t => t.GetInterfaces()
                    .Any(i => i.IsAssignableFrom(typeof (IReposonseFlow))))
                .AsSelf().SingleInstance();
            builder.RegisterType<SequencesFactory>();
            builder.Register(
                delegate(IComponentContext context)
                {
                    var deployCoreContext = new DeployCoreContext(context.Resolve<ISettingsManager>())
                    {
                        JenkinsApi = _mockIJenkensApi.Object
                    };
                    return deployCoreContext;
                });
            builder.RegisterType<ChatBot>().As<IChatBot>();
        }

        [TearDown]
        public virtual void TearDown()
        {
            _mockIVolumeSetter.VerifyAll();
          
            _mockITextToSpeech.VerifyAll();
            _mockIPinManager.VerifyAll();
            _mockIMp3Player.VerifyAll();
            _mockISoundFilePicker.VerifyAll();
            _mockISettingsManager.VerifyAll();
            _mockIVoiceEnhancer.VerifyAll();
            _mockIJenkensApi.VerifyAll();
            _mockIHttpLookup.VerifyAll();
         
          
        }

        public class MessageContext : IMessageContext
        {
            public MessageContext(string help)
            {
                Text = help;
                IsDirectedAtMe = true;
                LastMessages = new List<string>();
            }

            #region Implementation of IMessageContext

            public string Text { get; set; }
            public bool IsDirectedAtMe { get; set; }
            public List<string> LastMessages { get; private set; }

            public Task Respond(string message)
            {
                LastMessages.Add(message);
                return Task.FromResult(true);
            }

            #endregion
        }

    }
}