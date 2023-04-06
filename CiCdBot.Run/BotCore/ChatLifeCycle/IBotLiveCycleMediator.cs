using System.Threading.Tasks;

namespace CiCdBot.Run.BotCore.ChatLifeCycle
{
    public interface IBotLiveCycleMediator
    {
        Task SendAsync<TBotLiveCycleEvent>(TBotLiveCycleEvent botEvent) where TBotLiveCycleEvent : BotLiveCycleEvent;
    }
}
