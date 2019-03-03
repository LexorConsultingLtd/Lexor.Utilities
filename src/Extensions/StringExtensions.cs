using System;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Global

namespace Utilities.Extensions
{
    public static class StringExtensions
    {
        public static string SplitCamelCase(this string str) =>
            Regex.Replace(Regex.Replace(str, @"(\P{Ll})(\P{Ll}\p{Ll})", "$1 $2"), @"(\p{Ll})(\P{Ll})", "$1 $2");

        public static string SplitCamelCase(this Enum e) =>
            e.ToString().SplitCamelCase();

        public static string RemoveAccents(this string text) =>
            HttpUtility.UrlDecode(HttpUtility.UrlEncode(text, Encoding.GetEncoding("iso-8859-7")));

        /// <summary>
        /// This must be called to initialize/enable additional encodings
        /// </summary>
        public static void RegisterEncodings()
        {
            var provider = CodePagesEncodingProvider.Instance;
            Encoding.RegisterProvider(provider);
        }

        public static string ToSqlLiteral(this string s) => s.Replace("'", "''");

        public static bool IsNullOrEmpty(this string s) => string.IsNullOrEmpty(s);
        public static bool HasValue(this string s) => !string.IsNullOrEmpty(s);

        public static string EscapeJavascriptString(this string s) => s
            .Replace(@"\", @"\\");

        public static void ParseUnitNumber(this string unit, out int numberValue, out string letterValue)
        {
            numberValue = 0;
            letterValue = null;
            if (unit.IsNullOrEmpty()) return;

            // Remove any "bad" characters
            unit = unit.Replace(" ", "");

            // Find any leading number in the unit string
            var leadingNumber = Regex.Matches(unit, @"^\d+")
                .Select(i => i.Value)
                .FirstOrDefault();
            if (leadingNumber == null)
            {
                letterValue = unit;
                return;
            }

            numberValue = Convert.ToInt32(leadingNumber);
            letterValue = unit.Substring(leadingNumber.Length);
        }

        public static bool IsNumeric(this string s) => int.TryParse(s, out _);

        public static string GetUnitNumberSortKey(this string unit)
        {
            ParseUnitNumber(unit, out var numberValue, out var letterValue);
            return $"{numberValue:d6}{letterValue}";
        }

        public static string Repeat(this string input, int count)
        {
            if (input.IsNullOrEmpty()) return string.Empty;

            var sb = new StringBuilder(input.Length * count);
            for (var i = 0; i < count; i++) sb.Append(input);
            return sb.ToString();
        }
    }
}
