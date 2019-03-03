using System;

namespace Utilities.SeedWork
{
    public interface IEffectiveDated
    {
        DateTime? EffectiveFrom { get; }
        DateTime? EffectiveTo { get; }
    }
}
