using System;
using System.Linq;

namespace CiCdBot.Run.BotCore
{
    public class WorkflowConfigBuilder
    {
        private bool _userBind;
        private StagesConfigBuilder _stagesConfigBuilder = new StagesConfigBuilder();

        public WorkflowConfigBuilder()
        {
        }

        public StagesConfigBuilder Workflow => _stagesConfigBuilder;

        internal WorkflowInstance Build(IServiceProvider seviceProvider)
        {
            var stages = _stagesConfigBuilder.Build(seviceProvider);

            return new WorkflowInstance(){
                Stages = stages,
                UserBind = _userBind
            };
        }

        internal WorkflowConfigBuilder UserBind()
        {
            _userBind = true;
            return this;
        }
    }

}
