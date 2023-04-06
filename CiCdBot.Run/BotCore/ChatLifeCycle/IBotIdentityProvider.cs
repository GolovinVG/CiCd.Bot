using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace CiCdBot.Run.BotCore.ChatLifeCycle
{
    public interface IBotIdentityProvider
    {
        Task<ChatMember> GetMeAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken);
    }
}