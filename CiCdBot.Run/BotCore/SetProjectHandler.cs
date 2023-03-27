using System.Threading.Tasks;

namespace CiCdBot.Run.BotCore
{
    public class SetProjectHandler : IWorkflowStepHandler
    {
        public async Task HandleAsync(WorkflowContext context)
        {
            var projectName = context.Data["ProjectName"];
            var projectVersion = context.Data["ProjectVersion"];

            context.Project.Name = projectName;
            context.Project.Version = projectVersion;

            await Task.CompletedTask;
        }
    }

}
