using System.Collections.Generic;

namespace CiCd.Domain;

public class ProjectChat
{
    public long Id { get; set; }
    public bool IsActivated { get; set; }
    public ChatMember Owner { get; set; }
    public ICollection<ChatMember> Members { get; set; } = new List<ChatMember>();
    public ChatWorkflow? ChatWorkflow { get; set; }

    public DevProject? Project { get; set; }
}