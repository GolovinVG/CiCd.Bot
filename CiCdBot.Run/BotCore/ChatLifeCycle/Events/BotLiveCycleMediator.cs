using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace CiCdBot.Run.BotCore.ChatLifeCycle.Events
{
    public class BotLiveCycleMediator : IBotLiveCycleMediator
    {
        public readonly IServiceProvider serviceProvider;

        public BotLiveCycleMediator(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }

        public async Task SendAsync<TBotLiveCycleEvent>(TBotLiveCycleEvent botEvent) where TBotLiveCycleEvent : BotLiveCycleEvent
        {
            var handler = serviceProvider.GetRequiredService<IBotLiveCycleEventHandler<TBotLiveCycleEvent>>();

            if (handler != null)
            {
                await handler.HandleAsync(botEvent);
            }
        }
    }
}
