using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReinforceData : ICSVParsing
{
    public string id { get; set; }
    public string material1;
    public int material1Count;
    public int powder;
    public int essence;
    public string result;

    public void Parse(Dictionary<string, string> line)
    {
        id = line["Reinforce_ID"];
        material1 = line["material_1_ID"];
        material1Count = int.Parse(line["material_1_count"]);
        powder = int.Parse(line["powder"]);
        essence = int.Parse(line["Essence"]);
        result = line["Result"];
    }
}
