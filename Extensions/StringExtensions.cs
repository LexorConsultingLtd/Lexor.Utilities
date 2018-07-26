using System;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

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
    }
}
