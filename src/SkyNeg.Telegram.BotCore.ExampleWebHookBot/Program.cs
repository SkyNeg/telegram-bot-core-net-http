using SkyNeg.Telegram.BotCore.Web;

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
app.Map("/", () => "Ok");
app.Map("/Error", () => "Error");

app.Run();