namespace BuildIndicatron.Core.Chat
{
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
}