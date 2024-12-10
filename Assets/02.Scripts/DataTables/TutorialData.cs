using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialData : ICSVParsing
{
    public string id { get; set; }
    public string index;

    public void Parse(Dictionary<string, string> line)
    {
        id = line["ID"];
        index = line["Index"];
    }
}
