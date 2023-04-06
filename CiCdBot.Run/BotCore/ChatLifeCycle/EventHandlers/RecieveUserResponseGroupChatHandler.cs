using CiCdBot.Run.BotCore.ChatLifeCycle.Events;
using System.Threading.Tasks;

namespace CiCdBot.Run.BotCore.ChatLifeCycle.EventHandlers
{
    public class RecieveUserResponseGroupChatHandler : IBotLiveCycleEventHandler<RecieveUserResponseGroupChatEvent>
    {
        private readonly IWorkflowStorage _workflowStorage;
        private readonly IBotLiveCycleMediator _mediator;
        private readonly IWorkflowContextFactory _workflowContextFactory;

        public RecieveUserResponseGroupChatHandler(
                                       IWorkflowStorage workflowStorage,
                                       IBotLiveCycleMediator mediator,
                                       IWorkflowContextFactory workflowContextFactory)
        {
            _workflowStorage = workflowStorage;
            _mediator = mediator;
            _workflowContextFactory = workflowContextFactory;
        }

        public async Task HandleAsync(RecieveUserResponseGroupChatEvent @event)
        {
            var context = _workflowContextFactory.Create(@event.BotClient, @event.Update.Message, @event.CancellationToken, @event.Context);

            var completed = await context.RunningContext.WorkflowInstance.ContinueAsync(context, new[] { @event.Update.Message.Text });

            if (completed)
                await _mediator.SendAsync(new CompleteWorkflowGroupChatEvent(@event.BotClient, @event.Update, @event.BotInfo, @event.CancellationToken, context));
            else
                await _mediator.SendAsync(new BeginWaitingWorkflowGroupChatEvent(@event.BotClient, @event.Update, @event.BotInfo, @event.CancellationToken, context));

        }
    }
}
