using System.Collections.Generic;
using UnityEngine;

public class SkillData : ICSVParsing
{
    public string id { get; set; }
    public string name; //  ID
    public string desc; //  ID
    public string iconSpriteId; //  Resources ���
    public string group;
    public int reinforce;
    public int maxReinfore;
    public int reqMana;
    public float damageFigure;
    public float criticalChance;
    public float criticalDamage;
    public float lifeTime;
    public float knockBackForce;
    public float stunTime;
    public float slowDown;
    public float slowTime;
    public float reduceDef;
    public float reduceDefTime;
    public float CoolDown;

    public virtual void Parse(Dictionary<string, string> line)
    {
        id = line["ID"];
        name = line["Name_ID"];
        desc = line["Desc_ID"];
        iconSpriteId = line["Icon_ID"];
        reinforce = int.Parse(line["skillreinforce"]);
        maxReinfore = int.Parse(line["maxskillreinforce"]);
        CoolDown = float.Parse(line["coolTime"]);
        reqMana = int.Parse(line["reqMana"]);
        damageFigure = float.Parse(line["damageFigure"]);
        criticalChance = float.Parse(line["criticalPercent"]);
        criticalDamage = float.Parse(line["criticalDamage"]);
        stunTime = float.Parse(line["stunTime"]);
        knockBackForce = float.Parse(line["kBForce"]);
        slowDown = float.Parse(line["slowDown"]);
        slowTime = float.Parse(line["slowTime"]);
        reduceDef = float.Parse(line["reduceDef"]);
        reduceDefTime = float.Parse(line["reduceDefTime"]);
        group = line["SkillGroup"];

        lifeTime = 0f;
    }
}
