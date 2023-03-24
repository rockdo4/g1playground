using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class SkilldisassembleData: ICSVParsing
{
    public string id { get; set; }
    public int skillReinforce;
    public int powder;

    void ICSVParsing.Parse(Dictionary<string, string> line)
    {
        id = line["Skilldisassemble_ID"];
        skillReinforce = int.Parse(line["Skill_Reinforce"]);
        powder = int.Parse(line["Powder"]);
    }
}