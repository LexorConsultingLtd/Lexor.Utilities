namespace Utilities.Extensions
{
    public static class DecimalExtensions
    {
        public static string FormatCurrency(this decimal value, int numDecimals = 2) =>
            value.ToString($"C{numDecimals}");
    }
}
