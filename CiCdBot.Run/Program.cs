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

namespace CiCdBot.Run
{
    class Program
    {
        public static IList<ProjectChat> Chats = new List<ProjectChat>();

        static async Task Main(string[] args)
        {
            var token = "1717008846:AAH1ABoirW5BupdwN0SDmvL8p2b-6jPJves";

            var client = new TelegramBotClient(token);

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


            Console.ReadLine();

        }

        public static async Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
        {
            // Некоторые действия
            Console.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(exception));
        }

        public static async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            // Некоторые действия
            Console.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(update));
            if (update.Type == UpdateType.Message)
            {
                var message = update.Message;

                if (message.Type == MessageType.GroupCreated)
                {
                    await botClient.SendTextMessageAsync(message.Chat, "/Activate (/Abort)");
                    return;
                }

                if (message.Type == MessageType.Text)
                {
                    if (message.Chat.Type != ChatType.Group) return;

                    var me = await botClient.GetMeAsync(cancellationToken);
                    var meInChat = await botClient.GetChatMemberAsync(update.Message.Chat.Id, me.Id, cancellationToken);

                    var botCommands = new List<string>();

                    var activeChat = Chats.FirstOrDefault(x => x.Id == message.Chat.Id);

                    for (int i = 0; i < message.Entities?.Length; i++)
                    {
                        if (message.Entities[i].Type == MessageEntityType.BotCommand)
                        {
                            var commandParts = message.Text.Substring(message.Entities[i].Offset, message.Entities[i].Length).Split("@");

                            if (commandParts.Length == 2 && commandParts[1].Equals(meInChat.User.Username))
                                botCommands.Add(commandParts[0]);
                        }
                    }


                    if (botCommands?.Any() == false) {
                        if (activeChat?.ChatWorkflow?.ContinueBy == message.From.Id
                        && activeChat?.ChatWorkflow?.ContinueType == ChatWorkflowContinueType.Text){
                            var enteredValue = message.Text.Trim();

                            if (activeChat.ChatWorkflow.StepName.Equals("EnterProjectName")){
                                activeChat.Project ??= new  DevProject(){
                                    Name = enteredValue
                                };
                                await botClient.SendTextMessageAsync(message.Chat, "/ProjectInfo");
                                activeChat.ChatWorkflow = null;
                                return;
                            }
                        }


                    };

                    foreach (var item in botCommands)
                    {
                        if (item.Equals("/Activate"))
                        {
                            if (activeChat == null)
                            {

                                var chat = new ProjectChat
                                {
                                    Id = message.Chat.Id
                                };

                                Chats.Add(chat);

                                var member = new CiCd.Domain.ChatMember
                                {
                                    Id = message.From.Id,
                                    Name = message.From.Username
                                };

                                chat.Owner = member;
                                chat.Members.Add(member);
                                chat.IsActivated = true;

                                await botClient.SendTextMessageAsync(message.Chat, "Чат активирван, загружаем даные проекта");
                                activeChat = chat; ;
                            }
                            activeChat.ChatWorkflow = new CiCd.Domain.ChatWorkflow()
                            {
                                ContinueBy = activeChat.Owner.Id,
                                ContinueType = ChatWorkflowContinueType.Text,
                                StepName = "EnterProjectName"
                            };

                            return;
                        }

                        if (item.Equals("/Abort"))
                        {
                            if (activeChat != null && activeChat.ChatWorkflow != null)
                            {
                                activeChat.ChatWorkflow = null;
                                await botClient.SendTextMessageAsync(message.Chat, "Операция прерванна");
                            }
                        }

                        if (item.Equals("/ProjectInfo"))
                        {
                            if (activeChat != null && activeChat.ChatWorkflow == null && activeChat.Project != null)
                            {
                                await botClient.SendTextMessageAsync(message.Chat, $"Проект: {activeChat.Project.Name}");
                            }
                        }
                    }

                    return;
                }
                await botClient.SendTextMessageAsync(message.Chat, "Привет-привет!!");
            }
        }
    }
}