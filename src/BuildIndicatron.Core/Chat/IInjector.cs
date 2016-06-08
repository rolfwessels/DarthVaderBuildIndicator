namespace BuildIndicatron.Core.Chat
{
    public interface IInjector
    {
        T Resolve<T>();
    }
}