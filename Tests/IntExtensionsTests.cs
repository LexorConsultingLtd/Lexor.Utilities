using System;
using Utilities.Extensions;
using Xunit;

namespace Tests
{
    public class IntExtensionsTests
    {
        #region Pluralize

        [Theory]
        [InlineData(0, "Car", "Cars")]
        [InlineData(2, "Car", "Cars")]
        [InlineData(10000, "Car", "Cars")]
        public void TestPlural(int value, string singular, string plural) =>
            Assert.Equal(value.Pluralize(singular, plural), $"{value:n0} {plural}");

        [Theory]
        [InlineData(1, "Car", "Cars")]
        public void TestSingular(int value, string singular, string plural) =>
            Assert.Equal(value.Pluralize(singular, plural), $"{value} {singular}");

        #endregion
    }
}
