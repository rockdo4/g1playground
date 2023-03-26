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
    public Attack.CC cc;
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
        cc = new Attack.CC();
        cc.knockBackForce = float.Parse(line["kBForce"]);
        cc.stunTime = float.Parse(line["stunTime"]);
        cc.slowDown = float.Parse(line["slowDown"]);
        cc.slowTime = float.Parse(line["slowTime"]);
        cc.reduceDef = int.Parse(line["reduceDef"]);
        cc.reduceDefTime = float.Parse(line["reduceDefTime"]);
        group = line["SkillGroup"];

        lifeTime = 0f;
    }
}
