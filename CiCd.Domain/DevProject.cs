using System.Collections;
using System.Collections.Generic;

namespace CiCd.Domain;
public class DevProject
{
    public string Name { get; set; }

    public ProjectChat Chat { get; set; }

    public ICollection<ChatMember> Members { get; set; }
    public string Version { get; set; }
}
