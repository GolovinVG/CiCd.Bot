using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace CiCdBot.Run.BotCore.ChatLifeCycle
{
    public interface IBaseMessageHandler
    {
        public Task HandleAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken);
    }
}
