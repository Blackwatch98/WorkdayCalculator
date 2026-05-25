using Shouldly;
using WorkdayCalculator.App.Core;

namespace WorkdayCalculator.Tests;

public class WorkdayCalendarTests
{
    public static IEnumerable<object[]> WorkdayIncrementTestCases =>
    [
        [
            new DateTime(2004, 5, 24, 18, 5, 0),
            -5.5m,
            new DateTime(2004, 5, 14, 12, 0, 0)
        ],
        [
            new DateTime(2004, 5, 24, 19, 3, 0),
            44.723656m,
            new DateTime(2004, 7, 27, 13, 47, 0)
        ],
        [
            new DateTime(2004, 5, 24, 18, 3, 0),
            -6.7470217m,
            new DateTime(2004, 5, 13, 10, 2, 0)
        ],
        [
            new DateTime(2004, 5, 24, 8, 3, 0),
            12.782709m,
            new DateTime(2004, 6, 10, 14, 18, 0)
        ],
        [
            new DateTime(2004, 5, 24, 7, 3, 0),
            8.276628m,
            new DateTime(2004, 6, 4, 10, 12, 0)
        ]
    ];

    [Theory]
    [MemberData(nameof(WorkdayIncrementTestCases))]
    public void GetWorkdayIncrement_ShouldReturnExpectedDate(
        DateTime startDate,
        decimal incrementInWorkdays,
        DateTime expectedDate)
    {
        // Arrange
        IWorkdayCalendar calendar = CreateCalendar();

        // Act
        var result = calendar.GetWorkdayIncrement(startDate, incrementInWorkdays);

        // Assert
        result.ShouldBe(expectedDate);
    }

    private static IWorkdayCalendar CreateCalendar()
    {
        IWorkdayCalendar calendar = new WorkdayCalendar();

        calendar.SetWorkdayStartAndStop(8, 0, 16, 0);
        calendar.SetRecurringHoliday(5, 17);
        calendar.SetHoliday(new DateTime(2004, 5, 27));

        return calendar;
    }
}
