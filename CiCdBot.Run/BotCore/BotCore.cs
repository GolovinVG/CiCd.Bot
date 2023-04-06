using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using CiCdBot.Run.BotCore.ChatLifeCycle;
using CiCdBot.Run.BotCore.Workflow;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace CiCdBot.Run.BotCore
{
    public class BotEngine
    {
        private IBaseMessageHandler _messageHandler;
        private IHost host;

        public BotEngine(IHost host)
        {
            this.host = host;
        }

        internal void Run(TelegramBotClient client)
        {
            var cts = new CancellationTokenSource();
            var cancellationToken = cts.Token;
            var receiverOptions = new ReceiverOptions
            {
                AllowedUpdates = new[]{
                    UpdateType.Message
                }
            };

            _messageHandler = host.Services.GetRequiredService<IBaseMessageHandler>();

            client.StartReceiving(HandleUpdateAsync,
                HandleErrorAsync,
                receiverOptions,
                cancellationToken);

            host.Run();
        }

        public async Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
        {
            // Некоторые действия
            Console.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(exception));
        }

        public async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            await _messageHandler.HandleAsync(botClient, update, cancellationToken);
        }
    }
}
