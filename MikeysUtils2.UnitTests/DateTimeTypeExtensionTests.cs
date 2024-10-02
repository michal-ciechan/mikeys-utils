using MikeysUtils2.Client.Pages.DateTimePage;

namespace MikeysUtils2.UnitTests;

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
        var offset = new DateTimeOffset();
        
        if (!time.TryParseAsDateTime(DateTimeType.Time_UTC_ISO_8601, out var result))
        {
            Assert.Fail("Failed to parse time");
        }

        var formatted = result.ToString("HH:mm:ss.fffff");

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