using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComposeData : ICSVParsing
{
    public string id { get; set; }
    public string material1Id;
    public int material1Count;
    public string material2Id;
    public int material2Count;
    public string resultItem;
    void ICSVParsing.Parse(Dictionary<string, string> line)
    {
        id = line["compose_ID"];
        material1Id = line["material_1_ID"];
        material1Count = int.Parse(line["material_1_count"]);
        material1Id = line["material_2_ID"];
        material2Count = int.Parse(line["material_2_count"]);
        resultItem = line["Result_item"];
    }
}
