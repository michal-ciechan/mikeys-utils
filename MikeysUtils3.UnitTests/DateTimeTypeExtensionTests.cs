using MikeysUtils3.Pages.DateTimePage;

namespace MikeysUtils3.UnitTests;

public class DateTimeTypeExtensionTests
{
    [Test]
    [Arguments("T00:02:03+00:00", "00:02:03.00000", DateTimeKind.Utc)]
    [Arguments("T00:02:03+0000", "00:02:03.00000", DateTimeKind.Utc)]
    [Arguments("T00:02:03+00", "00:02:03.00000", DateTimeKind.Utc)]
    [Arguments("T00:02:03Z", "00:02:03.00000", DateTimeKind.Utc)]
    [Arguments("T00:02:03", "00:02:03.00000", DateTimeKind.Unspecified)]
    [Arguments("T000203+00:00", "00:02:03.00000", DateTimeKind.Utc)]
    [Arguments("T000203+0000", "00:02:03.00000", DateTimeKind.Utc)]
    [Arguments("T000203+00", "00:02:03.00000", DateTimeKind.Utc)]
    [Arguments("T000203Z", "00:02:03.00000", DateTimeKind.Utc)]
    [Arguments("T000203", "00:02:03.00000", DateTimeKind.Unspecified)]
    public async Task TryParseAsDateTime_Time_UTC_ISO_8601(string time, string expected, DateTimeKind kind)
    {
        if (!time.TryParseAsDateTime(DateTimeType.Time_UTC_ISO_8601, out var result))
        {
            Assert.Fail("Failed to parse time");
        }

        var formatted = result.ToString("HH:mm:ss.fffff");

        await Assert.That(formatted).IsEqualTo(expected);
        await Assert.That(result.Kind).IsEqualTo(kind);
    }
    
    // TODO: refactor tests to be based on input and all output
    
    [Test]
    [Arguments("2024-10-23 21:39:40", "2024-10-23T21:39:40.0000000", DateTimeKind.Unspecified)]
    [Arguments("2024-10-23 21:39:40.1", "2024-10-23T21:39:40.1000000", DateTimeKind.Unspecified)]
    [Arguments("2024-10-23 21:39:40.12", "2024-10-23T21:39:40.1200000", DateTimeKind.Unspecified)]
    [Arguments("2024-10-23 21:39:40.123", "2024-10-23T21:39:40.1230000", DateTimeKind.Unspecified)]
    [Arguments("2024-10-23 21:39:40.1234", "2024-10-23T21:39:40.1234000", DateTimeKind.Unspecified)]
    [Arguments("2024-10-23 21:39:40.12345", "2024-10-23T21:39:40.1234500", DateTimeKind.Unspecified)]
    [Arguments("2024-10-23 21:39:40.123456", "2024-10-23T21:39:40.1234560", DateTimeKind.Unspecified)]
    [Arguments("2024-10-23 21:39:40.1234567", "2024-10-23T21:39:40.1234567", DateTimeKind.Unspecified)]
    public async Task TryParseAsDateTime_DateTime_yyyy_MM_dd_hh_mm_ss(string time, string expected, DateTimeKind kind)
    {
        if (!time.TryParseAsDateTime(DateTimeType.DateTime_UniversalSortable_yyyy_MM_dd_HH_mm_ss, out var result))
        {
            Assert.Fail("Failed to parse time");
        }

        var formatted = result.ToString("O");

        await Assert.That(formatted).IsEqualTo(expected);
        await Assert.That(result.Kind).IsEqualTo(kind);
    }
    
    [Test]
    [Arguments("0", "0001-01-01", DateTimeKind.Unspecified)]
    [Arguments("1", "0001-01-02", DateTimeKind.Unspecified)]
    [Arguments("739244", "2024-12-25", DateTimeKind.Unspecified)]
    public async Task TryParseAsDateTime_DayNumber(string dayNumber, string expected, DateTimeKind kind)
    {
        if (!dayNumber.TryParseAsDateTime(DateTimeType.DayNumber, out var result))
        {
            Assert.Fail("Failed to parse DayNumber");
        }

        var formatted = result.ToString("yyyy-MM-dd");

        await Assert.That(formatted).IsEqualTo(expected);
        await Assert.That(result.Kind).IsEqualTo(kind);
    }
    // TODO Test time parsing
}