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