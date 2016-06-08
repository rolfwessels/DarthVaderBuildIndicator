namespace BuildIndicatron.Core.Chat
{
    public class ReposonseFlowBase
    {
        protected bool ContainsText(IMessageContext context, string text)
        {
            return context.Text.ToLower().Contains(text.ToLower());
        }
    
        protected bool StartsWith(IMessageContext context, string text)
        {
            return context.Text.ToLower().StartsWith(text.ToLower());
        }

        protected string ExtractStartsWith(IMessageContext context, string text)
        {
            return context.Text.Substring(text.Length).Trim();
        }

        protected bool IsDirectedAtMe(IMessageContext context)
        {
            return context.IsDirectedAtMe;
        }
    }
}