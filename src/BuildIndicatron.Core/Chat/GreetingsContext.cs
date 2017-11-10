using System.Threading.Tasks;
using BuildIndicatron.Core.Helpers;
using BuildIndicatron.Core.SimpleTextSplit;

namespace BuildIndicatron.Core.Chat
{
    public class GreetingsContext : TextSplitterContextBase<GreetingsContext.Meta>
    {
        #region Implementation of IReposonseFlow

        protected override void Apply(TextSplitter<Meta> textSplitter)
        {
            textSplitter.Map(@"(hi|hello|sup|hey)");
        }

        protected override async Task Response(ChatContextHolder chatContextHolder, IMessageContext context,
            Meta server)
        {
            await context.Respond(RandomTextHelper.Greetings);
        }

        #endregion

        public class Meta
        {
        }
    }
}