using System;
using System.Threading;
using System.Threading.Tasks;

namespace BuildIndicatron.Core.Helpers
{
    public static class TestHelper
    {
        public static bool WaitFor<T>(this T entity, Func<T, bool> func, int value = 1000)
        {
            return WaitFor(entity, func, b => b, value);
        }

        public static TType WaitFor<T, TType>(this T entity, Func<T, TType> func, Func<TType, bool> result, int value = 1000, int millisecondsDelay = 200)
        {
            return AwaitAsync(entity, func, result, value, millisecondsDelay).Result;
        }

        public static async Task<TType> AwaitAsync<T, TType>(this T entity, Func<T, TType> func, Func<TType, bool> result, int value = 1000, int millisecondsDelay = 200)
        {
            var dateTime = DateTime.Now.Add(TimeSpan.FromMilliseconds(value));
            TType type;
            do
            {
                type = func(entity);
                if (result(type))
                {
                    return type;
                }
                await Task.Delay(millisecondsDelay);
            } while (DateTime.Now < dateTime);
            return type;
        }

       
    }
}