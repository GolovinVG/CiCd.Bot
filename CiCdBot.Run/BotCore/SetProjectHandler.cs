using System.Threading.Tasks;
using CiCd.Domain;

namespace CiCdBot.Run.BotCore
{
    public class SetProjectHandler : IWorkflowStepHandler
    {
        public async Task HandleAsync(WorkflowContext context)
        {
            var projectName = context.RunningContext.Data["ProjectName"];
            var projectVersion = context.RunningContext.Data["ProjectVersion"];

            context.Chat.Project ??= new DevProject();
            context.Chat.Project.Name = projectName;
            context.Chat.Project.Version = projectVersion;

            await Task.CompletedTask;
        }
    }

}
