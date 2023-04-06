using CiCdBot.Run.BotCore.ChatLifeCycle;
using System.Threading;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace CiCdBot.Run.BotCore.ChatLifeCycle.Events
{
    public class DirectMessageGroupChatEvent : BotLiveCycleEvent
    {
        public DirectMessageGroupChatEvent(ITelegramBotClient botClient, Update update, ChatMember botInfo, CancellationToken cancellationToken) : base(botClient, update, botInfo, cancellationToken) { }

    }
}
