using SkyNeg.Telegram.BotCore.Controllers.Abstraction;
using SkyNeg.Telegram.BotCore.Extensions;
using SkyNeg.Telegram.BotCore.Models;
using SkyNeg.Telegram.BotCore.Requests;
using SkyNeg.Telegram.BotCore.WebHookBotSample.Requests;
using System.Diagnostics;
using Telegram.Bot;

namespace SkyNeg.Telegram.BotCore.WebHookBotSample.Controllers
{
    public class WebhookRequestController : RequestController<WebhookRequest>
    {
        public WebhookRequestController(ILogger<WebhookRequestController> logger, ITelegramBotClient client) : base(logger, client)
        {
        }

        public async override Task<RequestResult> ExecuteAsync(WebhookRequest request, RequestContext context, CancellationToken cancellationToken)
        {
            Logger.LogInformation($"{DateTime.Now} Received request {request}");
            Stopwatch sw = Stopwatch.StartNew();
            var callbackData = new ClearCacheRequest() { Type = null, Text = "qwe", }.ToCallbackData();
            sw.Stop();
            var message = $"{callbackData} {{{sw.ElapsedTicks} ticks}}";
            await Client.SendMessage(chatId: context.Chat.Id, message, cancellationToken: cancellationToken);

            return RequestResult.Completed;
        }
    }
}
