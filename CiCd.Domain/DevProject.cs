﻿using System.Collections;
using System.Collections.Generic;

namespace CiCd.Domain;
[Serializable]
public class DevProject
{
    public string Name { get; set; }

    public ICollection<ChatSubject> Members { get; set; }
    public string Version { get; set; }
}
