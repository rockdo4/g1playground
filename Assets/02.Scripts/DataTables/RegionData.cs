using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RegionData : ICSVParsing
{
    public string id { get; set; }
    public string region;
    public string index;

    public void Parse(Dictionary<string, string> line)
    {
        id = line["ID"];
        region = line["Region"];
        index = line["Index"];
    }
}
