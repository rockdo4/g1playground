using System.Collections;
using System.Collections.Generic;

public class PlayerData : StatusData
{
    public override void Parse(Dictionary<string, string> line)
    {
        id = line["Level"];
        meleePower = int.Parse(line["MeleePow"]);
        skillPower = int.Parse(line["SkillPow"]);
        meleeCriChance = float.Parse(line["MeleeCriChance"]);
        meleeCriDamage = float.Parse(line["MeleeCriDamage"]);
        skillCriChance = float.Parse(line["SkillCriChance"]);
        skillCriDamage = float.Parse(line["SkillCriDamage"]);
        meleeDef = int.Parse(line["addMeleeDef"]);
        skillDef = int.Parse(line["addSkillDef"]);
        maxHp = int.Parse(line["Health"]);
        maxMp = int.Parse(line["Mana"]);
        str = int.Parse(line["Str"]);
        dex = int.Parse(line["Dex"]);
        intel = int.Parse(line["Int"]);
    }
}
