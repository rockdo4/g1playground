using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IconData : ICSVParsing
{
    public string id { get; set; }
    public string iconName;

    public void Parse(Dictionary<string, string> line)
    {
        id = line["Icon_ID"];
        iconName = line["Icon_name"];
    }
}
