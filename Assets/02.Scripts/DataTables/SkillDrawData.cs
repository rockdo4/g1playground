using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillDrawData : ICSVParsing
{
    public string id { get; set; }
    public string skillId;
    public float rate;
    public void Parse(Dictionary<string, string> line)
    {
        id = line["SkillDraw_ID"];
        skillId = line["Skill_ID"];
        rate = float.Parse(line["Rate"]);
    }
}
