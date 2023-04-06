using System.Threading.Tasks;

namespace CiCdBot.Run.BotCore.ChatLifeCycle
{
    public interface IBotLiveCycleEventHandler<TBotLiveCycleEvent> where TBotLiveCycleEvent : BotLiveCycleEvent
    {
        Task HandleAsync(TBotLiveCycleEvent botEvent);
    }
}
