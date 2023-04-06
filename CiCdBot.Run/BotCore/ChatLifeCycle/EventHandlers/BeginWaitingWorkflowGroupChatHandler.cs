using CiCdBot.Run.BotCore.ChatLifeCycle;
using CiCdBot.Run.BotCore.ChatLifeCycle.Events;
using System;
using System.Threading.Tasks;

namespace CiCdBot.Run.BotCore.ChatLifeCycle.EventHandlers
{
    public class BeginWaitingWorkflowGroupChatHandler : IBotLiveCycleEventHandler<BeginWaitingWorkflowGroupChatEvent>
    {
        private readonly IWorkflowStorage _workflowStorage;

        public BeginWaitingWorkflowGroupChatHandler(IWorkflowStorage workflowStorage)
        {
            _workflowStorage = workflowStorage;
        }

        Task IBotLiveCycleEventHandler<BeginWaitingWorkflowGroupChatEvent>.HandleAsync(BeginWaitingWorkflowGroupChatEvent botEvent)
        {
            _workflowStorage.SaveRunningContext(botEvent.Update.Message, botEvent.WorkflowContext.RunningContext);
            return Task.CompletedTask;
        }
    }
}
