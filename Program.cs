using System;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Polling;

var token = "8458353334:AAGLS1kGunBNVW4JYuOP103mVB1tqCP3j_0";

var bot = new TelegramBotClient(token);

using var cts = new CancellationTokenSource();

bot.StartReceiving(
    HandleUpdateAsync,
    HandleErrorAsync,
    cancellationToken: cts.Token
);

var me = await bot.GetMe();
Console.WriteLine($"Bot @{me.Username} started");

Console.ReadLine();
cts.Cancel();

async Task HandleUpdateAsync(
    ITelegramBotClient bot,
    Update update,
    CancellationToken ct
)
{
    if (update.Message is { Text: { } text})
    {
        await bot.SendMessage(
            update.Message.Chat.Id,
            $"Ты написал: {text}"
        );
    };
}

Task HandleErrorAsync(
    ITelegramBotClient bot,
    Exception exception,
    CancellationToken ct
)
{
    Console.WriteLine(exception.Message);
    return Task.CompletedTask;
};