using Autofac;
using BuildIndicatron.Core.Chat;

namespace BuildIndicatron.Server.Setup
{
    public class AutofacInjector : IInjector
    {
        private readonly IContainer _build;

        public AutofacInjector(IContainer build)
        {
            _build = build;
        }

        #region Implementation of IInjector

        public T Resolve<T>()
        {
            return _build.Resolve<T>();
        }

        #endregion
    }
}