using System;
using System.Threading.Tasks;
using Telegram.Bot;
using CiCdBot.Run.BotCore;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using System.IO;

namespace CiCdBot.Run;

partial class Program
{
    static async Task Main(string[] args)
    {
        var token = "1717008846:AAH1ABoirW5BupdwN0SDmvL8p2b-6jPJves";

        var client = new TelegramBotClient(token);

        var host = Host.CreateDefaultBuilder(args);
        host.ConfigureServices(o =>
            o.AddSingleton<IProjectStorage, OfflineProjectStorage>()
        );

        Directory.SetCurrentDirectory(Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location));

        var botCore = new BotEngine(host.Build());
        botCore.SetupWorkflow(builder =>
        {
            builder.SetupIdle().Workflow
                .Wait<IdleHandler>();

            builder.Setup("Activate").UserBind().Workflow
                .Say("Чат активирван")
                .Then.Ask("Данные проекта... название:", "ProjectName")
                .Then.Ask("версия:", "ProjectVersion")
                .Then.Wait<SetProjectHandler>();

            builder.Setup("ProjectInfo")
                .Workflow
                .Wait<ProjectInfoHandler>();
        });

        botCore.Run(client);
    }
}
