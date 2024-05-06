using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
#if NET7_0_OR_GREATER
using System.Diagnostics.CodeAnalysis;
#endif

namespace SkyNeg.Telegram.BotCore.Web
{
    public static class TelegramBotRouteBuilderExtensions
    {
        private const string DefaultDisplayName = "Telegram Bot";

#if NET7_0_OR_GREATER
        public static IEndpointConventionBuilder MapTelegramBotWebhook(this IEndpointRouteBuilder endpoints, [StringSyntax("Route")] string pattern)
#else
        public static IEndpointConventionBuilder MapTelegramBotWebhook(this IEndpointRouteBuilder endpoints, string pattern)
#endif
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
        }
    }
}
