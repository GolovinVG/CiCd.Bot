using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace CiCdBot.Run.BotCore.ChatLifeCycle
{
    public interface IChatStrategy
    {
        Task InvokeAsync(ITelegramBotClient botClient, Update update, ChatMember botInfo, CancellationToken cancellationToken);
    }
}
