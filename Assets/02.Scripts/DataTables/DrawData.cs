using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawData : ICSVParsing
{
    public string id { get; set; }
    public string itemId;
    public float rate;
    public void Parse(Dictionary<string, string> line)
    {
        id = line["Draw_ID"];
        itemId = line["Item_ID"];
        rate = float.Parse(line["Rate"]);
    }
}
