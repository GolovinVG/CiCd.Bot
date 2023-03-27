using System;
using System.Threading.Tasks;

namespace CiCdBot.Run.BotCore;

public class ExecuteHandlerWorkflowStage<THandler> : WorkflowStage where THandler: IWorkflowStepHandler
{
    public override async Task ActivateAsync(WorkflowContext context){
        var handler = Activator.CreateInstance<THandler>();
        await handler.HandleAsync(context);
    }
}
