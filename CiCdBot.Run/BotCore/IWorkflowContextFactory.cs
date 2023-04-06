using CiCdBot.Run.BotCore.Workflow;
using System.Threading;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace CiCdBot.Run.BotCore
{
    public interface IWorkflowContextFactory
    {
        WorkflowContext Create(ITelegramBotClient botClient, Message message, CancellationToken cancellationToken, WorkflowRunningContext workflowInstance);
    }
}