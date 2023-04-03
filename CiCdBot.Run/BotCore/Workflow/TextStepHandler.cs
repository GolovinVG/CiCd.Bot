using System.Threading.Tasks;
using Telegram.Bot;

namespace CiCdBot.Run.BotCore.Workflow
{
    public class TextStepHandler : IWorkflowStepHandler
    {
        private string _text;

        public TextStepHandler(string text)
        {
            _text = text;

        }
        public async Task HandleAsync(WorkflowContext context)
        {
            await context.Client.SendTextMessageAsync(context.Message.Chat, _text);
        }
    }

}
