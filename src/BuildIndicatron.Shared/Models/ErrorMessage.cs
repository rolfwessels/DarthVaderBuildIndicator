namespace BuildIndicatron.Shared.Models
{
    public class ErrorMessage
    {
        public string Message { get; }
        public string AdditionalDetail { get; set; }

        public ErrorMessage(string message)
        {
            Message = message;
        }
    }
}