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
}
