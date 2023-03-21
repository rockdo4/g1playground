using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueData : ICSVParsing
{
    public int index;
    public string id { get; set; }
    public int group;
    public string dialogue;
    public int size;

    public void Parse(Dictionary<string, string> line)
    {
        index = int.Parse(line["index"]);
        id = line["Dialogue_ID"];
        group = int.Parse(line["group"]);
        dialogue = line["Dialogue"];
        size = int.Parse(line["Size"]);
    }
}
