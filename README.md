# MoneyMachine

MoneyMachine is a trading project that allows you to create, edit, and delete deals/orders through a Telegram bot.

## Requirements

To run the project, you need to create a Telegram bot on the official website and create an account with Capital.com. You also need to create an `appsettings.json` file in `Trading/Application` with the following template:

```json
{
    "Logging": {
        "Console": {
            "LogLevel": {
                "Microsoft.Hosting.Lifetime": "Trace"
            }
        }
    },
    "TelegramSettings": {
        "AccessToken": "",
        "ChatId": ""
    },
    "SchedulerSettings": {
        "CapitalTradingHandlerJobScheduleInterval": "00:00:30"
    },
    "CapitalIntegrationSettings": {
        "ApiKey": "",
        "Identifier": "",
        "Password": "",
        "Secret": "",
        "BaseUrl": ""
    },
    "ConnectionStrings": {
        "Sqlite": ""
    }
}
```

## Usage

The current version of the project allows you to create, edit, and delete deals/orders through a Telegram bot.

## Future of the project

In future versions of the project, we plan to add the following features:

- Generate reports with deal statistics.
- Add integration with a second broker.
- Auto Calculation order size depending on % risk.
- Algorithmic trading strategy.
- Uploading historical data to the database for Job processing.
- Backtest Job.
