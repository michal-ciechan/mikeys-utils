using Meziantou.Framework.InlineSnapshotTesting;
using MikeysUtils3.Pages.DateTimePage;

namespace MikeysUtils3.UnitTests;

public class DateTimeParsingSnapshotTests
{
    record Result(DateTimeType DateTimeType, string ParsedDateTime, DateTimeKind Kind);
    
    [Test]
    public async Task UniversaleSortableNoT1()
    {
        var input = "2024-10-23 21:39:40.1234567";

        var results = Parse(input);

        InlineSnapshot.Validate(
            results,
            """
            - DateTimeType: DateTime_UniversalSortable_yyyy_MM_dd_HH_mm_ss
              ParsedDateTime: 2024-10-23T21:39:40.1234567
            """
        );
    }
    
    [Test]
    public async Task UniversaleSortableNoTSingleMillisecond()
    {
        var input = "2024-10-23 21:39:40.1";

        var results = Parse(input);

        InlineSnapshot.Validate(
            results,
            """
            - DateTimeType: DateTime_UniversalSortable_yyyy_MM_dd_HH_mm_ss
              ParsedDateTime: 2024-10-23T21:39:40.1000000
            """
        );
    }
    
    [Test]
    public async Task UniversaleSortableNoTNoMilliseconds()
    {
        var input = "2024-10-23 21:39:40";

        var results = Parse(input);

        InlineSnapshot.Validate(
            results,
            """
            - DateTimeType: DateTime_UniversalSortable_yyyy_MM_dd_HH_mm_ss
              ParsedDateTime: 2024-10-23T21:39:40.0000000
            """
        );
    }
    
    [Test]
    public async Task UniversaleSortableNoTHasFullstop()
    {
        var input = "2024-10-23 21:39:40.";

        var results = Parse(input);

        InlineSnapshot.Validate(
            results,
            """
            - DateTimeType: DateTime_UniversalSortable_yyyy_MM_dd_HH_mm_ss
              ParsedDateTime: 2024-10-23T21:39:40.0000000
            """
        );
    }
    
    [Test]
    public async Task UniversaleSortableNoTNoSecondsTrailingColon()
    {
        var input = "2024-10-23 21:39:";

        var results = Parse(input);

        InlineSnapshot.Validate(
            results,
            """
            - DateTimeType: DateTime_UniversalSortable_yyyy_MM_dd_HH_mm_ss
              ParsedDateTime: 2024-10-23T21:39:00.0000000
            """
        );
    }
    
    [Test]
    public async Task UniversaleSortableNoTNoSeconds()
    {
        var input = "2024-10-23 21:39";

        var results = Parse(input);

        InlineSnapshot.Validate(
            results,
            """
            - DateTimeType: DateTime_UniversalSortable_yyyy_MM_dd_HH_mm_ss
              ParsedDateTime: 2024-10-23T21:39:00.0000000
            """
        );
    }
    
    [Test]
    public async Task UniversaleSortableNoTNoMinutesTrailingColon()
    {
        var input = "2024-10-23 21:";

        var results = Parse(input);

        InlineSnapshot.Validate(
            results,
            """
            - DateTimeType: DateTime_UniversalSortable_yyyy_MM_dd_HH_mm_ss
              ParsedDateTime: 2024-10-23T21:00:00.0000000
            """
        );
    }
    
    [Test]
    public async Task UniversaleSortableNoTNoMinutes()
    {
        var input = "2024-10-23 21";

        var results = Parse(input);

        InlineSnapshot.Validate(
            results,
            """
            - DateTimeType: DateTime_UniversalSortable_yyyy_MM_dd_HH_mm_ss
              ParsedDateTime: 2024-10-23T21:00:00.0000000
            """
        );
    }
    
    [Test]
    public async Task DateIntSimple()
    {
        var input = "20241023";

        var results = Parse(input);

        InlineSnapshot.Validate(
            results,
            """
            - DateTimeType: UnixSeconds
              ParsedDateTime: 1970-08-23T06:30:23.0000000Z
              Kind: Utc
            - DateTimeType: UnixMilliseconds
              ParsedDateTime: 1970-01-01T05:37:21.0230000Z
              Kind: Utc
            - DateTimeType: UnixNanoSeconds
              ParsedDateTime: 1970-01-01T00:00:00.0202410Z
              Kind: Utc
            - DateTimeType: yyyyMMdd_DateBasic
              ParsedDateTime: 2024-10-23T00:00:00.0000000
            - DateTimeType: yMMdd_DateInt
              ParsedDateTime: 2024-10-23T00:00:00.0000000
            """
        );
    }
    
    [Test]
    public async Task DateIntDoubleTripleDigitYear()
    {
        var input = "1691023";

        var results = Parse(input);

        InlineSnapshot.Validate(
            results,
            """
            - DateTimeType: DayNumber
              ParsedDateTime: 4630-11-13T00:00:00.0000000
            - DateTimeType: UnixSeconds
              ParsedDateTime: 1970-01-20T13:43:43.0000000Z
              Kind: Utc
            - DateTimeType: UnixMilliseconds
              ParsedDateTime: 1970-01-01T00:28:11.0230000Z
              Kind: Utc
            - DateTimeType: UnixNanoSeconds
              ParsedDateTime: 1970-01-01T00:00:00.0016910Z
              Kind: Utc
            - DateTimeType: yMMdd_DateInt
              ParsedDateTime: 0169-10-23T00:00:00.0000000
            """
        );
    }
    
    [Test]
    public async Task DateIntDoubleDigitYear()
    {
        var input = "691023";

        var results = Parse(input);

        InlineSnapshot.Validate(
            results,
            """
            - DateTimeType: DayNumber
              ParsedDateTime: 1892-12-16T00:00:00.0000000
            - DateTimeType: UnixSeconds
              ParsedDateTime: 1970-01-08T23:57:03.0000000Z
              Kind: Utc
            - DateTimeType: UnixMilliseconds
              ParsedDateTime: 1970-01-01T00:11:31.0230000Z
              Kind: Utc
            - DateTimeType: UnixNanoSeconds
              ParsedDateTime: 1970-01-01T00:00:00.0006910Z
              Kind: Utc
            - DateTimeType: yMMdd_DateInt
              ParsedDateTime: 0069-10-23T00:00:00.0000000
            """
        );
    }
    
    [Test]
    public async Task RoundtripStandard_NoMilliSeconds()
    {
        var input = "2024-10-16T17:03:43Z";

        var results = Parse(input);

        InlineSnapshot.Validate(
            results,
            """
            - DateTimeType: DateTime_UTC_ISO_8601
              ParsedDateTime: 2024-10-16T17:03:43.0000000Z
              Kind: Utc
            """
        );
    }
    
    [Test]
    public async Task DateTime_RFC1123()
    {
        var input = "Mon, 30 Sep 2024 17:58:47 GMT";

        var results = Parse(input);

        InlineSnapshot.Validate(
            results,
            """
            - DateTimeType: DateTime_UTC_RFC_1123
              ParsedDateTime: 2024-09-30T17:58:47.0000000Z
              Kind: Utc
            """
        );
    }
    
    [Test]
    public async Task DateTime_BasicInt_WithSuffix()
    {
        var input = "20240101-";

        var results = Parse(input);

        InlineSnapshot.Validate(
            results,
            """
            - DateTimeType: UnixSeconds
              ParsedDateTime: 1970-08-23T06:15:01.0000000Z
              Kind: Utc
            - DateTimeType: UnixMilliseconds
              ParsedDateTime: 1970-01-01T05:37:20.1010000Z
              Kind: Utc
            - DateTimeType: UnixNanoSeconds
              ParsedDateTime: 1970-01-01T00:00:00.0202401Z
              Kind: Utc
            - DateTimeType: yyyyMMdd_DateBasic
              ParsedDateTime: 2024-01-01T00:00:00.0000000
            - DateTimeType: yMMdd_DateInt
              ParsedDateTime: 2024-01-01T00:00:00.0000000
            """
        );
    }
    
    [Test]
    public async Task DateTime_BasicInt_WithHourOnly()
    {
        var input = "20240101 23";

        var results = Parse(input);

        InlineSnapshot.Validate(
            results,
            """
            - DateTimeType: DateTimeBasic_yyyyMMdd_HHmmssfffffff
              ParsedDateTime: 2024-01-01T23:00:00.0000000
            """
        );
    }
    
    private static List<Result> Parse(string input)
    {
        List<Result> results = [];

        foreach (var dateTimeType in Enum.GetValues<DateTimeType>())
        {
            if (input.TryParseAsDateTime(dateTimeType, out var result))
            {
                var parsedResult = new Result(dateTimeType, result.ToString("O"), result.Kind);

                results.Add(parsedResult);
            }
        }

        return results;
    }
}