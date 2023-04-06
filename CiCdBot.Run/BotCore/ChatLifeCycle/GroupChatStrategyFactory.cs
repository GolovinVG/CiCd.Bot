using Telegram.Bot.Types;

namespace CiCdBot.Run.BotCore.ChatLifeCycle
{
    public class GroupChatStrategyFactory : IChatStrategyFactory
    {
        private readonly IBotLiveCycleMediator _mediator;

        public GroupChatStrategyFactory(IBotLiveCycleMediator mediator)
        {
            _mediator = mediator;
        }

        public IChatStrategy Create(Chat chat, ChatMember botInfo)
        {
            return new GroupChatStrategy(_mediator);
        }
    }
}
