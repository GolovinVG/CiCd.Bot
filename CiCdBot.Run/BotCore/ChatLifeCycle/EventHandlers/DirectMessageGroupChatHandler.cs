using CiCdBot.Run.BotCore.ChatLifeCycle.Events;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot.Types.Enums;

namespace CiCdBot.Run.BotCore.ChatLifeCycle.EventHandlers
{
    public class DirectMessageGroupChatHandler : IBotLiveCycleEventHandler<DirectMessageGroupChatEvent>
    {
        private readonly IWorkflowStorage _workflowStorage;
        private readonly IBotLiveCycleMediator _mediator;

        public DirectMessageGroupChatHandler(
                                       IWorkflowStorage workflowStorage,
                                       IBotLiveCycleMediator mediator)
        {
            _workflowStorage = workflowStorage;
            _mediator = mediator;
        }

        public async Task HandleAsync(DirectMessageGroupChatEvent @event)
        {
            var message = @event.Update.Message;
            var botCommands = new List<string>();

            if (message.Entities?.Any() ?? false)
                foreach (var messageEntity in message?.Entities.Where(x => x.Type == MessageEntityType.BotCommand))
                {
                    var commandParts = message.Text.Substring(messageEntity.Offset, messageEntity.Length).Split("@");

                    if (commandParts.Length == 2 && commandParts[1].Equals(@event.BotInfo.User.Username))
                        botCommands.Add(commandParts[0].TrimStart('/'));
                }

            var onGoing = _workflowStorage.GetActiveContext(message);

            if (botCommands?.Any() == false)
            {
                if (onGoing.Any())
                {
                    //todo to stack
                    var context = onGoing.First();

                    await _mediator.SendAsync(new RecieveUserResponseGroupChatEvent(context, @event.BotClient, @event.Update, @event.BotInfo, @event.CancellationToken));
                }

                return;
            }

            var command = botCommands.First();

            if (onGoing.Any() == false)
            {
                await _mediator.SendAsync(new RunCommandGroupChatEvent(command, @event.BotClient, @event.Update, @event.BotInfo, @event.CancellationToken));
                return;
            }

        }
    }
}
