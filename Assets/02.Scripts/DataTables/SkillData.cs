using System.Collections.Generic;
using UnityEngine;

public class SkillData : ICSVParsing
{
    public string id { get; set; }
    public string name; //  ID
    public string desc; //  ID
    public string iconSpriteId; //  Resources ���

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
