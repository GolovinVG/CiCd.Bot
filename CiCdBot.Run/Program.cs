using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CiCd.Domain;
using Telegram.Bot;
using CiCdBot.Run.BotCore;

namespace CiCdBot.Run
{
    class Program
    {
        public static IList<ProjectChat> Chats = new List<ProjectChat>();

        static async Task Main(string[] args)
        {
            var token = "1717008846:AAH1ABoirW5BupdwN0SDmvL8p2b-6jPJves";

            var client = new TelegramBotClient(token);

            var botCore = new BotEngine();

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

            Console.ReadLine();
        }
    }

}
