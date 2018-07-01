namespace Utilities.Extensions
{
    public static class BooleanExtensions
    {
        public static string FormatYesNo(this bool value) => value ? "Yes" : "No";

        public static string FormatYesNo(this bool? value, string nullText = "") =>
            value.HasValue ? value.Value.FormatYesNo() : nullText;
    }
}
