using System.Collections.Generic;

namespace CiCdBot.Run.BotCore
{
    public class WorkflowSetupBuilder
    {
        private WorkflowConfigBuilder _idle;
        private IDictionary<string, WorkflowConfigBuilder> _worflows = new Dictionary<string, WorkflowConfigBuilder>();

        internal WorkflowConfigBuilder Setup(string name)
        {
            var builder = new WorkflowConfigBuilder();

            if (_worflows.TryAdd(name, builder) == false)
            {
                builder = _worflows[name];
            }

            return builder;
        }

        internal WorkflowConfigBuilder SetupIdle()
        {
            var builder = new WorkflowConfigBuilder();

            _idle = builder;

            return builder;
        }
    }

}
