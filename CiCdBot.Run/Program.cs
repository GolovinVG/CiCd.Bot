using System;
using System.Threading.Tasks;
using Telegram.Bot;
using CiCdBot.Run.BotCore;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using System.IO;
using CiCdBot.Run.BotCore.Logic;
using CiCdBot.Run.BotCore.Workflow;
using CiCdBot.Run.BotCore.ChatLifeCycle;
using CiCdBot.Run.BotCore.ChatLifeCycle.Events;
using CiCdBot.Run.BotCore.ChatLifeCycle.EventHandlers;

namespace CiCdBot.Run;

partial class Program
{
    static async Task Main(string[] args)
    {
        var token = "1717008846:AAH1ABoirW5BupdwN0SDmvL8p2b-6jPJves";

        var client = new TelegramBotClient(token);

        var host = Host.CreateDefaultBuilder(args);

        var builder = WorkflowSetupBuilder.SetupWorkflow(b =>
        {
            b.SetupIdle().Workflow
                .Wait<IdleHandler>();

            b.Setup("Activate").UserBind().Workflow
                .Say("Чат активирван")
                .Then.Ask("Данные проекта... название:", "ProjectName")
                .Then.Ask("версия:", "ProjectVersion")
                .Then.Wait<SetProjectHandler>();

            b.Setup("ProjectInfo")
                .Workflow
                .Wait<ProjectInfoHandler>();
        });

        host.ConfigureServices(o =>
            o
            .AddSingleton<IProjectStorage, OfflineProjectStorage>()
            .AddSingleton<IBaseMessageHandler, MessageOnlyFilterBaseMessageHandler>()
            .AddSingleton<IBotIdentityProvider, BotIdentityProvider>()
            .AddSingleton<IBotLiveCycleMediator, BotLiveCycleMediator>()
            .AddSingleton<IWorkflowSetupBuilder>(builder)
            .AddSingleton<IWorkflowContextFactory, WorkflowContextFactory>()
            .AddSingleton<IWorkflowStorage, WorkflowStorage>()
            .AddSingleton<GroupChatStrategyFactory>()
            .AddSingleton<PrivateChatStrategyFactory>()

            .AddSingleton<IBotLiveCycleEventHandler<BeginWaitingWorkflowGroupChatEvent>, BeginWaitingWorkflowGroupChatHandler>()
            .AddSingleton<IBotLiveCycleEventHandler<AddedToGroupChatEvent>, AddedToGroupChatHandler>()
            .AddSingleton<IBotLiveCycleEventHandler<CompleteWorkflowGroupChatEvent>, CompleteWorkflowGroupChatHandler>()
            .AddSingleton<IBotLiveCycleEventHandler<DirectMessageGroupChatEvent>, DirectMessageGroupChatHandler>()
            .AddSingleton<IBotLiveCycleEventHandler<RecieveUserResponseGroupChatEvent>, RecieveUserResponseGroupChatHandler>()
            .AddSingleton<IBotLiveCycleEventHandler<RunCommandGroupChatEvent>, UserRequestedCommanadGroupChatHandler>()

            .AddSingleton<IChatStrategyResolver>(sm => new ChatStrategyResolver(sm.GetRequiredService<GroupChatStrategyFactory>(), sm.GetRequiredService <PrivateChatStrategyFactory>()))
        );

        Directory.SetCurrentDirectory(Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location));

        var botCore = new BotEngine(host.Build());

        botCore.Run(client);
    }
}
