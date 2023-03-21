using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoryData : ICSVParsing
{
    public string id { get; set; }
    public string storyLine;
    public string iconId;

    public void Parse(Dictionary<string, string> line)
    {
        id = line["ID"];
        storyLine = line["Story_Line"];
        iconId = line["Icon_ID"];
    }
}
