using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace Lexor.Utilities.Extensions
{
    public static class StringExtensions
    {
        public static string SplitCamelCase(this string str)
        {
            return Regex.Replace(Regex.Replace(str, @"(\P{Ll})(\P{Ll}\p{Ll})", "$1 $2"), @"(\p{Ll})(\P{Ll})", "$1 $2");
        }

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
    }
}
