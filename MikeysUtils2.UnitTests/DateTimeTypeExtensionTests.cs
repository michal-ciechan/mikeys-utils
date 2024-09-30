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

    // TODO Test time parsing
}