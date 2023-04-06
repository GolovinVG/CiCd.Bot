using CiCdBot.Run.BotCore.ChatLifeCycle.Events;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace CiCdBot.Run.BotCore.ChatLifeCycle
{
    public class GroupChatStrategy : IChatStrategy
    {
        private readonly IBotLiveCycleMediator _eventBus;

        public GroupChatStrategy(IBotLiveCycleMediator eventBus)
        {
            _eventBus = eventBus;
        }

        public async Task InvokeAsync(ITelegramBotClient botClient, Update update, ChatMember botInfo, CancellationToken cancellationToken)
        {
            var message = update.Message;
            if (message.Type == MessageType.GroupCreated)
            {

                await _eventBus.SendAsync(new AddedToGroupChatEvent(botClient, update, botInfo, cancellationToken));
                return;
            }

            if (message.Type == MessageType.ChatMembersAdded)
            {
                foreach (var member in message.NewChatMembers)
                {
                    if (member.Id == botInfo.User.Id)
                    {
                        await _eventBus.SendAsync(new AddedToGroupChatEvent(botClient, update, botInfo, cancellationToken));
                        return;
                    }
                }

            }

            if (message.Type != MessageType.Text)
                return;

            if (botInfo.User.CanReadAllGroupMessages != true)
            {
                await _eventBus.SendAsync(new DirectMessageGroupChatEvent(botClient, update, botInfo, cancellationToken));
            }
        }
    }
}
