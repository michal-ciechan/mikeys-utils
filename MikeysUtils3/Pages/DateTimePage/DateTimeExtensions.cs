namespace MikeysUtils3.Pages.DateTimePage;

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
        return new TimeSpan(dateTime.Ticks - DateTime.UnixEpoch.Ticks);
    }
    
    public static DateOnly ToDateOnly(this DateTime dateTimeType)
    {
        return DateOnly.FromDateTime(dateTimeType);
    }
}