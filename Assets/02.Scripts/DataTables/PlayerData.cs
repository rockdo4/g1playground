using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData : StatusData
{
    public int level;

    public override void Parse(Dictionary<string, string> line)
    {
        base.Parse(line);
        level = int.Parse(line["Level"]);
    }
}
