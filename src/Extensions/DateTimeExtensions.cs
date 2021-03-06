﻿using System;

namespace Utilities.Extensions
{
    public static class DateTimeExtensions
    {
        public static bool DateOnly(this DateTime value)
        {
            return value.Hour == 0 && value.Minute == 0 && value.Second == 0 && value.Millisecond == 0;
        }

        public static bool HasTimeComponent(this DateTime value) => !DateOnly(value);

        public static string FormatDate(this DateTime value) => value.ToString("MMM d, yyyy");
        public static string FormatDate(this DateTime? value) => value.HasValue ? value.Value.FormatDate() : "";
        public static string FormatTime(this DateTime value) => value.ToString("hh:mm tt");
        public static string FormatTime(this DateTime? value) => value.HasValue ? value.Value.FormatTime() : "";
        public static string FormatTimestamp(this DateTime value) => value.ToString("MMM d, yyyy hh:mm:ss tt");
        public static string FormatTimestamp(this DateTime? value) => value.HasValue ? value.Value.FormatTimestamp() : "";
        public static string FormatIso(this DateTime value) => value.ToString("yyyy-MM-ddTHH:mm:ss");
        public static string FormatIso(this DateTime? value) => value.HasValue ? value.Value.FormatIso() : "";
        public static string FormatIsoDateOnly(this DateTime value) => value.ToString("yyyy-MM-dd");
        public static string FormatIsoDateOnly(this DateTime? value) => value.HasValue ? value.Value.FormatIsoDateOnly() : "";

        public static string Format(this DateTime value)
        {
            var result = value.FormatDate();
            if (value.HasTimeComponent()) result = $"{result} {value.FormatTime()}";
            return result;
        }

        public static DateTime WithTime(this DateTime date, DateTime time)
        {
            var result = new DateTime(date.Year, date.Month, date.Day, time.Hour, time.Minute, time.Second, time.Millisecond);
            return result;
        }

        public static bool IsNewlyUpdated(this DateTime date) =>
            DateTime.Now - date <= TimeSpan.FromDays(1);
    }
}
