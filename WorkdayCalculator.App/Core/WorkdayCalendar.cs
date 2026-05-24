using WorkdayCalculator.App.Models;

namespace WorkdayCalculator.App.Core;

public class WorkdayCalendar : IWorkdayCalendar
{
    private TimeSpan _workdayStart;
    private TimeSpan _workdayEnd;
    private readonly HashSet<DateOnly> _holidays = [];
    private readonly HashSet<RecurringHoliday> _recurringHolidays = [];
    public DateTime GetWorkdayIncrement(DateTime startDate, decimal incrementInWorkdays)
    {
        if (incrementInWorkdays == 0)
            return startDate;

        int direction = Math.Sign(incrementInWorkdays);
        int workMinutesPerDay = (int)(_workdayEnd - _workdayStart).TotalMinutes;
        int remainingWorkMinutes = Math.Abs((int)(incrementInWorkdays * workMinutesPerDay));

        DateTime currentDateTime = NormalizeToWorkdayBoundary(startDate, direction);

        while (remainingWorkMinutes > 0)
        {
            if (!IsWorkday(currentDateTime))
            {
                currentDateTime = MoveToNextDayBoundary(currentDateTime, direction);
                continue;
            }

            int availableMinutesInCurrentDay = GetAvailableWorkMinutesInCurrentDay(currentDateTime, direction);

            if (remainingWorkMinutes <= availableMinutesInCurrentDay)
                return AddWorkMinutes(currentDateTime, remainingWorkMinutes, direction);

            remainingWorkMinutes -= availableMinutesInCurrentDay;
            currentDateTime = MoveToNextDayBoundary(currentDateTime, direction);
        }

        return currentDateTime;
    }

    public void SetHoliday(DateTime date)
    {
        _holidays.Add(DateOnly.FromDateTime(date));
    }

    public void SetRecurringHoliday(int month, int day)
    {
        if (month < 1 || month > 12)
            throw new ArgumentOutOfRangeException(nameof(month));

        if (day < 1 || day > 31)
            throw new ArgumentOutOfRangeException(nameof(day));

        _recurringHolidays.Add(new RecurringHoliday(month, day));
    }

    public void SetWorkdayStartAndStop(int startHours, int startMinutes, int stopHours, int stopMinutes)
    {
        if (startHours < 0 || startHours > 23)
            throw new ArgumentOutOfRangeException(nameof(startHours));

        if (stopHours < 0 || stopHours > 23)
            throw new ArgumentOutOfRangeException(nameof(stopHours));

        if (startMinutes < 0 || startMinutes > 59)
            throw new ArgumentOutOfRangeException(nameof(startMinutes));

        if (stopMinutes < 0 || stopMinutes > 59)
            throw new ArgumentOutOfRangeException(nameof(stopMinutes));

        TimeSpan start = new TimeSpan(startHours, startMinutes, 0);
        TimeSpan stop = new TimeSpan(stopHours, stopMinutes, 0);

        if (stop <= start)
            throw new ArgumentException("Workday end must be after start time.");

        _workdayStart = start;
        _workdayEnd = stop;
    }

    public override string ToString()
    {
        return $"Workday: {_workdayStart} - {_workdayEnd}, " +
               $"Holidays: {_holidays.Count}, " +
               $"Recurring: {_recurringHolidays.Count}";
    }

    private DateTime NormalizeToWorkdayBoundary(DateTime currentDateTime, int direction)
    {
        if (currentDateTime.TimeOfDay < _workdayStart)
        {
            return direction > 0
                ? currentDateTime.Date + _workdayStart
                : currentDateTime.Date.AddDays(-1) + _workdayEnd;
        }

        if (currentDateTime.TimeOfDay > _workdayEnd)
        {
            return direction > 0
                ? currentDateTime.Date.AddDays(1) + _workdayStart
                : currentDateTime.Date + _workdayEnd;
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
            ? currentDateTime.Date.AddDays(1) + _workdayStart
            : currentDateTime.Date.AddDays(-1) + _workdayEnd;
    }

    private int GetAvailableWorkMinutesInCurrentDay(DateTime currentDateTime, int direction)
    {
        return direction > 0
            ? (int)(_workdayEnd - currentDateTime.TimeOfDay).TotalMinutes
            : (int)(currentDateTime.TimeOfDay - _workdayStart).TotalMinutes;
    }

    private static DateTime AddWorkMinutes(DateTime dateTime, int minutes, int direction)
    {
        return dateTime.AddMinutes(direction * minutes);
    }
}
