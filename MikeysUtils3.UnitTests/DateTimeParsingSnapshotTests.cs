using Meziantou.Framework.InlineSnapshotTesting;
using MikeysUtils3.Pages.DateTimePage;

namespace MikeysUtils3.UnitTests;

public class DateTimeParsingSnapshotTests
{
    record Result(DateTimeType DateTimeType, string ParsedDateTime, DateTimeKind Kind);
    
    [Test]
    public async Task SnapshotTest()
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