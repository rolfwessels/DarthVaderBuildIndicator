using System;

namespace BuildIndicatron.Core.Helpers
{
    public static class IntegegerHelper
    {
        public static int MinMax(this int amount, int min, int max)
        {
            return Math.Max(min, Math.Min(amount, max));
        }

        public static decimal Map(this decimal value, decimal fromSource, decimal toSource, decimal fromTarget,
            decimal toTarget)
        {
            return (value - fromSource) / (toSource - fromSource) * (toTarget - fromTarget) + fromTarget;
        }

        public static int Map(this int value, int fromSource, int toSource, int fromTarget, int toTarget)
        {
            var value1 = (decimal) value;
            return (int) Map(value1, fromSource, toSource, fromTarget, toTarget);
        }
    }
}