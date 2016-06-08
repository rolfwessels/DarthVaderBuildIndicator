using System.Collections.Generic;
using System.Threading.Tasks;
using Autofac;
using BuildIndicatron.Core.Chat;
using BuildIndicatron.Core.Processes;
using BuildIndicatron.Server.Setup;
using Moq;
using NUnit.Framework;

namespace BuildIndicatron.Tests.Chat
{
    public class ChatBotTestsBase
    {
        protected ChatBot _chatBot;
        private IContainer container;
        protected Mock<IPinManager> _mockIPinManager;

        public void Setup()
        {
            _mockIPinManager = new Mock<IPinManager>();
            
            
            var builder = new ContainerBuilder();
            IocContainer.SetAllContexts(builder);
            builder.Register(x => _mockIPinManager.Object).As<IPinManager>();
            builder.Register(context => new AutofacInjector(container)).As<IInjector>().SingleInstance();

            container = builder.Build();
            _chatBot = container.Resolve<ChatBot>();
        }


        [TearDown]
        public void TearDown()
        {
            _mockIPinManager.VerifyAll();
        }

        
        public class SampleMessage : IMessageContext
        {
            private readonly List<string> _responses
                ;

            public SampleMessage(string text, bool isDirectedAtMe)
            {
                _responses = new List<string>();
                Text = text;
                IsDirectedAtMe = isDirectedAtMe;
            }

            #region Implementation of IMessageContext

            public string Text { get; private set; }
            public bool IsDirectedAtMe { get; private set; }

            public Task Respond(string message)
            {
                return Task.Run(() => { _responses.Add(message); });
            }

            #endregion

            public List<string> Responses
            {
                get { return _responses; }
            }
        }
    }
}