using System;
using Utilities.Extensions;
using Xunit;

namespace Tests
{
    public class StringExtensionsTests
    {
        #region IsNumeric

        [Theory]
        [InlineData("0")]
        [InlineData("1")]
        [InlineData("123")]
        public void TestIsNumericTrue(string value) =>
            Assert.True(value.IsNumeric());

        [Theory]
        [InlineData("B")]
        [InlineData("1B")]
        [InlineData("B1")]
        [InlineData("")]
        [InlineData(null)]
        public void TestIsNumericFalse(string value) =>
            Assert.False(value.IsNumeric());

        #endregion

        #region ParseUnitNumber

        [Fact]
        public void TestParseUnitNumbers()
        {
            TestParseUnitNumber(null, 0, null);
            TestParseUnitNumber("", 0, null);
            TestParseUnitNumber("1", 1, "");
            TestParseUnitNumber("A", 0, "A");
            TestParseUnitNumber("1A", 1, "A");
            TestParseUnitNumber("A1", 0, "A1");
            TestParseUnitNumber("3100 C", 3100, "C");
            TestParseUnitNumber("1E15-1", 1, "E15-1");
            TestParseUnitNumber("T423-1", 0, "T423-1");
            TestParseUnitNumber("A103", 0, "A103");
        }

        private void TestParseUnitNumber(string unit, int expectedNumber, string expectedLetters)
        {
            unit.ParseUnitNumber(out var number, out var letters);
            Assert.Equal(expectedNumber, number);
            Assert.Equal(expectedLetters, letters);
        }

        #endregion

        #region GetUnitNumberSortKey

        [Fact]
        public void TestGetUnitNumberSortKeys()
        {
            TestGetUnitNumberSortKey(null, "000000");
            TestGetUnitNumberSortKey("", "000000");
            TestGetUnitNumberSortKey("1", "000001");
            TestGetUnitNumberSortKey("A", "000000A");
            TestGetUnitNumberSortKey("1A", "000001A");
            TestGetUnitNumberSortKey("A1", "000000A1");
            TestGetUnitNumberSortKey("3100 C", "003100C");
            TestGetUnitNumberSortKey("1E15-1", "000001E15-1");
            TestGetUnitNumberSortKey("T423-1", "000000T423-1");
            TestGetUnitNumberSortKey("A103", "000000A103");
        }

        private void TestGetUnitNumberSortKey(string unit, string expectedSortKey)
        {
            var actual = unit.GetUnitNumberSortKey();
            Assert.Equal(expectedSortKey, actual);
        }

        #endregion
    }
}
