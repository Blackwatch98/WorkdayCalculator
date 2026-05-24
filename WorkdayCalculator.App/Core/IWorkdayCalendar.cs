namespace WorkdayCalculator.App.Core;

public interface IWorkdayCalendar
{
    /// <summary>
    /// Marks a specific date as a holiday.
    /// Holidays are excluded from workdays and will not be considered
    /// during workday calculations.
    /// </summary>
    /// <param name="date">
    /// The date to register as a holiday.
    /// </param>
    public void SetHoliday(DateTime date);
    /// <summary>
    /// Marks a specific date as a holiday every year.
    /// Holidays are excluded from workdays and will not be considered
    /// during workday calculations.
    /// </summary>
    /// <param name="month">
    /// The month of the recurring holiday.
    /// </param>
    /// <param name="day">
    /// The day of the recurring holiday.
    /// </param>
    public void SetRecurringHoliday(int month, int day);
    /// <summary>
    /// Defines the businees hours of the work day.
    /// For instance from 08:00 to 16:00.
    /// </summary>
    /// <param name="startHours">
    /// The hour of the day when the workday starts.
    /// </param>
    /// <param name="startMinutes">
    /// The minute of the hour when the workday starts.
    /// </param>
    /// <param name="stopHours">
    /// The hour of the day when the workday ends.
    /// </param>
    /// <param name="stopMinutes">
    /// The minute of the hour when the workday ends.
    /// </param>
    public void SetWorkdayStartAndStop(int startHours, int startMinutes, int stopHours, int stopMinutes);
    /// <summary>
    /// Calculates the resulting date and time after adding the specified number
    /// of workdays to the given start date.
    /// 
    /// Working hours must first be configured using SetWorkdayStartAndStop.
    /// Holidays and recurring holidays can be configured using SetHoliday
    /// and SetRecurringHoliday.
    /// 
    /// The returned value is always within configured working hours and excludes
    /// weekends and holidays.
    /// </summary>
    /// <param name="startDate">
    /// The starting date and time for the calculation.
    /// </param>
    /// <param name="incrementInWorkdays">
    /// The number of workdays to add or subtract.
    /// Fractional values are supported.
    /// </param>
    /// <returns>
    /// A date and time within working hours representing the calculated workday increment.
    /// </returns>
    public DateTime GetWorkdayIncrement(DateTime startDate, decimal incrementInWorkdays);
}
