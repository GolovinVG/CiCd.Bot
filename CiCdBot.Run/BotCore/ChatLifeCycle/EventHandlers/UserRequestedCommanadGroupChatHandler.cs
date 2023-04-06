using System;
using System.Threading.Tasks;
using CiCdBot.Run.BotCore.ChatLifeCycle;
using CiCdBot.Run.BotCore.ChatLifeCycle.Events;
using CiCdBot.Run.BotCore.Workflow;

namespace CiCdBot.Run.BotCore.ChatLifeCycle.EventHandlers
{
    public class UserRequestedCommanadGroupChatHandler : IBotLiveCycleEventHandler<RunCommandGroupChatEvent>
    {
        private readonly IBotLiveCycleMediator _mediator;
        private readonly IWorkflowContextFactory _workflowContextFactory;
        private readonly IWorkflowSetupBuilder _workflowBuilder;
        private readonly IServiceProvider _services;

        public UserRequestedCommanadGroupChatHandler(
                                       IBotLiveCycleMediator mediator,
                                       IWorkflowContextFactory workflowContextFactory,
                                       IWorkflowSetupBuilder workflowBuilder,
                                       IServiceProvider services)
        {
            _mediator = mediator;
            _workflowContextFactory = workflowContextFactory;
            _workflowBuilder = workflowBuilder;
            _services = services;
        }

        public async Task HandleAsync(RunCommandGroupChatEvent @event)
        {
            var workFlowInstance = _workflowBuilder.Build(@event.Command, _services);

            if (workFlowInstance == null)
                return;

            var context = _workflowContextFactory.Create(@event.BotClient, @event.Update.Message, @event.CancellationToken, new WorkflowRunningContext
            {
                Id = Guid.NewGuid(),
                WorkflowInstance = workFlowInstance
            });

            var completed = await context.RunningContext.WorkflowInstance.RunAsync(context);

            if (completed)
                await _mediator.SendAsync(new CompleteWorkflowGroupChatEvent(@event.BotClient, @event.Update, @event.BotInfo, @event.CancellationToken, context));
            else
                await _mediator.SendAsync(new BeginWaitingWorkflowGroupChatEvent(@event.BotClient, @event.Update, @event.BotInfo, @event.CancellationToken, context));
        }
    }
}
