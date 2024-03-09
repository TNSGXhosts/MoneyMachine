using Core;

namespace Trading.Application.BLL;

public static class DataManagerExtensions
{
    public static DateTime IncreaseDateByTimeframe(this DateTime dateTime, Timeframe timeframe)
    {
        switch (timeframe)
        {
            case Timeframe.DAY:
                return dateTime.AddDays(1);
            case Timeframe.HOUR:
                return dateTime.AddHours(1);
            default:
                throw new ArgumentException("Invalid timeframe");
        }
    }

    public static (DateTime, DateTime) GetPeriod(this Period period)
    {
        var monthsNumber = 0;
        switch (period)
        {
            case Period.MONTH:
                monthsNumber = -1;
                break;
            case Period.YEAR:
                monthsNumber = -12;
                break;
            case Period.YEAR_5:
                monthsNumber = -12 * 5;
                break;
            default:
                throw new ArgumentException("Invalid period");
        }

        var toDate = DateTime.UtcNow.Date.AddDays(-1);
        var fromDate = toDate.AddMonths(monthsNumber);

        return (fromDate, toDate);
    }
}
