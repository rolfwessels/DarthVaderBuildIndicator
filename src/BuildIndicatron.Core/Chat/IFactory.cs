namespace BuildIndicatron.Core.Chat
{
    public interface IFactory
    {
        T Resolve<T>();
    }
}