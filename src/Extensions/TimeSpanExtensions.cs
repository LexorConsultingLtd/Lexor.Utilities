using Microsoft.EntityFrameworkCore.Internal;
using System;
using System.Collections.Generic;

namespace Utilities.Extensions
{
    public static class TimeSpanExtensions
    {
        public static string Format(this TimeSpan timeSpan, bool includeSeconds = false)
        {
            var parts = new List<string>();
            if (timeSpan.Days > 0) parts.Add($"{timeSpan.Days.Pluralize("day")}");
            if (timeSpan.Hours > 0) parts.Add($"{timeSpan.Hours.Pluralize("hour")}");
            if (timeSpan.Minutes > 0) parts.Add($"{timeSpan.Minutes.Pluralize("minute")}");
            if (includeSeconds && timeSpan.Seconds > 0) parts.Add($"{timeSpan.Seconds.Pluralize("second")}");
            return parts.Join(" ");
        }
    }
}
