using System.Threading.Tasks;
using Telegram.Bot;

namespace CiCdBot.Run.BotCore
{
    public class ProjectInfoHandler : IWorkflowStepHandler
    {
        public async Task HandleAsync(WorkflowContext context)
        {
            if (context.Chat?.Project == null)
                return;

            await context.Client.SendTextMessageAsync(context.Message.Chat, $"Проект - {context.Chat.Project.Name}: Версия {context.Chat.Project.Version}");
        }
    }

}
