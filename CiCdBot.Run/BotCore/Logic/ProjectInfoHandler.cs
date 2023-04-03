using CiCdBot.Run.BotCore.Workflow;
using System.Threading.Tasks;
using Telegram.Bot;
using static CiCdBot.Run.Program;

namespace CiCdBot.Run.BotCore.Logic
{
    public class ProjectInfoHandler : IWorkflowStepHandler
    {
        private readonly IProjectStorage _projectStorage;

        public ProjectInfoHandler(IProjectStorage projectStorage)
        {
            _projectStorage = projectStorage;
        }

        public async Task HandleAsync(WorkflowContext context)
        {
            var activeChat = _projectStorage.GetProjectChat(context.Message.Chat.Id);

            if (activeChat?.Project == null)
                return;

            await context.Client.SendTextMessageAsync(context.Message.Chat, $"Проект - {activeChat.Project.Name}: Версия {activeChat.Project.Version}");
        }
    }

}
