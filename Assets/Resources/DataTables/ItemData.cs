using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemTypes
{
    Weapon,
    Armor,
    Consumable,
}

public class ItemData : ICSVParsing
{
    public string id { get; set; }
    public ItemTypes type;
    public string name; //  ID
    public string desc; //  ID
    public string iconSpriteId; //  Resources °æ·Î

    public Sprite iconSprite;

    public void Parse(Dictionary<string, string> line)
    {
        id = line["ID"];
        type = (ItemTypes)System.Enum.Parse(typeof(ItemTypes), line["Type"]);
        name = line["Name"];
        desc = line["Desc"];
        iconSpriteId = line["IconSpriteId"];

        iconSprite = Resources.Load<Sprite>(iconSpriteId);
    }
}
