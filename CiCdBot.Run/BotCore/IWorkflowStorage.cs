using CiCdBot.Run.BotCore.Workflow;
using Telegram.Bot.Types;

namespace CiCdBot.Run.BotCore
{
    public interface IWorkflowStorage
    {
        WorkflowRunningContext[] GetActiveContext(Message message);
        void RemoveActiveRunningContext(Message message, WorkflowRunningContext runningContext);
        void SaveRunningContext(Message message, WorkflowRunningContext runningContext);
    }
}