using CiCdBot.Run.BotCore.ChatLifeCycle;
using CiCdBot.Run.BotCore.Workflow;
using System.Threading;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace CiCdBot.Run.BotCore.ChatLifeCycle.Events
{
    public class RecieveUserResponseGroupChatEvent : BotLiveCycleEvent
    {

        public RecieveUserResponseGroupChatEvent(WorkflowRunningContext context, ITelegramBotClient botClient, Update update, ChatMember botInfo, CancellationToken cancellationToken) : base(botClient, update, botInfo, cancellationToken)
        {
            Context = context;
        }

        public WorkflowRunningContext Context { get; }
    }
}