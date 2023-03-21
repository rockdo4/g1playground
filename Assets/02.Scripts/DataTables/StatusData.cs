using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusData : ICSVParsing
{
    public string id { get; set; }
    public int meleePower;
    public int skillPower;
    public float meleeCriChance;
    public float meleeCriDamage;
    public float skillCriChance;
    public float skillCriDamage;
    public int meleeDef;
    public int skillDef;
    public int maxHp;
    public int maxMp;
    public int str;
    public int dex;
    public int intel;

    public virtual void Parse(Dictionary<string, string> line)
    {
        meleePower = 0;
        skillPower = 0;
        meleeCriChance = 0f;
        skillCriChance = 0f;
        meleeDef = 0;
        skillDef = 0;
        maxMp = 0;
        meleeCriDamage = 2f;
        skillCriDamage = 1.5f;
    }
}
