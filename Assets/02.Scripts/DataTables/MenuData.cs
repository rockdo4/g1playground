using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuData : ICSVParsing
{
    public int index;
    public string id { get; set; }
    public string text;

    public void Parse(Dictionary<string, string> line)
    {
        index = int.Parse(line["index"]);
        id = line["Menu_ID"];
        text = line["Text"];
    }
}
