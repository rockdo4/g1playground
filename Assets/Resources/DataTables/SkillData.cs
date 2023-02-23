using UnityEngine;

public enum SkillTypes
{
    Immediate,
    Dot,
}

public class SkillData
    //
{
    public string id { get; set; }
    public SkillTypes type;
    public string name; //  ID
    public string desc; //  ID
    public string iconSpriteId; //  Resources °æ·Î

    public Sprite iconSprite;

    //public void Parse(Dictionary<string, string> line)
    //{
    //    id = line["ID"];
    //    type = (SkillTypes)System.Enum.Parse(typeof(SkillTypes), line["Type"]);
    //    name = line["Name"];
    //    desc = line["Desc"];
    //    iconSpriteId = line["IconSpriteId"];

    //    iconSprite = Resources.Load<Sprite>(iconSpriteId);
    //}
}
