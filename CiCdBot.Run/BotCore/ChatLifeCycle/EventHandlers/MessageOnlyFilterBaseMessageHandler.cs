using CiCdBot.Run.BotCore.ChatLifeCycle;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace CiCdBot.Run.BotCore.ChatLifeCycle.EventHandlers
{
    public class MessageOnlyFilterBaseMessageHandler : IBaseMessageHandler
    {
        private readonly IBotIdentityProvider _botIdentityProvider;
        private readonly IChatStrategyResolver _chatStrategyResolver;

        public MessageOnlyFilterBaseMessageHandler(IBotIdentityProvider botIdentitProvider, IChatStrategyResolver chatStrategyResolver)
        {
            _botIdentityProvider = botIdentitProvider;
            _chatStrategyResolver = chatStrategyResolver;
        }

        public async Task HandleAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            if (update.Type == UpdateType.Message)
            {
                var botInfo = await _botIdentityProvider.GetMeAsync(botClient, update, cancellationToken);

                var strategy = _chatStrategyResolver.Resolve(update.Message.Chat, botInfo);

                if (strategy != null)
                    await strategy.InvokeAsync(botClient, update, botInfo, cancellationToken);
            }
        }
    }
}
