using System;
using System.Threading.Tasks;
using BuildIndicatron.Core.SimpleTextSplit;

namespace BuildIndicatron.Core.Chat
{
    public class QuickTextSplitterContext<T> : TextSplitterContextBase<T> where T : class
    {
        private readonly Action<TextSplitter<T>> _apply;
        private readonly Func<ChatContextHolder, IMessageContext, T, Task> _response;

        public QuickTextSplitterContext(T value, Action<TextSplitter<T>> apply, Func<ChatContextHolder , IMessageContext , T , Task > response)
        {
            if (value == null) throw new ArgumentNullException("value");
            if (apply == null) throw new ArgumentNullException("apply");
            if (response == null) throw new ArgumentNullException("response");
            Value = value;
            _apply = apply;
            _response = response;
        }

        public T Value { get; set; }

        #region Overrides of TextSplitterContextBase<T>

        protected override void Apply(TextSplitter<T> textSplitter)
        {
            _apply(textSplitter);
        }

        #region Overrides of TextSplitterContextBase<T>

        public override Task Respond(ChatContextHolder chatContextHolder, IMessageContext context)
        {

            _textSplitter.Value.Process(context.Text, Value);
            return Response(chatContextHolder, context, Value);
        }

        #endregion

        protected override Task Response(ChatContextHolder chatContextHolder, IMessageContext context, T server)
        {
            return _response(chatContextHolder, context, server);
        }

        #endregion
    }
}