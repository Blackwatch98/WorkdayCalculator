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
        throw new NotImplementedException();
    }
}
