using NodaTime;

namespace MikeysUtils3.Pages.TimeZonePage;


public record DateTimeZoneInfo
{
    public DateTimeZoneInfo(string id, string displayName)
    {
        Id = id;
        DisplayName = displayName;
        Zone = DateTimeZoneProviders.Tzdb[id];
    }

    public string Id { get; set; }
    public string DisplayName { get; }

    public DateTimeZone Zone { get; set; }

    public static DateTimeZoneInfo America_LosAngeles => new ("America/Los_Angeles", "WST");
    public static DateTimeZoneInfo America_Chicago => new ("America/Chicago", "CST");
    public static DateTimeZoneInfo America_NewYork => new ("America/New_York", "EST");
    public static DateTimeZoneInfo Utc => new ("Etc/UTC", "UTC");
    public static DateTimeZoneInfo Europe_London => new ("Europe/London", "BST");
    public static DateTimeZoneInfo Europe_Amsterdam => new ("Europe/Amsterdam", "CEST");
    public static DateTimeZoneInfo Asia_HongKong => new ("Asia/Hong_Kong", "HKT");
    public static DateTimeZoneInfo Australia_Sydney => new ("Australia/Sydney", "AUS");
}