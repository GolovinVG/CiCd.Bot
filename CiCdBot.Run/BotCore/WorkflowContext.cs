using System.Collections.Generic;
using System.Threading;
using CiCd.Domain;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace CiCdBot.Run.BotCore
{
    public class WorkflowContext
    {

        public WorkflowContext(ITelegramBotClient client, Message message, DevProject project, CancellationToken cancellationToken)
        {
            Client = client;
            Message = message;
            Project = project;
            CancellationToken = cancellationToken;
        }

        public ITelegramBotClient Client { get; }
        public Message Message { get; }
        public DevProject Project { get; }
        public CancellationToken CancellationToken { get; }

        public IDictionary<string, string> Data = new Dictionary<string, string>();
    }

}
