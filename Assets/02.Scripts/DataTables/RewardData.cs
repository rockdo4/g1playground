using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RewardData : ICSVParsing
{
    public string id { get; set; }
    public int powder;
    public int essence;
    public int skill_piece;
    public int equipe_piece;
    public int exp;

    public void Parse(Dictionary<string, string> line)
    {
        id = line["Reward_ID"];
        powder = Int32.Parse(line["Powder"]);
        essence = Int32.Parse(line["Essence"]);
        skill_piece = Int32.Parse(line["skill_Piece"]);
        equipe_piece = Int32.Parse(line["equipe_Piece"]);
        exp = Int32.Parse(line["exp"]);
    }
}
