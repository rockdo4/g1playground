using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DescData : ICSVParsing
{
    public string id { get; set; }
    public string text;
    public int size;
    public void Parse(Dictionary<string, string> line)
    {
        id = line["Desc_ID"];
        text = line["Text"];
        size = int.Parse(line["Size"]);
    }
}
