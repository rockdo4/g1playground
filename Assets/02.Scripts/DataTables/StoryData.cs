using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoryData : ICSVParsing
{
    public string id { get; set; }
    public string type;
    public string storyLine;
    public string iconId;

    public void Parse(Dictionary<string, string> line)
    {
        id = line["ID"];
        type = line["Type"];
        storyLine = line["Story_Line"];
        iconId = line["Icon_ID"];
    }
}
