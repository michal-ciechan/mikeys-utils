using NodaTime;

namespace MikeysUtils3.Pages.TimeZonePage;


public record DateTimeZoneInfo
{
    public DateTimeZoneInfo(string id)
    {
        Id = id;
        DisplayName = id;
        TimeZone = id;
        Zone = DateTimeZoneProviders.Tzdb[id];
    }

    public DateTimeZoneInfo(string id, string timeZoneId, string timeZone)
    {
        Id = id;
        DisplayName = timeZoneId;
        TimeZone = timeZone;
        Zone = DateTimeZoneProviders.Tzdb[id];
    }

    public string TimeZone { get; set; }

    public string Id { get; set; }
    public string DisplayName { get; }

    public DateTimeZone Zone { get; set; }

    public static DateTimeZoneInfo America_LosAngeles => new ("America/Los_Angeles", "PST", "Pacific Standard Time");
    public static DateTimeZoneInfo America_Chicago => new ("America/Chicago", "CST", "Central Standard Time");
    public static DateTimeZoneInfo America_NewYork => new ("America/New_York", "EST", "Eastern Standard Time");
    public static DateTimeZoneInfo Utc => new ("Etc/UTC", "UTC", "Universal Time");
    public static DateTimeZoneInfo Europe_London => new ("Europe/London", "BST", "British Standard Time");
    public static DateTimeZoneInfo Europe_Amsterdam => new ("Europe/Amsterdam", "CEST", "Central Europe Standard Time");
    public static DateTimeZoneInfo Asia_HongKong => new ("Asia/Hong_Kong", "HKT", "Hong Kong Time");
    public static DateTimeZoneInfo Australia_Sydney => new ("Australia/Sydney", "AEDT", "Australia Eastern Standard Time");
}