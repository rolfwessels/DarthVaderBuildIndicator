using System;
using System.Reflection;
using System.Threading.Tasks;
using BuildIndicatron.Core.Helpers;
using BuildIndicatron.Core.SimpleTextSplit;
using log4net;

namespace BuildIndicatron.Core.Chat
{
    public abstract class TextSplitterContextBase<T> : ReposonseFlowBase, IReposonseFlow where T:class
    {
        private static readonly ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        protected readonly Lazy<TextSplitter<T>> _textSplitter;

        protected TextSplitterContextBase()
        {
            _textSplitter = new Lazy<TextSplitter<T>>(() =>
            {
                var textSplitter = new TextSplitter<T>();
                Apply(textSplitter);
                return textSplitter;
            });
        }

        protected abstract void Apply(TextSplitter<T> textSplitter);

        #region Implementation of IReposonseFlow

        public virtual Task<bool>  CanRespond(IMessageContext context)
        {
           
            return Task.FromResult(IsDirectedAtMe(context) && _textSplitter.Value.IsMatch(context.Text));
        }

        public virtual async Task Respond(ChatContextHolder chatContextHolder, IMessageContext context)
        {
            try
            {
                var value = Activator.CreateInstance<T>();
                _textSplitter.Value.Process(context.Text, value);
                await Response(chatContextHolder, context, value);
            }
            catch (Exception e)
            {
                context.Respond(string.Format("Ooops, something has gone wrong...'`{0}`' check the logs for more information", e.Message)).FireAndForgetWithLogging();
                _log.Error(e.Message, e);
            }
        }


        protected abstract Task Response(ChatContextHolder chatContextHolder, IMessageContext context, T server);

        #endregion


    }
}