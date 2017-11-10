using System;
using System.Threading.Tasks;
using BuildIndicatron.Core.Helpers;

namespace BuildIndicatron.Core.Chat
{
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
            return Task.FromResult(_r.Next(0, _ods) == _ods - 1);
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
}