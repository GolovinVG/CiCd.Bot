using System.Threading.Tasks;
using Telegram.Bot;

namespace CiCdBot.Run.BotCore
{
    public class IdleHandler : IWorkflowStepHandler
    {
        public async Task HandleAsync(WorkflowContext context)
        {
            await context.Client.SendTextMessageAsync(context.Message.Chat, "Команды:");
            await context.Client.SendTextMessageAsync(context.Message.Chat, "/Activate - начало работы с проектом");
            await context.Client.SendTextMessageAsync(context.Message.Chat, "/Current - статус текущей задачи");
        }
    }

}
