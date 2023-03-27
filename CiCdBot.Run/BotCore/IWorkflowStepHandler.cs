using System.Threading.Tasks;

namespace CiCdBot.Run.BotCore
{
    public interface IWorkflowStepHandler
    {
        Task HandleAsync(WorkflowContext context);
    }

}
