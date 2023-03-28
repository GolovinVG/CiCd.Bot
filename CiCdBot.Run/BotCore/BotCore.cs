using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CiCd.Domain;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace CiCdBot.Run.BotCore
{
    public class BotEngine
    {

        //TODO --> Storage
        public static IList<ProjectChat> Chats = new List<ProjectChat>();
        private WorkflowSetupBuilder _builder = new WorkflowSetupBuilder();

        private IDictionary<long, ICollection<WorkflowRunningContext>> _runningContexts = new Dictionary<long, ICollection<WorkflowRunningContext>>();
        private IHost host;

        public BotEngine(IHost host)
        {
            this.host = host;
        }

        internal void Run(TelegramBotClient client)
        {
            var cts = new CancellationTokenSource();
            var cancellationToken = cts.Token;
            var receiverOptions = new ReceiverOptions
            {
                AllowedUpdates = new []{
                    UpdateType.Message
                }
            };

            client.StartReceiving(HandleUpdateAsync,
                HandleErrorAsync,
                receiverOptions,
                cancellationToken);
            
            host.Run();
        }

        internal void SetupWorkflow(Action<WorkflowSetupBuilder> act)
        {
            act(_builder);
        }

        public async Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
        {
            // Некоторые действия
            Console.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(exception));
        }

        public async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            using var scope = host.Services.CreateScope();
            var serviceProvider = scope.ServiceProvider;

            if (update.Type == UpdateType.Message)
            {
                var message = update.Message;

                var activeChat = Chats.FirstOrDefault(x => x.Id == message.Chat.Id);
                if (activeChat == null)
                {
                    activeChat = new ProjectChat
                    {
                        Id = message.Chat.Id
                    };
                    Chats.Add(activeChat);
                }

                if (message.Type == MessageType.GroupCreated)
                {
                    var idleWorkflow = _builder.Build("Idle", serviceProvider);
                    var context = new WorkflowContext(botClient, message, activeChat, cancellationToken, new WorkflowRunningContext
                    {
                        WorkflowInstance = idleWorkflow
                    });
                    await idleWorkflow.RunAsync(context);

                    return;
                }

                if (message.Type == MessageType.Text)
                {
                    if (message.Chat.Type != ChatType.Group) return;

                    var me = await botClient.GetMeAsync(cancellationToken);
                    var meInChat = await botClient.GetChatMemberAsync(update.Message.Chat.Id, me.Id, cancellationToken);

                    var botCommands = new List<string>();

                    for (int i = 0; i < message.Entities?.Length; i++)
                    {
                        if (message.Entities[i].Type == MessageEntityType.BotCommand)
                        {
                            var commandParts = message.Text.Substring(message.Entities[i].Offset, message.Entities[i].Length).Split("@");

                            if (commandParts.Length == 2 && commandParts[1].Equals(meInChat.User.Username))
                                botCommands.Add(commandParts[0]);
                        }
                    }

                    if (botCommands?.Any() == false)
                    {
                        if (_runningContexts.ContainsKey(message.From.Id) == false)
                            return;

                        var currentContexts = _runningContexts[message.From.Id];
                        var contextToContinue = currentContexts.FirstOrDefault(x => x.WorkflowInstance.CurrentStage.StageType == WorkflowStageTypes.UserWait);

                        if (contextToContinue == null) return;
                        var context = new WorkflowContext(botClient, message, activeChat, cancellationToken, contextToContinue);

                        var completed = await contextToContinue.WorkflowInstance.ContinueAsync(context, new[] { message.Text });

                        if (completed)
                        {
                            _runningContexts[message.From.Id].Remove(contextToContinue);


                            var idleWorkflow = _builder.Build("Idle", serviceProvider);
                            var idleContext = new WorkflowContext(botClient, message, activeChat, cancellationToken, new WorkflowRunningContext
                            {
                                WorkflowInstance = idleWorkflow
                            });
                            await idleWorkflow.RunAsync(context);
                        }
                    }

                    foreach (var item in botCommands)
                    {
                        WorkflowRunningContext? runningContext = null;

                        if (_runningContexts.ContainsKey(message.From.Id) == false || _runningContexts[message.From.Id].Any() == false)
                        {
                            runningContext = new WorkflowRunningContext();

                            var workflow = _builder.Build(item.TrimStart('/'), serviceProvider);

                            runningContext.WorkflowInstance = workflow;
                            runningContext.CreatedDate = DateTime.Now;

                            var context = new WorkflowContext(botClient, message, activeChat, cancellationToken, runningContext);

                            var completed = await workflow.RunAsync(context);

                            if (completed == false)
                                if (_runningContexts.TryAdd(message.From.Id, new List<WorkflowRunningContext> { runningContext }) == false)
                                    _runningContexts[message.From.Id].Add(runningContext);

                            return;
                        }

                        runningContext = _runningContexts[message.From.Id].FirstOrDefault();

                        // wait for command
                    }

                    return;
                }
            }

        }
    }
}
