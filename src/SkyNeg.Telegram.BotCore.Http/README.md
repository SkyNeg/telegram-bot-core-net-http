# Telegram Bot Core library Middleware
Allow use SkyNeg Telegram Bot Core library with Webhooks

## Usage
```C#
using SkyNeg.Telegram.BotCore.Http;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddBot(builder.Configuration.GetSection("TelegramBot"));

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.MapTelegramBotWebhook("/telegramWebHook");

await app.RunAsync();
```
