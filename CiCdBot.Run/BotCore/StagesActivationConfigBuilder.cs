using System;

namespace CiCdBot.Run.BotCore
{
    public abstract class StagesActivationConfigBuilder
    {
        private readonly StagesConfigBuilder stagesBuilder;

        public StagesActivationConfigBuilder(StagesConfigBuilder stagesBuilder)
        {
            this.stagesBuilder = stagesBuilder;
        }

        public StagesConfigBuilder Then => stagesBuilder;

        internal abstract WorkflowStage Build();
        
    }

}
