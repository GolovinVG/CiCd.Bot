using CiCdBot.Run.BotCore.ChatLifeCycle;
using System.Threading;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace CiCdBot.Run.BotCore.ChatLifeCycle.Events
{
    public class RunCommandGroupChatEvent : BotLiveCycleEvent
    {
        public RunCommandGroupChatEvent(string command, ITelegramBotClient botClient, Update update, ChatMember botInfo, CancellationToken cancellationToken) : base(botClient, update, botInfo, cancellationToken)
        {
            Command = command;
        }

        public string Command { get; }
    }
}