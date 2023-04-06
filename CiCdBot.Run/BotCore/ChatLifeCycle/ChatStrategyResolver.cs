using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace CiCdBot.Run.BotCore.ChatLifeCycle
{
    public class ChatStrategyResolver : IChatStrategyResolver
    {
        private readonly IChatStrategyFactory _groupChatStrategyFactory;
        private readonly IChatStrategyFactory _privateChatStrategyFactory;

        public ChatStrategyResolver(IChatStrategyFactory groupChatStrategyFactory, IChatStrategyFactory privateChatStrategyFactory)
        {
            _groupChatStrategyFactory = groupChatStrategyFactory;
            _privateChatStrategyFactory = privateChatStrategyFactory;
        }



        public IChatStrategy Resolve(Chat chat, ChatMember botInfo)
        {
            if (chat.Type == ChatType.Group)
                return _groupChatStrategyFactory.Create(chat, botInfo);

            if (chat.Type == ChatType.Private)
                return _privateChatStrategyFactory.Create(chat, botInfo);

            return null;
        }
    }
}
