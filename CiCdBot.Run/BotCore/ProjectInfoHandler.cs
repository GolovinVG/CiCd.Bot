using System.Threading.Tasks;
using Telegram.Bot;

namespace CiCdBot.Run.BotCore
{
    public class ProjectInfoHandler : IWorkflowStepHandler
    {
        public async Task HandleAsync(WorkflowContext context)
        {
            await context.Client.SendTextMessageAsync(context.Message.Chat, $"Проект - {context.Project.Name}: Версия {context.Project.Version}");
        }
    }

}
