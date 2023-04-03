using CiCdBot.Run.BotCore.Workflow;
using System;

namespace CiCdBot.Run.BotCore.Builder;

public class WaitStagesActivationConfigBuilder<THandler> : StagesActivationConfigBuilder where THandler : IWorkflowStepHandler
{
    public WaitStagesActivationConfigBuilder(StagesConfigBuilder stagesBuilder) : base(stagesBuilder)
    {
        HandlerType = typeof(THandler);
    }

    public Type HandlerType { get; }

    internal override WorkflowStage Build(IServiceProvider seviceProvider)
    {
        return new ExecuteHandlerWorkflowStage<THandler>(seviceProvider);
    }
}
