using System.Collections;
using System.Collections.Generic;

public class PlayerData : StatusData
{
    public override void Parse(Dictionary<string, string> line)
    {
        base.Parse(line);
        id = line["Level"];
        str = int.Parse(line["Str"]);
        dex = int.Parse(line["Dex"]);
        intel = int.Parse(line["Int"]);
        int level = int.Parse(id);
        maxHp = level * 350;
        maxMp = level * 100;
    }
}
