using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace CiCdBot.Run.BotCore.Workflow;

public class ExecuteHandlerWorkflowStage<THandler> : WorkflowStage where THandler : IWorkflowStepHandler
{
    private IServiceProvider _seviceProvider;

    public ExecuteHandlerWorkflowStage(IServiceProvider seviceProvider)
    {
        _seviceProvider = seviceProvider;
    }

    public override async Task ActivateAsync(WorkflowContext context)
    {

        using var scope = _seviceProvider.CreateScope();
        var serviceProvider = scope.ServiceProvider;
        var handler = ActivatorUtilities.CreateInstance<THandler>(serviceProvider);
        await handler.HandleAsync(context);
    }
}
