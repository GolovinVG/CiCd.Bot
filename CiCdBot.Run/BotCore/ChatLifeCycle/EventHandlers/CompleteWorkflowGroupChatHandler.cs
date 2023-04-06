using CiCdBot.Run.BotCore.ChatLifeCycle;
using CiCdBot.Run.BotCore.ChatLifeCycle.Events;
using System.Threading.Tasks;

namespace CiCdBot.Run.BotCore.ChatLifeCycle.EventHandlers
{
    public class CompleteWorkflowGroupChatHandler : IBotLiveCycleEventHandler<CompleteWorkflowGroupChatEvent>
    {
        private readonly IWorkflowStorage _workflowStorage;

        public CompleteWorkflowGroupChatHandler(IWorkflowStorage workflowStorage)
        {
            _workflowStorage = workflowStorage;
        }

        Task IBotLiveCycleEventHandler<CompleteWorkflowGroupChatEvent>.HandleAsync(CompleteWorkflowGroupChatEvent botEvent)
        {
            _workflowStorage.RemoveActiveRunningContext(botEvent.Update.Message, botEvent.WorkflowContext.RunningContext);
            return Task.CompletedTask;
        }
    }
}
