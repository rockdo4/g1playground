using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopUpData : ICSVParsing
{
    public string id { get; set; }
    public int size;
    public string text;

    public void Parse(Dictionary<string, string> line)
    {
        id = line["ID"];
        size = int.Parse(line["Size"]);
        text = line["Text"];
    }
}
