using System.Collections.Generic;

namespace CiCdBot.Run.BotCore
{
    public class StagesConfigBuilder
    {
        private readonly IList<StagesActivationConfigBuilder> _stageActivationBuilders = new List<StagesActivationConfigBuilder>();

        public StagesActivationConfigBuilder Ask(string question, string parameter)
        {
            var stagesActivationConfigBuilder = new AskStagesActivationConfigBuilder(this, question, parameter);

            return stagesActivationConfigBuilder;
        }

        public StagesActivationConfigBuilder Say(string caption)
        {
            var stagesActivationConfigBuilder = new SayStagesActivationConfigBuilder(this, caption);

            return stagesActivationConfigBuilder;
        }

        public StagesActivationConfigBuilder Wait<T>()
        {

            var stagesActivationConfigBuilder = new WaitStagesActivationConfigBuilder<T>(this);

            return stagesActivationConfigBuilder;
        }
    }

}
