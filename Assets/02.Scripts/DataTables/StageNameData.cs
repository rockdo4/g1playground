using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageNameData : ICSVParsing
{
    public string id { get; set; }
    public string stageName;


    public void Parse(Dictionary<string, string> line)
    {
        id = line["ID"];
        stageName = line["Stage_Name"];        
    }
}
