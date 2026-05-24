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
        int remainingWorkminutes = (int)(incrementInWorkdays * workMinutesPerDay);
        DateTime currentDateTime = startDate;

        if (direction > 0)
        {
            if (currentDateTime.TimeOfDay < _workdayStart)
                currentDateTime = currentDateTime.Date + _workdayStart;
            else if (currentDateTime.TimeOfDay > _workdayEnd)
            {
                currentDateTime = currentDateTime.Date.AddDays(1) + _workdayStart;
            }
        }
        else if (direction < 0)
        {
            if (currentDateTime.TimeOfDay > _workdayEnd)
                currentDateTime = currentDateTime.Date + _workdayEnd;
            else if (currentDateTime.TimeOfDay < _workdayStart)
            {
                currentDateTime = currentDateTime.Date.AddDays(-1) + _workdayEnd;
            }
        }

        while (true)
        {
            if (currentDateTime.DayOfWeek == DayOfWeek.Saturday || currentDateTime.DayOfWeek == DayOfWeek.Sunday)
            {
                currentDateTime = currentDateTime.AddDays(direction > 0 ? 1 : -1);
                continue;
            }
            if(_holidays.Contains(DateOnly.FromDateTime(currentDateTime)))
            {
                currentDateTime = currentDateTime.AddDays(direction > 0 ? 1 : -1);
                continue;
            }
            if(_recurringHolidays.Contains(new RecurringHoliday(currentDateTime.Month, currentDateTime.Day)))
            {
                currentDateTime = currentDateTime.AddDays(direction > 0 ? 1 : -1);
                continue;
            }

            if(Math.Abs(remainingWorkminutes) < workMinutesPerDay)
            {
                currentDateTime = currentDateTime.AddMinutes(remainingWorkminutes);
                break;
            }
            currentDateTime = currentDateTime.AddDays(direction > 0 ? 1 : -1);

            remainingWorkminutes -= direction * workMinutesPerDay;
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
        if(startHours < 0 || startHours > 23)
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
}
