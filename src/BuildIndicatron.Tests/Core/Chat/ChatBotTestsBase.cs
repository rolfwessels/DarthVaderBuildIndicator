using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Autofac;
using BuildIndicatron.Core.Chat;
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
        private IContainer _container;
        protected Mock<ITextToSpeech> _mockITextToSpeech;
        protected Mock<IPinManager> _mockIPinManager;
        protected Mock<IMp3Player> _mockIMp3Player;
        protected Mock<ISoundFilePicker> _mockISoundFilePicker;
        protected Mock<ISettingsManager> _mockISettingsManager;

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

            builder.Register(context => _mockITextToSpeech.Object).As<ITextToSpeech>();
            builder.Register(context => _mockIPinManager.Object).As<IPinManager>();
            builder.Register(context => _mockIMp3Player.Object).As<IMp3Player>();
            builder.Register(context => _mockISoundFilePicker.Object).As<ISoundFilePicker>();
            builder.Register(context => _mockISettingsManager.Object).As<ISettingsManager>();
        }

        private void DefaultRegsters(ContainerBuilder builder)
        {
            builder.Register(context => new IocContainer.Factory(_container)).As<IFactory>().SingleInstance();
            builder.RegisterAssemblyTypes(typeof (IFactory).Assembly)
                .Where(t => t.GetInterfaces()
                    .Any(i => i.IsAssignableFrom(typeof (IReposonseFlow))))
                .AsSelf().SingleInstance();
            builder.RegisterType<SequencesFactory>();
            builder.RegisterType<ChatBot>().As<IChatBot>();
        }

        [TearDown]
        public virtual void TearDown()
        {
            _mockITextToSpeech.VerifyAll();
            _mockIPinManager.VerifyAll();
            _mockIMp3Player.VerifyAll();
            _mockISoundFilePicker.VerifyAll();
            _mockISettingsManager.VerifyAll();
          
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