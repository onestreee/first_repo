using System;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Polling;
using DotNetEnv;
using MusicIdeaBot.Services;
using System.Linq.Expressions;
using Telegram.Bot.Types.ReplyMarkups;

Env.Load();

var token = Environment.GetEnvironmentVariable("BOT_TOKEN");

if (string.IsNullOrEmpty(token))
{
    Console.WriteLine("BOT_TOKEN not found in .env");
    return;
}

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
    if (update.Type == Telegram.Bot.Types.Enums.UpdateType.CallbackQuery)
    {
        var data = update.CallbackQuery.Data;
        var chatId = update.CallbackQuery.Message.Chat.Id;

        if (data == "idea_more")
        {
            var idea = MusicIdeaBot.Services.MusicIdeaService.GenerateIdea();

            var keyboard = new InlineKeyboardMarkup(
                InlineKeyboardButton.WithCallbackData(
                    "Ещё идея!",
                    "idea_more"));

            await bot.SendMessage(
                chatId,
                idea,
                replyMarkup: keyboard,
                cancellationToken: ct);
            return;
        }
    }

    try
    {
        if (update.Type != Telegram.Bot.Types.Enums.UpdateType.Message) return;

        if (update.Message?.Text == null) return;

        var text = update.Message.Text;
        var chatId = update.Message.Chat.Id;

        if (text.StartsWith("/start"))
        {
            await bot.SendMessage(
                chatId,
                "Привет, я бот для генерации музыкальных идей.\nНапиши /idea или /chords",
                cancellationToken: ct);
            return;
        }

        if (text.StartsWith("/chords"))
        {
            var parts = text.Split(' ');

            if (parts.Length < 2)
            {
                await bot.SendMessage(
                    chatId,
                    "Использование: /chords sad | dark | pop",
                    cancellationToken: ct);
                return;
            }

            var mood = parts[1];
            var prog = MusicIdeaBot.Services.ChordService.GetProgression(mood);

            await bot.SendMessage(
                chatId,
                $"Прогрессия: \n{prog}",
                cancellationToken: ct);
            return;
        }

        if (text.StartsWith("/idea"))
        {
            var idea = MusicIdeaBot.Services.MusicIdeaService.GenerateIdea();

            var keyboard = new InlineKeyboardMarkup(
                InlineKeyboardButton.WithCallbackData(
                    "Ещё идея!",
                    "idea_more"));

            await bot.SendMessage(
                chatId,
                idea,
                replyMarkup: keyboard,
                cancellationToken: ct);
            return;
        }

        await bot.SendMessage(
            chatId,
            "Не понял команду.\nПопробуй /idea или /chords",
            cancellationToken: ct);

    }
    catch (Exception ex)
    {
        Console.WriteLine("UPDATE ERROR");
        Console.WriteLine(ex);
    }
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