using System;
using Utilities.SeedWork;

namespace Utilities.Extensions
{
    public static class EffectiveDatedExtensions
    {
        public static bool InEffect(this IEffectiveDated item)
        {
            var now = DateTime.Now;
            var effectiveFrom = item.EffectiveFrom ?? DateTime.MinValue;
            var effectiveTo = item.EffectiveTo ?? DateTime.MaxValue;
            return effectiveFrom <= now && now < effectiveTo;
        }
    }
}
