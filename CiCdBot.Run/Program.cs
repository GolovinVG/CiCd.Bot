using System;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace CiCdBot.Run
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var token = "1717008846:AAH1ABoirW5BupdwN0SDmvL8p2b-6jPJves";

            var client = new TelegramBotClient(token);

            var cts = new CancellationTokenSource();
            var cancellationToken = cts.Token;
            var receiverOptions = new ReceiverOptions
            {
                AllowedUpdates = { }, // receive all update types
            };
            
            client.StartReceiving(HandleUpdateAsync,
                HandleErrorAsync,
                receiverOptions,
                cancellationToken);


            Console.ReadLine();

        }

        public static async Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
        {
            // Некоторые действия
            Console.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(exception));
        }

        public static async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            // Некоторые действия
            Console.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(update));
            if(update.Type == UpdateType.Message)
            {
                var message = update.Message;
                if (message.Text.ToLower() == "/start")
                {
                    await botClient.SendTextMessageAsync(message.Chat, "Добро пожаловать на борт, добрый путник! /Start");
                    return;
                }
                await botClient.SendTextMessageAsync(message.Chat, "Привет-привет!!");
            }
        }
    }
}