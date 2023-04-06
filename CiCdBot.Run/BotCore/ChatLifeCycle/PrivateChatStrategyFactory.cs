using Telegram.Bot.Types;

namespace CiCdBot.Run.BotCore.ChatLifeCycle
{
    public class PrivateChatStrategyFactory : IChatStrategyFactory
    {

        public PrivateChatStrategyFactory()
        {
        }

        public IChatStrategy Create(Chat chat, ChatMember botInfo)
        {
            return new PrivateChatStrategy();
        }
    }
}
