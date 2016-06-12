using System.Threading.Tasks;
using BuildIndicatron.Core.Helpers;
using BuildIndicatron.Core.SimpleTextSplit;

namespace BuildIndicatron.Core.Chat
{


    public class AboutContext : TextSplitterContextBase<GreetingsContext.Meta>
    {
        #region Implementation of IReposonseFlow

        protected override void Apply(TextSplitter<GreetingsContext.Meta> textSplitter)
        {
            textSplitter.Map(@"(who)(ANYTHING)(you|this)(ANYTHING)")
                .Map(@"(where are you)(ANYTHING)")
                .Map(@"(what)(ANYTHING)(ip)(ANYTHING)")
                ;
        }

        protected override async Task Response(ChatContextHolder chatContextHolder, IMessageContext context, GreetingsContext.Meta server)
        {
            await context.Respond(string.Format("{1}, I'm @r2d2... working from home today at {0}.", IpAddressHelper.GetLocalIpAddresses().StringJoin(" or "), RandomTextHelper.Greetings));
            await context.Respond(string.Format("Im locate at {0}.", this.GetType().Assembly.Location));
        }

        #endregion

        private class Meta
        {

        }
    }
}   