using System;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Threading.Tasks;
using BuildIndicatron.Core;
using BuildIndicatron.Core.Chat;
using BuildIndicatron.Core.Helpers;
using log4net;
using SlackConnector;
using SlackConnector.Models;

namespace BuildIndicatron.Server.Setup
{
    public class SlackBotServer
    {
        private static string _apiToken;
        private static readonly ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private readonly ISlackConnector _connector;
        private readonly IChatBot _chatBot;
        private ISlackConnection _connection;
        private bool _isConnected;

        public SlackBotServer(string apiToken)
        {
            _apiToken = apiToken;
            _connector = new SlackConnector.SlackConnector();
            _chatBot = IocContainer.Instance.Resolve<IChatBot>();
            _isConnected = false;
        }

        public async Task<bool> ContinueslyTryToConnect()
        {
            if (_isConnected) return true;
            var wait = 0;
            while (true)
            {
                await Task.Delay(wait);
                var task = Task.Run(Connect);
                var connected = await task;
                if (connected) return true;
                wait = (wait * 2).MinMax(2000, 160000);
            }
        }

        public async Task<bool> Connect()
        {
            using (NDC.Push("Slackbot"))
            {
                ServicePointManager.ServerCertificateValidationCallback +=
                    (sender, certificate, chain, policyErrors) => { return true; };
                try
                {
                    Disconnect();
                    _log.Info("Connecting");
                    if (_connection != null && _connection.IsConnected) return true;
                    _connection = await _connector.Connect(_apiToken);
                    _connection.OnMessageReceived += MessageReceived;
                    _connection.OnDisconnect += ConnectionStatusChanged;
                    _log.Info("Connected");
                    _isConnected = true;
                    return true;
                }
                catch (Exception e)
                {
                    _log.Error("Error connecting to slackbot:" + e.Message, e);
                }
                return false;
            }
        }

        public Task SayTo(string userName, string message)
        {
            using (NDC.Push("Slackbot"))
            {
                var chatHub = _connection.ConnectedHubs.Where(x => x.Value.Name.ToLower() == userName.ToLower())
                    .Select(x => x.Value).FirstOrDefault();
                if (chatHub == null)
                    _log.Warn(string.Format("Could not find user {0} in {1}", userName,
                        _connection.ConnectedHubs.Select(x => x.Value.Name).StringJoin()));
                return SayTo(chatHub, message);
            }
        }

        public async Task SayTo(SlackChatHub chatHub, string message)
        {
            await ContinueslyTryToConnect();
            if (chatHub != null)
                await _connection.Say(new BotMessage {ChatHub = chatHub, Text = message});
        }

        #region Private Methods

        private Task MessageReceived(SlackMessage message)
        {
            using (NDC.Push("Slackbot"))
            {
                var slackBotMessageContext = new SlackBotMessageContext(this, message) {Text = message.Text};
                _log.Info(string.Format("Chathub {0}-{2} Text:{1}", message.ChatHub.Name, message.Text,
                    message.User.Name));
                _chatBot.Process(slackBotMessageContext).FireAndForgetWithLogging();
                return Task.Delay(1);
            }
        }

        private void ConnectionStatusChanged()
        {
            using (NDC.Push("Slackbot"))
            {
                _isConnected = false;
                _log.Error("disconnected");
                Disconnect();
                _log.Info("trying to connect again");
                ContinueslyTryToConnect().ContinueWith(task => _log.Info("reconnected"));
            }
        }

        private void Disconnect()
        {
            if (_connection != null)
            {
                _connection.OnMessageReceived -= MessageReceived;
                _connection.OnDisconnect -= ConnectionStatusChanged;
                _connection.Close().Wait();
                _connection = null;
            }
        }

        #endregion
    }
}