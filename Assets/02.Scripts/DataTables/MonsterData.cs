using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterData : StatusData
{
    public string name;
    public string desc;
    public string iconId;
    public string dropId;
    public float patrolTime;
    public float monAttackRange;
    public float monDetectRange;
    public int detectRange;
    public float chaseTime;
    public int exp;
    public float damageFigure;

    public override void Parse(Dictionary<string, string> line)
    {
        base.Parse(line);
        id = line["mon_id"];
        name = line["Name_ID"];
        desc = line["Desc_ID"];
        iconId = line["Icon_ID"];
        dropId = line["drop_id"];
        str = int.Parse(line["monStr"]);
        dex = int.Parse(line["monDex"]);
        intel = int.Parse(line["monInt"]);
        monAttackRange = int.Parse(line["monAttackRange"]);
        monDetectRange = int.Parse(line["detectRange"]);
        //monAttackRange = float.Parse(line["monAttackRange"]);
        maxHp = int.Parse(line["monHealth"]);
        detectRange = int.Parse(line["detectRange"]);
        exp = int.Parse(line["exp"]);
        damageFigure = float.Parse(line["damageFigure"]);
    }
}
