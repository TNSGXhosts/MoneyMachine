﻿using Core;

using Skender.Stock.Indicators;

namespace Trading.Application.BLL;

public static class TestProcessExtensions
{
    public static DateTime GetPreviousDate(this DateTime dateTime, Timeframe timeframe)
    {
        switch (timeframe)
        {
            case Timeframe.HOUR:
                return dateTime.AddHours(-1);
            case Timeframe.DAY:
                return dateTime.AddDays(-1);
            default:
                throw new ArgumentException("Invalid timeframe value", nameof(timeframe));
        }
    }

    public static DateTime[] GetPreviousDates(this DateTime dateTime, Timeframe timeframe, int numberOfDates)
    {
        var dates = new DateTime[numberOfDates];
        dates[numberOfDates - 1] = dateTime.GetPreviousDate(timeframe);

        for (var i = numberOfDates - 2; i >= 0; i--)
        {
            dates[i] = dates[i + 1].GetPreviousDate(timeframe);
        }

        return dates;
    }

    public static DateTime GetPreviousWeekDate(this DateTime dateTime, Timeframe timeframe)
    {
        switch (timeframe)
        {
            case Timeframe.HOUR:
                return dateTime.AddHours(-7);
            case Timeframe.DAY:
                return dateTime.AddDays(-7 * 24);
            default:
                throw new ArgumentException("Invalid timeframe value", nameof(timeframe));
        }
    }

    public static Quote GetAskPrice(this Dictionary<DateTime, EpicTestData> epicData, DateTime date)
    {
        if (epicData?.TryGetValue(date, out var price) == true)
        {
            return price.AskPrice;
        }

        return new Quote();
    }
}
