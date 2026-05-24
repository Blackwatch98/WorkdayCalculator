using WorkdayCalculator.App.Models;

namespace WorkdayCalculator.App.Core;

public class WorkdayCalendar : IWorkdayCalendar
{
    private TimeSpan _workdayStart;
    private TimeSpan _workdayEnd;
    private HashSet<DateOnly> _holidays = [];
    private HashSet<RecurringHoliday> _recurringHolidays = [];
    public DateTime GetWorkdayIncrement(DateTime startDate, decimal incrementInWorkdays)
    {
        throw new NotImplementedException();
    }

    public void SetHoliday(DateTime date)
    {
        throw new NotImplementedException();
    }

    public void SetRecurringHoliday(int month, int day)
    {
        throw new NotImplementedException();
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
