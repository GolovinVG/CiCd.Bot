using System.Collections.Generic;

namespace CiCd.Domain;

[Serializable]
public class ProjectChat
{
    public long Id { get; set; }
    public bool IsActivated { get; set; }
    public ChatSubject? Owner { get; set; }
    public ICollection<ChatSubject> Members { get; set; } = new List<ChatSubject>();

    public DevProject? Project { get; set; }
}