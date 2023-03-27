namespace CiCd.Domain;

public class ChatWorkflow
{
    public long ContinueBy { get; set; }
    public ChatWorkflowContinueType ContinueType { get; set; }
    public string StepName { get; set; }
}

public enum ChatWorkflowContinueType{
    Text,
    Command
}
