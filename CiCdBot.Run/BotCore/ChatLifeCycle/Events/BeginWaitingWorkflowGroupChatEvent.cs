using CiCdBot.Run.BotCore.Workflow;
using System.Threading;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace CiCdBot.Run.BotCore.ChatLifeCycle.Events
{
    internal class BeginWaitingWorkflowGroupChatEvent : BotLiveCycleEvent
    {
        public BeginWaitingWorkflowGroupChatEvent(ITelegramBotClient botClient,
                                              Update message,
                                              ChatMember botInfo,
                                              CancellationToken cancellationToken,
                                              WorkflowContext workflowContext) : base(botClient, message, botInfo, cancellationToken)
        {
            WorkflowContext = workflowContext;
        }

        public WorkflowContext WorkflowContext { get; }
    }
}