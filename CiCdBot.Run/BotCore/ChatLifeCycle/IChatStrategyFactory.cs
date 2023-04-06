using Telegram.Bot.Types;

namespace CiCdBot.Run.BotCore.ChatLifeCycle
{
    public interface IChatStrategyFactory
    {
        IChatStrategy Create(Chat chat, ChatMember botInfo);
    }
}