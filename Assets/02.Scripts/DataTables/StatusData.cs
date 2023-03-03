using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusData : ICSVParsing
{
    public string id { get; set; }
    public int attackPower;
    public int skillPower;
    public float criticalChance;
    public float criticalDamage;
    public int defense;
    public int maxHp;
    public int maxMp;
    public int str;
    public int dex;
    public int intel;
    public Sprite iconSprite;

    public virtual void Parse(Dictionary<string, string> line)
    {
        id = line["ID"];
        attackPower = int.Parse(line["AttackPower"]);
        skillPower = int.Parse(line["SkillPower"]);
        criticalChance = float.Parse(line["CriticalChance"]);
        criticalDamage = float.Parse(line["CriticalDamage"]);
        defense = int.Parse(line["Defense"]);
        maxHp = int.Parse(line["MaxHp"]);
        maxMp = int.Parse(line["MaxMp"]);
        str = int.Parse(line["Str"]);
        dex = int.Parse(line["Dex"]);
        intel = int.Parse(line["Int"]);
    }
}
