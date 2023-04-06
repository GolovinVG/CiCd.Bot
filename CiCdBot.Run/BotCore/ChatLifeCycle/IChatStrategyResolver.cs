using Telegram.Bot.Types;

namespace CiCdBot.Run.BotCore.ChatLifeCycle
{
    public interface IChatStrategyResolver
    {
        IChatStrategy Resolve(Chat chat, ChatMember botInfo);
    }
}