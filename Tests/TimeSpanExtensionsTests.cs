using System;
using Utilities.Extensions;
using Xunit;

namespace Tests
{
    public class TimeSpanExtensionsTests
    {
        private DateTime StartDate { get; } = new DateTime(2000, 1, 1);

        #region Format

        [Fact]
        public void TestParseUnitNumbers()
        {
            TestFormat(StartDate.AddDays(1500).Subtract(StartDate), false, "1,500 days");
            TestFormat(StartDate.AddDays(1500).Subtract(StartDate), true, "1,500 days");

            TestFormat(StartDate.AddDays(1).Subtract(StartDate), false, "1 day");
            TestFormat(StartDate.AddHours(1).Subtract(StartDate), false, "1 hour");
            TestFormat(StartDate.AddMinutes(1).Subtract(StartDate), false, "1 minute");
            TestFormat(StartDate.AddSeconds(1).Subtract(StartDate), false, "");
            TestFormat(StartDate.AddSeconds(1).Subtract(StartDate), true, "1 second");

            TestFormat(StartDate.AddDays(2).AddHours(10).AddMinutes(20).AddSeconds(30).Subtract(StartDate), false, "2 days 10 hours 20 minutes");
            TestFormat(StartDate.AddDays(2).AddHours(10).AddMinutes(20).AddSeconds(30).Subtract(StartDate), true, "2 days 10 hours 20 minutes 30 seconds");
        }

        private void TestFormat(TimeSpan timeSpan, bool includeSeconds, string expectedFormat)
        {
            var actualFormat = timeSpan.Format(includeSeconds);
            Assert.Equal(expectedFormat, actualFormat);
        }

        #endregion
    }
}
