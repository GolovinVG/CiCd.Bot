using System.Threading.Tasks;

namespace CiCdBot.Run.BotCore.Workflow
{
    public abstract class WorkflowStage
    {
        public WorkflowStage(WorkflowStageTypes stageType = WorkflowStageTypes.Continue)
        {
            StageType = stageType;
        }

        public virtual Task ActivateAsync(WorkflowContext context)
        {
            return Task.CompletedTask;
        }

        public virtual Task ContinueAsync(WorkflowContext context, string[] data)
        {
            return Task.CompletedTask;
        }

        public WorkflowStageTypes StageType { get; }

    }
}
