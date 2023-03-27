namespace CiCdBot.Run.BotCore
{
    public class StagesActivationConfigBuilder
    {
        private readonly StagesConfigBuilder stagesBuilder;

        public StagesActivationConfigBuilder(StagesConfigBuilder stagesBuilder)
        {
            this.stagesBuilder = stagesBuilder;
        }

        public StagesConfigBuilder Then => stagesBuilder;

    }

}
