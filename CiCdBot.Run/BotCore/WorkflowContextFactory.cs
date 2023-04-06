using System.Threading;
using CiCdBot.Run.BotCore.Workflow;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace CiCdBot.Run.BotCore
{
    public class WorkflowContextFactory : IWorkflowContextFactory
    {
        public WorkflowContext Create(ITelegramBotClient botClient, Message message, CancellationToken cancellationToken, WorkflowRunningContext parentContext)
        {
            return new WorkflowContext(botClient, message, cancellationToken, parentContext);
        }
    }
}
