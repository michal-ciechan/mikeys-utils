using System.Globalization;

namespace MikeysUtils3.Pages.DateTimePage;


public enum DateTimeType
{
    None,
    Date_yyyy_MM_dd_ISO_8601,
    DateTime_UniversalSortable_yyyy_MM_dd_HH_mm_ss,
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
    // TODO: Get regex to get numbers only?
    // private Regex NonNumbersRegex  = new(@"^\d+$");
    static DateTimeTypeExtensions()
    {
        HashSet<char> numbers = ['0', '1', '2', '3', '4', '5', '6', '7', '8', '9'];
        HashSet<char> dayOfWeekFirstLetters = ['M', 'm', 'T', 't', 'W', 'w', 'F', 'f', 'S', 's'];
        HashSet<char> validPrefixChars = [ ..numbers, 'T', '-', ..dayOfWeekFirstLetters ];
        HashSet<char> validSuffixChars = [ ..numbers, 'T', 'Z', 'z'];

        InvalidPrefixChars = Enumerable.Range(0, char.MaxValue).Select(x => (char)x).Except(validPrefixChars).ToArray();
        InvalidSuffixChars = Enumerable.Range(0, char.MaxValue).Select(x => (char)x).Except(validSuffixChars).ToArray();
    }

    public static char[] InvalidSuffixChars { get; set; }
    public static char[] InvalidPrefixChars { get; set; }

    public static string ToDescription(this DateTimeType dateTimeType)
    {
        return dateTimeType switch
        {
            DateTimeType.None => "None",
            DateTimeType.Unknown => "Unknown",
            DateTimeType.Date_yyyy_MM_dd_ISO_8601 => "Date (ISO 8601)",
            DateTimeType.Time_UTC_ISO_8601 => "Time - UTC (ISO 8601)",
            DateTimeType.DateTime_UTC_ISO_8601 => "Date Time - UTC (ISO 8601)",
            DateTimeType.DateTime_UTC_RFC_1123 => "Date Time - UTC (RFC 1123)",
            DateTimeType.DateTime_UniversalSortable_yyyy_MM_dd_HH_mm_ss => "Date Time - Universal Sortable",
            DateTimeType.DayNumber => ".NET Day Number",
            DateTimeType.UnixSeconds => "Unix Seconds",
            DateTimeType.UnixMilliseconds => "Unix Milliseconds",
            DateTimeType.UnixNanoSeconds => "Unix Nanoseconds",
            DateTimeType.yyyyMMdd_DateBasic => "Date (ISO 8601 Basic)",  // TODO: implement ISO BASIC parsing
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
            DateTimeType.Date_yyyy_MM_dd_ISO_8601 => dateTime.ToString("yyyy-MM-dd"),
            DateTimeType.Time_UTC_ISO_8601 => dateTime.SetKind(DateTimeKind.Utc).ToString("THH:mm:ssZ"),
            DateTimeType.DateTime_UTC_ISO_8601 => dateTime.SetKind(DateTimeKind.Utc).ToString("O"),
            DateTimeType.DateTime_UTC_RFC_1123 => dateTime.SetKind(DateTimeKind.Utc).ToString("R"),
            DateTimeType.DateTime_UniversalSortable_yyyy_MM_dd_HH_mm_ss => dateTime.ToString("yyyy-MM-dd hh:mm:ss.fffffff").TrimEnd('0', '.'),
            // Tests for type above + finish implementation 2024-10-09 23:11:49
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

    public static bool TryParseAsDateTime(this string input, DateTimeType type, out DateTime dateTimeParsed)
    {
        input = input.TrimStart(InvalidPrefixChars).TrimEnd(InvalidSuffixChars);
        dateTimeParsed = default;

        switch (type)
        {
            case DateTimeType.Date_yyyy_MM_dd_ISO_8601:
                return DateTime.TryParseExact(
                    input,
                    "yyyy-MM-dd",
                    CultureInfo.InvariantCulture,
                    DateTimeStyles.None,
                    out dateTimeParsed);
            // TODO: fix parsing of time, and not converting to local
            case DateTimeType.Time_UTC_ISO_8601:
                // Extended format
                return DateTime.TryParseExact(
                           input,
                           "THH:mm:ssZ",
                           CultureInfo.InvariantCulture,
                           DateTimeStyles.AssumeUniversal | DateTimeStyles.AdjustToUniversal,
                           out dateTimeParsed
                       ) || DateTime.TryParseExact(
                           input,
                           "THH:mm:sszzz",
                           CultureInfo.InvariantCulture,
                           DateTimeStyles.AdjustToUniversal,
                           out dateTimeParsed
                       ) || DateTime.TryParseExact(
                           input,
                           "THH:mm:sszz",
                           CultureInfo.InvariantCulture,
                           DateTimeStyles.AdjustToUniversal,
                           out dateTimeParsed
                       ) || DateTime.TryParseExact(
                           input,
                           "THH:mm:ss",
                           CultureInfo.InvariantCulture,
                           DateTimeStyles.AdjustToUniversal,
                           out dateTimeParsed
                       ) ||
                       // Basic Format
                       DateTime.TryParseExact(
                           input,
                           "THHmmssZ",
                           CultureInfo.InvariantCulture,
                           DateTimeStyles.AssumeUniversal | DateTimeStyles.AdjustToUniversal,
                           out dateTimeParsed
                       ) || DateTime.TryParseExact(
                           input,
                           "THHmmsszzz",
                           CultureInfo.InvariantCulture,
                           DateTimeStyles.AdjustToUniversal,
                           out dateTimeParsed
                       ) || DateTime.TryParseExact(
                           input,
                           "THHmmsszz",
                           CultureInfo.InvariantCulture,
                           DateTimeStyles.AdjustToUniversal,
                           out dateTimeParsed
                       ) || DateTime.TryParseExact(
                           input,
                           "THHmmss",
                           CultureInfo.InvariantCulture,
                           DateTimeStyles.AdjustToUniversal,
                           out dateTimeParsed
                       );
            case DateTimeType.DateTime_UTC_ISO_8601:
                return DateTime.TryParseExact(
                           input, "O", DateTimeFormatInfo.InvariantInfo, DateTimeStyles.None,
                           out dateTimeParsed) ||
                       DateTime.TryParseExact(input, "yyyy-MM-ddTHH:mm:ssZ", DateTimeFormatInfo.InvariantInfo,
                           DateTimeStyles.AdjustToUniversal, out dateTimeParsed);
            case DateTimeType.DateTime_UTC_RFC_1123:
                return DateTime.TryParseExact(
                    input, "R", DateTimeFormatInfo.InvariantInfo,
                    DateTimeStyles.AdjustToUniversal | DateTimeStyles.AssumeUniversal,
                    out dateTimeParsed);
            case DateTimeType.DateTime_UniversalSortable_yyyy_MM_dd_HH_mm_ss:
                input = input.Trim(['.', ':']).Trim();

                return DateTime.TryParseExact(
                    input, "yyyy-MM-dd HH:mm:ss.fffffff", DateTimeFormatInfo.InvariantInfo, DateTimeStyles.None,
                    out dateTimeParsed) || DateTime.TryParseExact(
                    input, "yyyy-MM-dd HH:mm:ss.ffffff", DateTimeFormatInfo.InvariantInfo, DateTimeStyles.None,
                    out dateTimeParsed) || DateTime.TryParseExact(
                    input, "yyyy-MM-dd HH:mm:ss.fffff", DateTimeFormatInfo.InvariantInfo, DateTimeStyles.None,
                    out dateTimeParsed) || DateTime.TryParseExact(
                    input, "yyyy-MM-dd HH:mm:ss.ffff", DateTimeFormatInfo.InvariantInfo, DateTimeStyles.None,
                    out dateTimeParsed) || DateTime.TryParseExact(
                    input, "yyyy-MM-dd HH:mm:ss.fff", DateTimeFormatInfo.InvariantInfo, DateTimeStyles.None,
                    out dateTimeParsed) || DateTime.TryParseExact(
                    input, "yyyy-MM-dd HH:mm:ss.ff", DateTimeFormatInfo.InvariantInfo, DateTimeStyles.None,
                    out dateTimeParsed) || DateTime.TryParseExact(
                    input, "yyyy-MM-dd HH:mm:ss.f", DateTimeFormatInfo.InvariantInfo, DateTimeStyles.None,
                    out dateTimeParsed) || DateTime.TryParseExact(
                    input, "yyyy-MM-dd HH:mm:ss", DateTimeFormatInfo.InvariantInfo, DateTimeStyles.None,
                    out dateTimeParsed) || DateTime.TryParseExact(
                    input, "yyyy-MM-dd HH:mm", DateTimeFormatInfo.InvariantInfo, DateTimeStyles.None,
                    out dateTimeParsed) || DateTime.TryParseExact(
                    input, "yyyy-MM-dd HH", DateTimeFormatInfo.InvariantInfo, DateTimeStyles.None,
                    out dateTimeParsed);
            case DateTimeType.DayNumber:
            {
                if (!long.TryParse(input, out var longParsed))
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
                if (!long.TryParse(input, out var longParsed))
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
                if (!long.TryParse(input, out var longParsed))
                {
                    return false;
                }

                if (longParsed >= MinUnixTimestamp.TotalMilliseconds &&
                    longParsed <= MaxUnixTimestamp.TotalMilliseconds)
                {
                    dateTimeParsed = DateTime.UnixEpoch.AddMilliseconds(longParsed);
                    return true;
                }

                return false;
            }
            case DateTimeType.UnixNanoSeconds:
            {
                if (!long.TryParse(input, out var longParsed))
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
                return DateTime.TryParseExact(
                    input, "yyyyMMdd", DateTimeFormatInfo.InvariantInfo, DateTimeStyles.None,
                    out dateTimeParsed);
            case DateTimeType.yMMdd_DateInt:
                if (DateTime.TryParseExact(
                        input, "yyyyMMdd", DateTimeFormatInfo.InvariantInfo, DateTimeStyles.None,
                        out dateTimeParsed))
                {
                    return true;
                }

                if (DateTime.TryParseExact(
                        input, "yyyMMdd", DateTimeFormatInfo.InvariantInfo, DateTimeStyles.None,
                        out dateTimeParsed))
                {
                    return true;
                }

                if (DateTime.TryParseExact(
                        input, "yMMdd", DateTimeFormatInfo.InvariantInfo, DateTimeStyles.None,
                        out dateTimeParsed))
                {
                    if (dateTimeParsed.Year > 100)
                    {
                        // yMMdd parses single digit/2 digit values as 19xx, therefore need to trim to just xx by removing 1900 
                        var centuryYears = -1 * dateTimeParsed.Year / 100 * 100;
                        
                        dateTimeParsed = dateTimeParsed.AddYears(centuryYears);
                    }
                    return true;
                }

                return false;
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
            DateTimeType.Date_yyyy_MM_dd_ISO_8601 => true,
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
            DateTimeType.DateTime_UniversalSortable_yyyy_MM_dd_HH_mm_ss => true,
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
            DateTimeType.Date_yyyy_MM_dd_ISO_8601 => true,
            DateTimeType.DateTime_UTC_ISO_8601 => true,
            DateTimeType.DateTime_UTC_RFC_1123 => true,
            DateTimeType.DateTime_UniversalSortable_yyyy_MM_dd_HH_mm_ss => true,
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