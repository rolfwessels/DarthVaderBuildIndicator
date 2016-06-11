using System;
using System.Threading.Tasks;
using BuildIndicatron.Core.SimpleTextSplit;

namespace BuildIndicatron.Core.Chat
{
    public abstract class TextSplitterContextBase<T> : ReposonseFlowBase, IReposonseFlow where T:class
    {
        protected readonly Lazy<TextSplitter<T>> _textSplitter;
        private bool _hasBeenApplied;

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

        public virtual Task Respond(ChatContextHolder chatContextHolder, IMessageContext context)
        {
            var value = Activator.CreateInstance<T>();
            _textSplitter.Value.Process(context.Text, value);
            return Response(chatContextHolder, context, value);
        }

        protected abstract Task Response(ChatContextHolder chatContextHolder, IMessageContext context, T server);

        #endregion


    }
}