using System.Collections.Generic;
using System;

public class HealthData : ICSVParsing
{
    public string id { get; set; }
    public string name { get; private set; }
    public int maxHp { get; private set; }
    public int growthHp { get; private set; }

    public void Parse(Dictionary<string, string> line)
    {
        id = line["ID"];
        name = line["Name"];
        maxHp = Convert.ToInt32(line["MaxHp"]);
        if (!string.IsNullOrEmpty(line["GrowthHp"]))
            growthHp = Convert.ToInt32(line["GrowthHp"]);
    }
}