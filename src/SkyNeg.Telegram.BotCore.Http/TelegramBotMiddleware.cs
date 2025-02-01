using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using SkyNeg.Telegram.Bot.Abstraction;
using System.Text.Json;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace SkyNeg.Telegram.BotCore.Web
{
    public class TelegramBotMiddleware
    {
        const string TelegramBotApiSecretTokenHeader = "X-Telegram-Bot-Api-Secret-Token";
        readonly RequestDelegate _next;
        readonly IBotService _botService;

        public TelegramBotMiddleware(RequestDelegate next, IBotService botService)
        {
            ArgumentNullException.ThrowIfNull(next);
            ArgumentNullException.ThrowIfNull(botService);

            _next = next;
            _botService = botService;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            ArgumentNullException.ThrowIfNull(httpContext);

            //Check secret token header
            if (!httpContext.Request.Headers.TryGetValue(TelegramBotApiSecretTokenHeader, out StringValues token) || !token.Any(q => string.Equals(q, _botService.SecretToken, StringComparison.Ordinal)))
            {
                httpContext.Response.StatusCode = 403;
                return;
            }

            //Read request
            try
            {
                Update? update = null;
                string data = string.Empty;
                using (var stream = httpContext.Request.Body)
                using (var reader = new StreamReader(stream))
                {
                    data = await reader.ReadToEndAsync();
                }

                update = JsonSerializer.Deserialize<Update>(data, JsonBotAPI.Options);
                if (update != null)
                {
                    var updateResponse = await _botService.HandleUpdateAsync(update, httpContext.RequestAborted);
                    var responseData = JsonSerializer.Serialize(updateResponse, JsonBotAPI.Options);
                    httpContext.Response.StatusCode = 200;
                    await httpContext.Response.WriteAsJsonAsync(responseData, httpContext.RequestAborted);
                }
            }
            catch (Exception)
            {
                //Response with 200 to avoid receiving the same request that can not be processed over and over again
                httpContext.Response.StatusCode = 200;
            }
        }
    }
}
