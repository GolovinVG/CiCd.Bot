using System.Threading;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace CiCdBot.Run.BotCore.ChatLifeCycle
{
    public abstract class BotLiveCycleEvent
    {
        protected BotLiveCycleEvent(ITelegramBotClient botClient, Update update, ChatMember botInfo, CancellationToken cancellationToken)
        {
            BotClient = botClient;
            Update = update;
            BotInfo = botInfo;
            CancellationToken = cancellationToken;
        }

        public ITelegramBotClient BotClient { get; }
        public Update Update { get; }
        public CancellationToken CancellationToken { get; }
        public ChatMember BotInfo { get; }
    }
}
