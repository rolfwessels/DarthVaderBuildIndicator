using System;
using System.Threading;

namespace BuildIndicatron.Tests.Helpers
{
    public static class TestHelper
    {
        public static bool WaitFor<T>(this T webApiIntegrationTests, Func<T, bool> func, int value = 1000)
        {
            return WaitFor(webApiIntegrationTests, func, b => b, value);
        }

        public static TType WaitFor<T, TType>(this T webApiIntegrationTests, Func<T, TType> func, Func<TType, bool> result, int value = 1000)
        {
            var dateTime = DateTime.Now.Add(TimeSpan.FromMilliseconds(value));
            TType type;
            do
            {
                type = func(webApiIntegrationTests);
                if (result(type))
                {
                    return type;
                }
                Thread.Sleep(200);
            } while (DateTime.Now < dateTime);
            return type;
        }
    }
}