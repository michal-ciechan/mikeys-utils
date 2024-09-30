using MikeysUtils2.Client.Pages.DateTimePage;

namespace MikeysUtils2.Client;

public static class DateTimeExtensions
{
    public static DateTime SetKind(this DateTime dateTime, DateTimeKind kind)
    {
        return new DateTime(dateTime.Ticks, kind);
    }

    public static DateTime ToDateTime(this DateOnly dateTime)
    {
        return dateTime.ToDateTime(default);
    }

    public static TimeSpan SinceEpoch(this DateTime dateTime)
    {
        return new TimeSpan(dateTime.Ticks - dateTime.Ticks % TimeSpan.TicksPerSecond);
    }
}