using System.Threading.Tasks;

namespace CiCdBot.Run.BotCore.Workflow
{
    public interface IWorkflowStepHandler
    {
        Task HandleAsync(WorkflowContext context);
    }

}
