# Workday Calculator

A simple C# application for calculating dates based on working days.

The calculator allows you to define working hours, one-time holidays, and recurring holidays. It can then add or subtract a given number of workdays from a selected start date.

## Features

- Configure workday start and end time
- Add one-time holidays
- Add recurring holidays
- Add or subtract fractional workdays
- Skip weekends and configured holidays during calculations

## Example

```csharp
IWorkdayCalendar calendar = new WorkdayCalendar();

calendar.SetWorkdayStartAndStop(8, 0, 16, 0);
calendar.SetRecurringHoliday(5, 17);
calendar.SetHoliday(new DateTime(2004, 5, 27));

var start = new DateTime(2004, 5, 24, 18, 5, 0);
decimal increment = -5.5m;

var result = calendar.GetWorkdayIncrement(start, increment);

Console.WriteLine(result);