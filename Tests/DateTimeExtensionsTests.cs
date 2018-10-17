using System;
using Utilities.Extensions;
using Xunit;

namespace Tests
{
    public class DateTimeExtensionsTests
    {
        [Theory]
        [InlineData(null)]
        public void TestNullDateTimeFormats(DateTime? value)
        {
            Assert.Equal("", value.FormatDate());
            Assert.Equal("", value.FormatIso());
            Assert.Equal("", value.FormatIsoDateOnly());
            Assert.Equal("", value.FormatTime());
            Assert.Equal("", value.FormatTimestamp());
        }

        [Theory]
        [InlineData("2000-01-01 01:01:01")]
        private void TestValidDateTimeFormats(string stringValue)
        {
            DateTime.TryParse(stringValue, out var dateTime);
            CheckValidDateTimeFormats(dateTime);

            DateTime? nullableDateTime = dateTime;
            CheckValidDateTimeFormats(nullableDateTime);
        }

        private static void CheckValidDateTimeFormats(DateTime value)
        {
            Assert.Equal("Jan 1, 2000", value.FormatDate());
            Assert.Equal("2000-01-01T01:01:01", value.FormatIso());
            Assert.Equal("2000-01-01", value.FormatIsoDateOnly());
            Assert.Equal("01:01 AM", value.FormatTime());
            Assert.Equal("Jan 1, 2000 01:01:01 AM", value.FormatTimestamp());
        }

        private static void CheckValidDateTimeFormats(DateTime? value)
        {
            Assert.Equal("Jan 1, 2000", value.FormatDate());
            Assert.Equal("2000-01-01T01:01:01", value.FormatIso());
            Assert.Equal("2000-01-01", value.FormatIsoDateOnly());
            Assert.Equal("01:01 AM", value.FormatTime());
            Assert.Equal("Jan 1, 2000 01:01:01 AM", value.FormatTimestamp());
        }
    }
}
