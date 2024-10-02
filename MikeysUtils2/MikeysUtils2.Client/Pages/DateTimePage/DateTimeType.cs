using System.Globalization;

namespace MikeysUtils2.Client.Pages.DateTimePage;


public enum DateTimeType
{
    None,
    yyyy_MM_dd_Date_ISO_8601,
    Time_UTC_ISO_8601,
    DateTime_UTC_ISO_8601,
    DateTime_UTC_RFC_1123,
    DayNumber,
    UnixSeconds,
    UnixMilliseconds,
    UnixNanoSeconds,
    yyyyMMdd_DateBasic,
    yMMdd_DateInt,
    Unknown
}

public static class DateTimeTypeExtensions
{
    public static string ToDescription(this DateTimeType dateTimeType)
    {
        return dateTimeType switch
        {
            DateTimeType.None => "None",
            DateTimeType.Unknown => "Unknown",
            DateTimeType.yyyy_MM_dd_Date_ISO_8601 => "Date (ISO 8601)",
            DateTimeType.Time_UTC_ISO_8601 => "Time - UTC (ISO 8601)",
            DateTimeType.DateTime_UTC_ISO_8601 => "Date Time - UTC (ISO 8601)",
            DateTimeType.DateTime_UTC_RFC_1123 => "Date Time - UTC (RFC 1123)",
            DateTimeType.DayNumber => ".NET Day Number",
            DateTimeType.UnixSeconds => "Unix Seconds",
            DateTimeType.UnixMilliseconds => "Unix Milliseconds",
            DateTimeType.UnixNanoSeconds => "Unix Nanoseconds",
            DateTimeType.yyyyMMdd_DateBasic => "Date (ISO 8601 Basic)",
            DateTimeType.yMMdd_DateInt => "Date (Int32)",
            _ => throw new ArgumentOutOfRangeException(nameof(dateTimeType), dateTimeType, null)
        };
    }

    public static string ToString(this DateTime dateTime, DateTimeType dateTimeType)
    {
        return dateTimeType switch
        {
            DateTimeType.None => "",
            DateTimeType.Unknown => "",
            DateTimeType.yyyy_MM_dd_Date_ISO_8601 => dateTime.ToString("yyyy-MM-dd"),
            DateTimeType.Time_UTC_ISO_8601 => dateTime.SetKind(DateTimeKind.Utc).ToString("THH:mm:ssZ"),
            DateTimeType.DateTime_UTC_ISO_8601 => dateTime.SetKind(DateTimeKind.Utc).ToString("O"),
            DateTimeType.DateTime_UTC_RFC_1123 => dateTime.SetKind(DateTimeKind.Utc).ToString("R"),
            DateTimeType.DayNumber => dateTime.ToDateOnly().DayNumber.ToString(),
            DateTimeType.UnixSeconds => dateTime.SinceEpoch().TotalSeconds.ToString("####################"),
            DateTimeType.UnixMilliseconds => dateTime.SinceEpoch().TotalMilliseconds.ToString("####################"),
            DateTimeType.UnixNanoSeconds => dateTime.SinceEpoch().TotalNanoseconds.ToString("####################"),
            DateTimeType.yyyyMMdd_DateBasic => dateTime.ToString("yyyyMMdd"),
            DateTimeType.yMMdd_DateInt =>  dateTime.ToString(dateTime.Year < 100 ? "yMMdd" : "yyyMMdd" ),
            _ => throw new ArgumentOutOfRangeException(nameof(dateTimeType), dateTimeType, null)
        };
    }
    
    public static TimeSpan MaxUnixTimestamp = DateTime.MaxValue - DateTime.UnixEpoch;
    public static TimeSpan MinUnixTimestamp = DateTime.MinValue - DateTime.UnixEpoch;

    public static bool TryParseAsDateTime(this string _input, DateTimeType type, out DateTime dateTimeParsed)
    {
        dateTimeParsed = default;
        
        switch (type)
        {
            case DateTimeType.yyyy_MM_dd_Date_ISO_8601:
                return DateTime.TryParseExact(
                    _input,
                    "yyyy-MM-dd",
                    CultureInfo.InvariantCulture,
                    DateTimeStyles.None,
                    out dateTimeParsed);
            // TODO: fix parsing of time, and not converting to local
            case DateTimeType.Time_UTC_ISO_8601:
                // Extended format
                return DateTime.TryParseExact(
                    _input,
                    "THH:mm:ssZ",
                    CultureInfo.InvariantCulture,
                    DateTimeStyles.AssumeUniversal | DateTimeStyles.AdjustToUniversal,
                    out dateTimeParsed
                ) || DateTime.TryParseExact(
                    _input,
                    "THH:mm:sszzz",
                    CultureInfo.InvariantCulture,
                    DateTimeStyles.AdjustToUniversal,
                    out dateTimeParsed
                ) || DateTime.TryParseExact(
                    _input,
                    "THH:mm:sszz",
                    CultureInfo.InvariantCulture,
                    DateTimeStyles.AdjustToUniversal,
                    out dateTimeParsed
                ) || DateTime.TryParseExact(
                    _input,
                    "THH:mm:ss",
                    CultureInfo.InvariantCulture,
                    DateTimeStyles.AdjustToUniversal,
                    out dateTimeParsed
                ) || 
                       // Basic Format
                       DateTime.TryParseExact(
                    _input,
                    "THHmmssZ",
                    CultureInfo.InvariantCulture,
                    DateTimeStyles.AssumeUniversal | DateTimeStyles.AdjustToUniversal,
                    out dateTimeParsed
                ) || DateTime.TryParseExact(
                    _input,
                    "THHmmsszzz",
                    CultureInfo.InvariantCulture,
                    DateTimeStyles.AdjustToUniversal,
                    out dateTimeParsed
                ) || DateTime.TryParseExact(
                    _input,
                    "THHmmsszz",
                    CultureInfo.InvariantCulture,
                    DateTimeStyles.AdjustToUniversal,
                    out dateTimeParsed
                ) || DateTime.TryParseExact(
                    _input,
                    "THHmmss",
                    CultureInfo.InvariantCulture,
                    DateTimeStyles.AdjustToUniversal,
                    out dateTimeParsed
                );
            case DateTimeType.DateTime_UTC_ISO_8601:
                return DateTime.TryParseExact(
                    _input, "O", DateTimeFormatInfo.InvariantInfo, DateTimeStyles.None,
                    out dateTimeParsed);
            case DateTimeType.DateTime_UTC_RFC_1123:
                return DateTime.TryParseExact(
                    _input, "R", DateTimeFormatInfo.InvariantInfo, DateTimeStyles.None,
                    out dateTimeParsed);
            case DateTimeType.DayNumber:
            {
                if (!long.TryParse(_input, out var longParsed))
                {
                    return false;
                }
                
                if (longParsed >= 0 && longParsed < DateOnly.MaxValue.DayNumber)
                {
                    dateTimeParsed = DateOnly.FromDayNumber((int)longParsed).ToDateTime();
                    return true;
                }

                return false;
            }
            case DateTimeType.UnixSeconds:
            {
                if (!long.TryParse(_input, out var longParsed))
                {
                    return false;
                }
                
                if (longParsed >= MinUnixTimestamp.TotalSeconds && longParsed <= MaxUnixTimestamp.TotalSeconds)
                {
                    dateTimeParsed = DateTime.UnixEpoch.AddSeconds(longParsed);
                    return true;
                }

                return false;
            }
            case DateTimeType.UnixMilliseconds:
            {
                if (!long.TryParse(_input, out var longParsed))
                {
                    return false;
                }
                
                if (longParsed >= MinUnixTimestamp.TotalMilliseconds && longParsed <= MaxUnixTimestamp.TotalMilliseconds)
                {
                    dateTimeParsed = DateTime.UnixEpoch.AddMilliseconds(longParsed);
                    return true;
                }

                return false;
            }
            case DateTimeType.UnixNanoSeconds:
            {
                if (!long.TryParse(_input, out var longParsed))
                {
                    return false;
                }
                
                if (longParsed >= MinUnixTimestamp.TotalNanoseconds && longParsed <= MaxUnixTimestamp.TotalNanoseconds)
                {
                    dateTimeParsed = DateTime.UnixEpoch.AddTicks(longParsed / 100);
                    return true;
                }

                return false;
            }
            case DateTimeType.yyyyMMdd_DateBasic:
                break;
            case DateTimeType.yMMdd_DateInt:
                return DateTime.TryParseExact(
                    _input, "yyyMMdd", DateTimeFormatInfo.InvariantInfo, DateTimeStyles.None,
                    out dateTimeParsed) || DateTime.TryParseExact(
                    _input, "yMMdd", DateTimeFormatInfo.InvariantInfo, DateTimeStyles.None,
                    out dateTimeParsed) && dateTimeParsed.Year < 100;
            case DateTimeType.None:
            case DateTimeType.Unknown:
                    return false;
            default:
                throw new ArgumentOutOfRangeException(nameof(type), type, null);
        }

        return false;
    } 
    
    public static bool IsTimeOnly(this DateTimeType dateTimeType)
    {
        return dateTimeType switch
        {
            DateTimeType.Time_UTC_ISO_8601 => true,
            _ => false,
        };
    }

    public static bool IsDateOnly(this DateTimeType dateTimeType)
    {
        return dateTimeType switch
        {
            DateTimeType.DayNumber => true,
            DateTimeType.yMMdd_DateInt => true,
            DateTimeType.yyyyMMdd_DateBasic => true,
            DateTimeType.yyyy_MM_dd_Date_ISO_8601 => true,
            _ => false,
        };
    }

    public static bool HasTime(this DateTimeType dateTimeType)
    {
        return dateTimeType switch
        {
            DateTimeType.Time_UTC_ISO_8601 => true,
            DateTimeType.DateTime_UTC_ISO_8601 => true,
            DateTimeType.DateTime_UTC_RFC_1123 => true,
            DateTimeType.UnixSeconds => true,
            DateTimeType.UnixMilliseconds => true,
            DateTimeType.UnixNanoSeconds => true,
             _ => false,
        };
    }

    public static bool HasDate(this DateTimeType dateTimeType)
    {
        return dateTimeType switch
        {
            DateTimeType.yyyy_MM_dd_Date_ISO_8601 => true,
            DateTimeType.DateTime_UTC_ISO_8601 => true,
            DateTimeType.DateTime_UTC_RFC_1123 => true,
            DateTimeType.DayNumber => true,
            DateTimeType.UnixSeconds => true,
            DateTimeType.UnixMilliseconds => true,
            DateTimeType.UnixNanoSeconds => true,
            DateTimeType.yyyyMMdd_DateBasic => true,
            DateTimeType.yMMdd_DateInt => true,
            _ => false,
        };
    }
}

public static class DateTimeExtensions
{
    public static DateOnly ToDateOnly(this DateTime dateTimeType)
    {
        return DateOnly.FromDateTime(dateTimeType);
    }
}