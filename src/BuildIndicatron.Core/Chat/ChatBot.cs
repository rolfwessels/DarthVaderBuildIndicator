using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BuildIndicatron.Core.Helpers;

namespace BuildIndicatron.Core.Chat
{
    public class ChatBot : IChatBot
    {
        private ChatContextHolder _chatContextHolder;

        public ChatBot()
        {
            _chatContextHolder = new ChatContextHolder()
            .ListenTo<HelpContext>()
            .ListenTo<RandomJokeResponse>()
            ;
        }

        #region Implementation of IChatBot

        public Task Process(IMessageContext context)
        {
            return _chatContextHolder.MessageIn(context);
            }

        #endregion
    }

    public class RandomJokeResponse : ReposonseFlowBase, IReposonseFlow
    {
        private readonly Random _r = new Random();
        private readonly int _ods;

        public RandomJokeResponse()
        {
            _ods = 100;
        }

        #region Implementation of IReposonseFlow

        public Task<bool> CanRespond(IMessageContext context)
        {
            return Task.FromResult(_r.Next(0, _ods) == _ods-1);
        }

        public async Task Respond(ChatContextHolder chatContextHolder, IMessageContext context)
        {
            var oneLiner = RandomTextHelper.OneLiner.Split('.');
            foreach (var message in oneLiner)
            {
                await context.Respond(message);
                await Task.Delay(5000);
            }
        }

        #endregion
    }

    public class HelpContext : ReposonseFlowBase, IReposonseFlow
    {
        #region Implementation of IReposonseFlow

        public Task<bool> CanRespond(IMessageContext context)
        {
            return Task.FromResult(IsDirectedAtMe(context) || ContainsText(context,"help"));
        }

        public Task Respond(ChatContextHolder chatContextHolder, IMessageContext context)
        {
            return context.Respond("helping you now");
        }

        #endregion
    }

    public class ReposonseFlowBase
    {
        protected bool ContainsText(IMessageContext context, string help)
        {
            return context.Text.ToLower().Contains(help.ToLower());
        }

        protected bool IsDirectedAtMe(IMessageContext context)
        {
            return context.IsDirectedAtMe;
        }
    }

    public class ChatContextHolder
    {
        private readonly List<IReposonseFlow> _responseFlows = new List<IReposonseFlow>();

        public ChatContextHolder ListenTo<T>() where T : new()
        {
            _responseFlows.Add( (IReposonseFlow) new T());
            return this;
        }

        public async Task MessageIn(IMessageContext context)
        {
            foreach (var reposonseFlow in _responseFlows)
            {
                if (await reposonseFlow.CanRespond(context))
                {
                    await reposonseFlow.Respond(this,context);
                }
            }
            
        }
    }

    public interface IReposonseFlow
    {
        Task<bool> CanRespond(IMessageContext context);
        Task Respond(ChatContextHolder chatContextHolder, IMessageContext context);
    }
}