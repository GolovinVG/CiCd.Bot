using System.Threading.Tasks;
using Telegram.Bot;

namespace CiCdBot.Run.BotCore.Workflow
{
    internal class SayWorkflowStage : WorkflowStage
    {
        private string caption;

        public SayWorkflowStage(string caption)
        {
            this.caption = caption;
        }

        public override async Task ActivateAsync(WorkflowContext context)
        {
            await context.Client.SendTextMessageAsync(context.Message.Chat, caption);
        }
    }
}
