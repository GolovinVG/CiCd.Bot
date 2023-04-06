using System;
using System.Linq;
using System.Threading.Tasks;
using CiCdBot.Run.BotCore.ChatLifeCycle;
using CiCdBot.Run.BotCore.ChatLifeCycle.Events;
using CiCdBot.Run.BotCore.Workflow;

namespace CiCdBot.Run.BotCore.ChatLifeCycle.EventHandlers
{
    public class AddedToGroupChatHandler : IBotLiveCycleEventHandler<AddedToGroupChatEvent>
    {
        private readonly IWorkflowContextFactory _workflowContextFactory;
        private readonly IServiceProvider _serviceProvider;
        private readonly IWorkflowSetupBuilder _builder;
        private readonly IWorkflowStorage _workflowStorage;

        public AddedToGroupChatHandler(IWorkflowSetupBuilder workflowSetupBuilder,
                                       IServiceProvider serviceProvider,
                                       IWorkflowContextFactory workflowContextFactory,
                                       IWorkflowStorage workflowStorage)
        {
            _builder = workflowSetupBuilder;
            _serviceProvider = serviceProvider;
            _workflowContextFactory = workflowContextFactory;
            _workflowStorage = workflowStorage;
        }

        public async Task HandleAsync(AddedToGroupChatEvent @event)
        {
            var message = @event.Update.Message;

            var current = _workflowStorage.GetActiveContext(message);

            if (current.Any())
                return;

            var idleWorkflow = _builder.Build("Idle", _serviceProvider);
            var context = _workflowContextFactory.Create(@event.BotClient, message, @event.CancellationToken, new WorkflowRunningContext
            {
                Id = Guid.NewGuid(),
                WorkflowInstance = idleWorkflow
            });
            await idleWorkflow.RunAsync(context);
        }
    }
}
