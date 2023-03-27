namespace CiCdBot.Run.BotCore
{
    public class WorkflowConfigBuilder
    {
        private bool _userBind;
        public StagesConfigBuilder _stagesConfigBuilder = new StagesConfigBuilder();

        public WorkflowConfigBuilder()
        {
        }

        public StagesConfigBuilder Workflow => _stagesConfigBuilder;

        internal WorkflowConfigBuilder UserBind()
        {
            _userBind = true;
            return this;
        }
    }

}
