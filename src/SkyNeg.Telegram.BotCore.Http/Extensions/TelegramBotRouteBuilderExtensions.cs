using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using System.Diagnostics.CodeAnalysis;

namespace SkyNeg.Telegram.BotCore.Web
{
    public static class TelegramBotRouteBuilderExtensions
    {
        private const string DefaultDisplayName = "Telegram Bot";

        public static IEndpointConventionBuilder MapTelegramBotWebhook(this IEndpointRouteBuilder endpoints, [StringSyntax("Route")] string pattern)
        {
            ArgumentNullException.ThrowIfNull(endpoints);

            return MapTelegramBotWebhookCore(endpoints, pattern);
        }

        private static IEndpointConventionBuilder MapTelegramBotWebhookCore(IEndpointRouteBuilder endpoints, string pattern)
        {
            var pipeline = endpoints.CreateApplicationBuilder()
               .UseMiddleware<TelegramBotMiddleware>()
               .Build();

            return endpoints.MapPost(pattern, pipeline).WithDisplayName(DefaultDisplayName);
            //endpoints.MapPost(pattern, (Update update) => HandleUpdate(update));
        }
    }
}
