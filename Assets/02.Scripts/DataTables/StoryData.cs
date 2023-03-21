using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoryData : ICSVParsing
{
    public string id { get; set; }
    public string storyScript;
    public string iconSpriteId;
    public Sprite iconSprite;

    public virtual void Parse(Dictionary<string, string> line)
    {
        id = line["ID"];
        storyScript = line["Story_Line"];
        iconSpriteId = line["Icon_ID"];

        iconSprite = Resources.Load<Sprite>(iconSpriteId);
    }
}
