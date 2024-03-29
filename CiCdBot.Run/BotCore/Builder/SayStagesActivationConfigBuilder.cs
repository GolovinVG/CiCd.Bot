using CiCdBot.Run.BotCore.Workflow;
using System;

namespace CiCdBot.Run.BotCore.Builder;

public class SayStagesActivationConfigBuilder : StagesActivationConfigBuilder
{
    public SayStagesActivationConfigBuilder(StagesConfigBuilder stagesBuilder, string caption) : base(stagesBuilder)
    {
        Caption = caption;
    }

    public string Caption { get; }

    internal override WorkflowStage Build(IServiceProvider seviceProvider)
    {
        return new SayWorkflowStage(Caption);
    }
}
