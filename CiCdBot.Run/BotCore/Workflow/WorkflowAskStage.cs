using System.Threading.Tasks;
using Telegram.Bot;

namespace CiCdBot.Run.BotCore
{
    public class WorkflowAskStage : WorkflowStage
    {
        private string question;
        private string parameter;

        public WorkflowAskStage(string question, string parameter): base(WorkflowStageTypes.UserWait)
        {
            this.question = question;
            this.parameter = parameter;
        }

        public override async Task ActivateAsync(WorkflowContext context){
            await context.Client.SendTextMessageAsync(context.Message.Chat, question);
        }

        public override Task ContinueAsync(WorkflowContext context, string[] data){
            if (data.Length > 0)
                context.RunningContext.Data[parameter] = data[0];
            return Task.CompletedTask;
        }
    }
}
