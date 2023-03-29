using System;
using System.Collections.Generic;
using Telegram.Bot.Types;

namespace CiCdBot.Run.BotCore
{
    public class WorkflowRunningContext
    {
        public User User { get; set; }
        public IDictionary<string, string> Data { get; } = new Dictionary<string, string>();

        public DateTime CreatedDate { get; set; }

        public DateTime LastCall { get; set; }

        public WorkflowInstance WorkflowInstance { get; set; }
    }
}
