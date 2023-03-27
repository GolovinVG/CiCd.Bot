namespace CiCdBot.Run.BotCore
{
    public class AskStagesActivationConfigBuilder : StagesActivationConfigBuilder
    {
        public AskStagesActivationConfigBuilder(StagesConfigBuilder stagesBuilder, string question, string parameter) : base(stagesBuilder)
        {
            Question = question;
            Parameter = parameter;
        }

        public string Question { get; }
        public string Parameter { get; }
    }
}