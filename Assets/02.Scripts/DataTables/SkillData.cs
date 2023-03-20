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
    public bool isKnockbackable;
    public bool isStunnable;
    public float stunTime;
    public float CoolDown;
    public Sprite iconSprite;

    public virtual void Parse(Dictionary<string, string> line)
    {
        id = line["ID"];
        name = line["Name_ID"];
        desc = line["Desc_ID"];
        iconSpriteId = line["Icon_ID"];

        iconSprite = Resources.Load<Sprite>(iconSpriteId);
    }
}
