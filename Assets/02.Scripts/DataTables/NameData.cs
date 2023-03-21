using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NameData : ICSVParsing
{
   public string id { get; set; }
   public string name;

    public void Parse(Dictionary<string, string> line)
    {
        id = line["Name_ID"];
        name = line["name"];
    }
}
