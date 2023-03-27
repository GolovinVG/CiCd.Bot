using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CiCd.Domain;
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

        internal void Run(TelegramBotClient client)
        {
            var cts = new CancellationTokenSource();
            var cancellationToken = cts.Token;
            var receiverOptions = new ReceiverOptions
            {
                AllowedUpdates = {

                }, // receive all update types
            };

            client.StartReceiving(HandleUpdateAsync,
                HandleErrorAsync,
                receiverOptions,
                cancellationToken);
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
                    var idleWorkflow = _builder.Build("Idle");
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


                            var idleWorkflow = _builder.Build("Idle");
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

                            var workflow = _builder.Build(item.TrimStart('/'));

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

    public class WorkflowRunningContext
    {
        public User User { get; set; }
        public IDictionary<string, string> Data { get; } = new Dictionary<string, string>();

        public DateTime CreatedDate { get; set; }

        public DateTime LastCall { get; set; }

        public WorkflowInstance WorkflowInstance { get; set; }
    }

    public class WorkflowInstance
    {
        internal bool UserBind;

        public WorkflowStage[] Stages { get; internal set; }

        private int currentStageIndex = 0;

        public WorkflowStage CurrentStage => Stages[currentStageIndex];

        internal async Task<bool> RunAsync(WorkflowContext context)
        {
            for (; currentStageIndex < Stages.Length; currentStageIndex++)
            {
                await CurrentStage.ActivateAsync(context);

                if (CurrentStage.StageType != WorkflowStageTypes.Continue)
                    return false;
            }

            return true;
        }

        internal async Task<bool> ContinueAsync(WorkflowContext context, string[] data)
        {
            await CurrentStage.ContinueAsync(context, data);

            currentStageIndex++;

            return await RunAsync(context);
        }
    }

    public abstract class WorkflowStage
    {
        public WorkflowStage(WorkflowStageTypes stageType = WorkflowStageTypes.Continue)
        {
            StageType = stageType;
        }

        public virtual Task ActivateAsync(WorkflowContext context)
        {
            return Task.CompletedTask;
        }

        public virtual Task ContinueAsync(WorkflowContext context, string[] data)
        {
            return Task.CompletedTask;
        }

        public WorkflowStageTypes StageType { get; }

    }

    public enum WorkflowStageTypes
    {
        Continue,
        UserWait,
        CallWait
    }
}
