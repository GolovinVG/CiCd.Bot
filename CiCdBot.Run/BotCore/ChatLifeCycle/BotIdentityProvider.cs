using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace CiCdBot.Run.BotCore.ChatLifeCycle
{
    public class BotIdentityProvider : IBotIdentityProvider
    {
        public async Task<ChatMember> GetMeAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            var me = await botClient.GetMeAsync(cancellationToken);

            var meInChat = await botClient.GetChatMemberAsync(update.Message.Chat.Id, me.Id, cancellationToken);

            return meInChat;
        }
    }
}
