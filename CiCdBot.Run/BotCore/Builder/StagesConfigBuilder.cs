using CiCdBot.Run.BotCore.Workflow;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CiCdBot.Run.BotCore.Builder
{
    public class StagesConfigBuilder
    {
        private readonly IList<StagesActivationConfigBuilder> _stageActivationBuilders = new List<StagesActivationConfigBuilder>();

        public StagesActivationConfigBuilder Ask(string question, string parameter)
        {
            var stagesActivationConfigBuilder = new AskStagesActivationConfigBuilder(this, question, parameter);
            _stageActivationBuilders.Add(stagesActivationConfigBuilder);

            return stagesActivationConfigBuilder;
        }

        public StagesActivationConfigBuilder Say(string caption)
        {
            var stagesActivationConfigBuilder = new SayStagesActivationConfigBuilder(this, caption);
            _stageActivationBuilders.Add(stagesActivationConfigBuilder);
            return stagesActivationConfigBuilder;
        }

        public StagesActivationConfigBuilder Wait<T>() where T : IWorkflowStepHandler
        {

            var stagesActivationConfigBuilder = new WaitStagesActivationConfigBuilder<T>(this);
            _stageActivationBuilders.Add(stagesActivationConfigBuilder);
            return stagesActivationConfigBuilder;
        }

        internal WorkflowStage[] Build(IServiceProvider seviceProvider)
        {
            return _stageActivationBuilders.Select(x => x.Build(seviceProvider)).ToArray();
        }
    }

}
