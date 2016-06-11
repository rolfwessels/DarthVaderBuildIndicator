using System;
using System.Threading.Tasks;
using BuildIndicatron.Core.SimpleTextSplit;

namespace BuildIndicatron.Core.Chat
{
    public class QuickQuickTextMatch : QuickTextSplitterContext<QuickQuickTextMatch.Matcdh>
    {
        public class Matcdh
        {
        }

        public QuickQuickTextMatch(Action<TextSplitter<Matcdh>> apply, Func<ChatContextHolder, IMessageContext, Task> response)
            : base(new Matcdh(), apply, (holder, context, arg3) => response(holder, context))
        {
        }
    }
}