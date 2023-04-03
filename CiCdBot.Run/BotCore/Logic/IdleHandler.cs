using System.Threading.Tasks;
using Telegram.Bot;
using static CiCdBot.Run.Program;

namespace CiCdBot.Run.BotCore.Logic
{
    public class IdleHandler : IWorkflowStepHandler
    {
        
        private readonly IProjectStorage _projectStorage;

        public IdleHandler(IProjectStorage projectStorage)
        {
            _projectStorage = projectStorage;
        }

        public async Task HandleAsync(WorkflowContext context)
        {
            var activeChat = _projectStorage.GetProjectChat(context.Message.Chat.Id);
            //TODO Достать вокфлоу, которые готовы запуститься в текущих условиях
            var message = 
@"Команды:
/Activate - начало работы с проектом
/Current - статус текущей задачи";


            if (activeChat.Project != null){
                message += "\n\r/ProjectInfo - Данные текущего проекта";
            }
            await context.Client.SendTextMessageAsync(context.Message.Chat, message);
        }
    }

}
