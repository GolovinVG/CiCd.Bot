namespace CiCdBot.Run.BotCore
{
    public class SayStagesActivationConfigBuilder : StagesActivationConfigBuilder
    {
        public SayStagesActivationConfigBuilder(StagesConfigBuilder stagesBuilder, string caption) : base(stagesBuilder)
        {
            Caption = caption;
        }

        public string Caption { get; }

        internal override WorkflowStage Build()
        {
            return new SayWorkflowStage(Caption);
        }
    }
}
