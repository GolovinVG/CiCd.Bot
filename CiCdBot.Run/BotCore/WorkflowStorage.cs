using System.Collections.Generic;
using System.Linq;
using CiCdBot.Run.BotCore.Workflow;
using Telegram.Bot.Types;

namespace CiCdBot.Run.BotCore
{
    public class WorkflowStorage : IWorkflowStorage
    {
        private IDictionary<long, ICollection<WorkflowRunningContext>> _runningContexts = new Dictionary<long, ICollection<WorkflowRunningContext>>();
        public WorkflowRunningContext[] GetActiveContext(Message message)
        {
            if (_runningContexts.ContainsKey(message.From.Id) == false) return Enumerable.Empty<WorkflowRunningContext>().ToArray();
            var currentContexts = _runningContexts[message.From.Id];
            var contextToContinue = currentContexts.Where(x => x.WorkflowInstance.CurrentStage.StageType == WorkflowStageTypes.UserWait);
            return contextToContinue.ToArray();

        }

        public void RemoveActiveRunningContext(Message message, WorkflowRunningContext runningContext)
        {
            if (_runningContexts.ContainsKey(message.From.Id))
                _runningContexts[message.From.Id].Remove(runningContext);
        }

        public void SaveRunningContext(Message message, WorkflowRunningContext runningContext)
        {
            if (_runningContexts.ContainsKey(message.From.Id) == false)
            {
                _runningContexts.Add(message.From.Id, new List<WorkflowRunningContext>() { runningContext });
                return;
            }

            var contexts = _runningContexts[message.From.Id];
            var exist = contexts.FirstOrDefault(x => x.Id == runningContext.Id);
            if (exist == null)
            {
                _runningContexts[message.From.Id].Add(runningContext);
                return;
            }

            contexts.Remove(exist);
            _runningContexts[message.From.Id].Add(runningContext);
        }
    }
}
