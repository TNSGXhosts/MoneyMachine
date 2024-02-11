using Microsoft.Extensions.Logging;

using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

using Trading.Application.Handlers;
using Trading.Application.TelegramConstants;
using Trading.Application.UserContext;

namespace Trading.Application.TelegramIntegration;

public class CallbackHandler(ILogger<CallbackHandler> logger,
                            IUserContext userContext,
                            IEnumerable<IHandler> handlers,
                            ICallbackParser callbackParser) : ICallbackHandler
{
    public Tuple<string, InlineKeyboardMarkup> HandleCallback(CallbackQuery callbackQuery)
    {
        Tuple<string, InlineKeyboardMarkup>? reply = new Tuple<string, InlineKeyboardMarkup>(string.Empty, null);
        try
        {
            var parsedData = callbackParser.ParseCallbackData(callbackQuery?.Data);
            if (parsedData != null)
            {
                userContext.CatchEvent(parsedData.Item1);

                logger.LogInformation($"Parsed trigger: {parsedData.Item1}, new state: {userContext.State}");

                var handler = handlers.FirstOrDefault(h => h.Trigger == parsedData.Item1);
                reply = handler?.Handle(string.IsNullOrEmpty(parsedData.Item2) ? parsedData.Item1.ToString() : parsedData.Item2);
            }
            else if (userContext.State != States.Start)
            {
                logger.LogInformation($"Can't parse callback data as trigger: {callbackQuery.Data} -  used as select");

                var hasError = userContext.ExecuteUserInputPipeline(callbackQuery.Data);

                reply = new Tuple<string, InlineKeyboardMarkup>(hasError ? "Operation failed" : "Operation have processed successfully",
                    new InlineKeyboardMarkup(new[]
                    {
                        new[]
                        {
                            InlineKeyboardButton.WithCallbackData("Go back", nameof(Triggers.Start)),
                        }
                    })
                );
            }
        }
        catch (Exception e)
        {
            logger.LogError($"{e.Message} - {e.InnerException}");
        }

        return reply ?? new Tuple<string, InlineKeyboardMarkup>(string.Empty, null);
    }
}