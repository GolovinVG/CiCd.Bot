using System.Collections.Generic;
using System;

namespace CiCdBot.Run.BotCore.Workflow
{
    public class WorkflowSetupBuilder : IWorkflowSetupBuilder
    {
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
            return Setup("Idle");
        }


        public WorkflowInstance Build(string name, IServiceProvider seviceProvider)
        {
            if (_worflows.ContainsKey(name) == false)
                return null;

            return _worflows[name].Build(seviceProvider);
        }

        internal static WorkflowSetupBuilder SetupWorkflow(Action<WorkflowSetupBuilder> act)
        {
            var builder = new WorkflowSetupBuilder();
            act(builder);

            return builder;
        }
    }
}
