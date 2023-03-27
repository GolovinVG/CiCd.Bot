using System;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;

namespace CiCdBot.Run.BotCore
{
    public class BotEngine
    {
        private WorkflowSetupBuilder _builder = new WorkflowSetupBuilder();

        internal void Run(TelegramBotClient client)
        {
            var cts = new CancellationTokenSource();
            var cancellationToken = cts.Token;
            var receiverOptions = new ReceiverOptions
            {
                AllowedUpdates = {

                }, // receive all update types
            };


            client.StartReceiving(HandleUpdateAsync,
                HandleErrorAsync,
                receiverOptions,
                cancellationToken);
        }

        internal void SetupWorkflow(Action<WorkflowSetupBuilder> act)
        {
            act(_builder);
        }

        
        public static async Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
        {
            // Некоторые действия
            Console.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(exception));
        }

        
        public static async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {

            
        }
    }

}
