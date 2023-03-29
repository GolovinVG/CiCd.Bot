using System.Threading.Tasks;
using CiCd.Domain;
using static CiCdBot.Run.Program;

namespace CiCdBot.Run.BotCore
{
    public class SetProjectHandler : IWorkflowStepHandler
    {
        private readonly IProjectStorage _projectStorage;

        public SetProjectHandler(IProjectStorage projectStorage)
        {
            _projectStorage = projectStorage;
        }

        public async Task HandleAsync(WorkflowContext context)
        {
            var activeChat = _projectStorage.GetProjectChat(context.Message.Chat.Id);
            if (activeChat == null)
                {
                    activeChat = new ProjectChat
                    {
                        Id = context.Message.Chat.Id
                    };
                }
            var projectName = context.RunningContext.Data["ProjectName"];
            var projectVersion = context.RunningContext.Data["ProjectVersion"];

            activeChat.Project ??= new DevProject();
            activeChat.Project.Name = projectName;
            activeChat.Project.Version = projectVersion;

            _projectStorage.SetProjectChat(activeChat);
            await _projectStorage.SaveAsync();

            await Task.CompletedTask;
        }
    }

}
