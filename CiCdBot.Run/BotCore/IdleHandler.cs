using System.Threading.Tasks;
using Telegram.Bot;

namespace CiCdBot.Run.BotCore
{
    public class IdleHandler : IWorkflowStepHandler
    {
        public async Task HandleAsync(WorkflowContext context)
        {
            var message = 
            @"Команды:
/Activate - начало работы с проектом
/Current - статус текущей задачи";


            if (context.Chat.Project != null){
                message += "\n\r/ProjectInfo - Данные текущего проекта";
            }
            await context.Client.SendTextMessageAsync(context.Message.Chat, message);
        }
    }

}
