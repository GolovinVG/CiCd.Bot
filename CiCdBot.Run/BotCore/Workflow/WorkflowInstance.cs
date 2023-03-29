using System.Threading.Tasks;

namespace CiCdBot.Run.BotCore
{
    public class WorkflowInstance
    {
        internal bool UserBind;

        public WorkflowStage[] Stages { get; internal set; }

        private int currentStageIndex = 0;

        public WorkflowStage CurrentStage => Stages[currentStageIndex];

        internal async Task<bool> RunAsync(WorkflowContext context)
        {
            for (; currentStageIndex < Stages.Length; currentStageIndex++)
            {
                await CurrentStage.ActivateAsync(context);

                if (CurrentStage.StageType != WorkflowStageTypes.Continue)
                    return false;
            }

            return true;
        }

        internal async Task<bool> ContinueAsync(WorkflowContext context, string[] data)
        {
            await CurrentStage.ContinueAsync(context, data);

            currentStageIndex++;

            return await RunAsync(context);
        }
    }
}
