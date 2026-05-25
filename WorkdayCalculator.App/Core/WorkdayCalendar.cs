using WorkdayCalculator.App.Models;

namespace WorkdayCalculator.App.Core;

public class WorkdayCalendar : IWorkdayCalendar
{
    private WorkTimeRange? _workTimeRange;
    private WorkTimeRange WorkTime => _workTimeRange
        ?? throw new InvalidOperationException(
            $"Workday start/stop must be set via {nameof(SetWorkdayStartAndStop)} before use.");
    private readonly HashSet<DateOnly> _holidays = [];
    private readonly HashSet<RecurringHoliday> _recurringHolidays = [];
    public DateTime GetWorkdayIncrement(DateTime startDate, decimal incrementInWorkdays)
    {
        if (incrementInWorkdays == 0)
            return startDate;

        int direction = Math.Sign(incrementInWorkdays);
        int workMinutesPerDay = (int)(WorkTime.Duration.TotalMinutes);
        int remainingWorkMinutes = Math.Abs((int)(incrementInWorkdays * workMinutesPerDay));

        DateTime currentDateTime = NormalizeToWorkdayBoundary(startDate, direction);

        while (remainingWorkMinutes > 0)
        {
            if (!IsWorkday(currentDateTime))
            {
                currentDateTime = MoveToNextDayBoundary(currentDateTime, direction);
                continue;
            }

            int availableMinutesInCurrentDay = GetAvailableWorkMinutes(currentDateTime, direction);

            if (remainingWorkMinutes <= availableMinutesInCurrentDay)
                return AddWorkMinutes(currentDateTime, remainingWorkMinutes, direction);

            remainingWorkMinutes -= availableMinutesInCurrentDay;
            currentDateTime = MoveToNextDayBoundary(currentDateTime, direction);
        }

        return currentDateTime;
    }

    public void SetHoliday(DateTime date) => _holidays.Add(DateOnly.FromDateTime(date));

    public void SetRecurringHoliday(int month, int day)
    {
        if (month < 1 || month > 12)
            throw new ArgumentOutOfRangeException(nameof(month));

        if (day < 1 || day > 31)
            throw new ArgumentOutOfRangeException(nameof(day));

        _recurringHolidays.Add(new RecurringHoliday(month, day));
    }

    public void SetWorkdayStartAndStop(int startHours, int startMinutes, int stopHours, int stopMinutes) 
        => _workTimeRange = WorkTimeRange.Create(startHours, startMinutes, stopHours, stopMinutes);

    public override string ToString()
    {
        return $"Workday: {WorkTime.Start} - {WorkTime.End}, " +
               $"Holidays: {_holidays.Count}, " +
               $"Recurring: {_recurringHolidays.Count}";
    }

    private DateTime NormalizeToWorkdayBoundary(DateTime currentDateTime, int direction)
    {
        if (currentDateTime.TimeOfDay < WorkTime.Start)
        {
            return direction > 0
                ? currentDateTime.Date + WorkTime.Start
                : currentDateTime.Date.AddDays(-1) + WorkTime.End;
        }

        if (currentDateTime.TimeOfDay > WorkTime.End)
        {
            return direction > 0
                ? currentDateTime.Date.AddDays(1) + WorkTime.Start
                : currentDateTime.Date + WorkTime.End;
        }

        return currentDateTime;
    }

    private bool IsWorkday(DateTime date)
    {
        if (date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday)
            return false;

        if (_holidays.Contains(DateOnly.FromDateTime(date)))
            return false;

        if (_recurringHolidays.Contains(new RecurringHoliday(date.Month, date.Day)))
            return false;

        return true;
    }

    private DateTime MoveToNextDayBoundary(DateTime currentDateTime, int direction)
    {
        return direction > 0
            ? currentDateTime.Date.AddDays(1) + WorkTime.Start
            : currentDateTime.Date.AddDays(-1) + WorkTime.End;
    }

    private int GetAvailableWorkMinutes(DateTime currentDateTime, int direction)
    {
        return direction > 0
            ? (int)(WorkTime.End - currentDateTime.TimeOfDay).TotalMinutes
            : (int)(currentDateTime.TimeOfDay - WorkTime.Start).TotalMinutes;
    }

    private static DateTime AddWorkMinutes(DateTime dateTime, int minutes, int direction)
    {
        return dateTime.AddMinutes(direction * minutes);
    }
}
