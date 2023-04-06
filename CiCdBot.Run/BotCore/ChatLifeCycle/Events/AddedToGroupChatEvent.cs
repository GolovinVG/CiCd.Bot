using System.Threading;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace CiCdBot.Run.BotCore.ChatLifeCycle.Events
{
    public class AddedToGroupChatEvent : BotLiveCycleEvent
    {
        public AddedToGroupChatEvent(ITelegramBotClient botClient, Update update, ChatMember botInfo, CancellationToken cancellationToken) : base(botClient, update, botInfo, cancellationToken) { }

    }
}
