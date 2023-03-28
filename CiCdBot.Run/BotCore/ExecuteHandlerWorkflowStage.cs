using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace CiCdBot.Run.BotCore;

public class ExecuteHandlerWorkflowStage<THandler> : WorkflowStage where THandler: IWorkflowStepHandler
{
    private IServiceProvider _seviceProvider;

    public ExecuteHandlerWorkflowStage(IServiceProvider seviceProvider)
    {
        this._seviceProvider = seviceProvider;
    }

    public override async Task ActivateAsync(WorkflowContext context){
        var handler = ActivatorUtilities.CreateInstance<THandler>(_seviceProvider);
        await handler.HandleAsync(context);
    }
}
