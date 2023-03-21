using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemTypes
{
    Weapon,
    Armor,
    Consumable,
}
public enum ItemClass
{
    Normal,
    Rare,
    Unique,
    Legendary,
}

public class ItemData : ICSVParsing
{
    public string id { get; set; }
    public string name; //  ID
    public string desc; //  ID
    public string iconSpriteId; //  Resources ���

    public virtual void Parse(Dictionary<string, string> line)
    {
        name = line["Name_ID"];
        desc = line["Desc_ID"];
        iconSpriteId = line["Icon_ID"];
    }
}
