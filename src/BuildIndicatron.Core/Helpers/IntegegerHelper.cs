using System;

namespace BuildIndicatron.Core.Helpers
{
    public static class IntegegerHelper
    {
        public static int MinMax(this int amount, int min, int max)
        {
            return Math.Max(min, Math.Min(amount, max));
        }
    }
}