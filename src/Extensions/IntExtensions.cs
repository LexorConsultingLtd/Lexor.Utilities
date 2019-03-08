namespace Utilities.Extensions
{
    public static class IntExtensions
    {
        public static string Pluralize(this int value, string singular, string plural = null) =>
            value == 1 ? $"1 {singular}" : $"{value:n0} {(string.IsNullOrEmpty(plural) ? $"{singular}s" : plural)}";
    }
}
