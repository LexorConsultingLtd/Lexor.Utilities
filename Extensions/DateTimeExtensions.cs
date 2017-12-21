﻿using System;

namespace Lexor.Utilities.Extensions
{
    public static class DateTimeExtensions
    {
        public static bool DateOnly(this DateTime value)
        {
            return value.Hour == 0 && value.Minute == 0 && value.Second == 0 && value.Millisecond == 0;
        }

        public static bool HasTimeComponent(this DateTime value) => !DateOnly(value);

        public static string FormatDate(this DateTime value) => value.ToString("MMM d, yyyy");
        public static string FormatTime(this DateTime value) => value.ToString("HH:mm:sstt");

        public static string Format(this DateTime value)
        {
            var result = value.FormatDate();
            if (value.HasTimeComponent()) result += value.FormatTime();
            return result;
        }
    }
}