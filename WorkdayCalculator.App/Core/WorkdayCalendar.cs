namespace WorkdayCalculator.App.Core;

public class WorkdayCalendar : IWorkdayCalendar
{
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
