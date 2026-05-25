namespace WorkdayCalculator.App.Models;

public sealed record class WorkTimeRange
{
    public TimeSpan Start { get; }
    public TimeSpan End { get; }
    public TimeSpan Duration => End - Start;

    private WorkTimeRange(TimeSpan start, TimeSpan end)
    {
        if (start >= end)
            throw new ArgumentException("Workday end must be after start time.");

        Start = start;
        End = end;
    }
    public static WorkTimeRange Create(int startHours, int startMinutes, int endHours, int endMinutes)
    {
        if (!IsHoursValid(startHours))
            throw new ArgumentOutOfRangeException(nameof(startHours));

        if (!IsHoursValid(endHours))
            throw new ArgumentOutOfRangeException(nameof(endHours));

        if (!IsMinutesValid(startMinutes))
            throw new ArgumentOutOfRangeException(nameof(startMinutes));

        if (!IsMinutesValid(endMinutes))
            throw new ArgumentOutOfRangeException(nameof(endMinutes));

        return new WorkTimeRange(
            new TimeSpan(startHours, startMinutes, 0),
            new TimeSpan(endHours, endMinutes, 0));
    }

    private static bool IsHoursValid(int hours) => hours >= 0 && hours <= 23;
    private static bool IsMinutesValid(int minutes) => minutes >= 0 && minutes <= 59;
}
