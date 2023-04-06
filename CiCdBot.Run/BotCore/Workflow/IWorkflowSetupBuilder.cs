using System;

namespace CiCdBot.Run.BotCore.Workflow
{
    public interface IWorkflowSetupBuilder
    {
        WorkflowInstance Build(string name, IServiceProvider seviceProvider);
    }
}