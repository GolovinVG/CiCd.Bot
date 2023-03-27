using System;

namespace CiCdBot.Run.BotCore;

public class WaitStagesActivationConfigBuilder<THandler> : StagesActivationConfigBuilder
{
    public WaitStagesActivationConfigBuilder(StagesConfigBuilder stagesBuilder) : base(stagesBuilder)
    {
        HandlerType = typeof(THandler);
    }

    public Type HandlerType { get; }
}
