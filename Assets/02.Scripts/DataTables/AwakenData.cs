using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AwakenData : ICSVParsing
{
    public string id { get; set; }
    public string material1Id;
    public int material1Count;
    public string material2Id;
    public int material2Count;
    public string material3Id;
    public int material3Count;
    public int powder;
    public string result;

    public void Parse(Dictionary<string, string> line)
    {
        id = line["Awaken_ID"];
        material1Id = line["material_1_ID"];
        material1Count = int.Parse(line["material_1_count"]);
        material2Id = line["material_2_ID"];
        material2Count = int.Parse(line["material_2_count"]);
        material3Id = line["material_3_ID"];
        material3Count = int.Parse(line["material_3_count"]);
        powder = int.Parse(line["powder"]);
        result = line["Result"];
    }
}
