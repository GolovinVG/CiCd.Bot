using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace CiCdBot.Run.BotCore.ChatLifeCycle
{
    public class PrivateChatStrategy : IChatStrategy
    {
        public Task InvokeAsync(ITelegramBotClient botClient, Update update, ChatMember botInfo, CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
