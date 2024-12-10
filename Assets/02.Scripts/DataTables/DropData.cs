using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropData : ICSVParsing
{
    public string id { get; set; }
    public string ItemId;
    public float itemRate;
    public float essenceRate;
    public int minQtPowder;
    public int maxQtPowder;
    public void Parse(Dictionary<string, string> line)
    {
        id = line["Drop_ID"];
        ItemId = line["Item_ID"];
        itemRate = float.Parse(line["Item_Rate"]);
        essenceRate = float.Parse(line["Essence_Rate"]);
        minQtPowder = int.Parse(line["minQt_powder"]);
        maxQtPowder = int.Parse(line["maxQt_powder"]);
    }
}
