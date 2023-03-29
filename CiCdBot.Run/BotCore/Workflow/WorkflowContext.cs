using System.Collections.Generic;
using System.Threading;
using CiCd.Domain;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace CiCdBot.Run.BotCore
{
    public class WorkflowContext
    {

        public WorkflowContext(
            ITelegramBotClient client,
            Message message,
            CancellationToken cancellationToken,
            WorkflowRunningContext runningContext)
        {
            Client = client;
            Message = message;
            CancellationToken = cancellationToken;
            RunningContext = runningContext;
        }

        public ITelegramBotClient Client { get; }
        public Message Message { get; }
        public CancellationToken CancellationToken { get; }

        public WorkflowRunningContext RunningContext { get; }
    }

}
