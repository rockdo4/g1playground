using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyData : StatusData
{
    // public EnemyType type;
    public string name; // ID
    public string desc; // ID
    public string dialog;   // ID
    public string drop; // ID
    public float moveSpeed;
    public float patrolTime;
    public float meleeRange;
    public float skillRange;
    public float detectRange;
    public float chaseTime;
    public int exp;
    public Sprite iconSprite;

    public override void Parse(Dictionary<string, string> line)
    {
        id = line["mon_id"];
        //type = line["monType"]; -> need to parse if type is not string
        name = line["Name_ID"];
        desc = line["Desc_ID"];
        dialog = line["Dialogue_ID"];
        drop = line["drop_id"];
        meleePower = int.Parse(line["monMeleePow"]);
        skillPower = int.Parse(line["monSkillPow"]);
        meleeCriChance = float.Parse(line["monMeleeCriChance"]);
        meleeCriDamage = float.Parse(line["monMeleeCriDamage"]);
        skillCriChance = float.Parse(line["monSkillCriChance"]);
        skillCriDamage = float.Parse(line["monSkillCriDamage"]);
        meleeDef = int.Parse(line["monMeleeDef"]);
        skillDef = int.Parse(line["monSkillDef"]);
        moveSpeed = float.Parse(line["monMoveSpeed"]);
        patrolTime = float.Parse(line["patrolTime"]);
        meleeRange = float.Parse(line["monMeleeRange"]);
        skillRange = float.Parse(line["monSkillRange"]);
        detectRange = float.Parse(line["detectRange"]);
        chaseTime = float.Parse(line["chaseTime"]);
        exp = int.Parse(line["exp"]);
        maxHp = int.Parse(line["monHealth"]);
        str = int.Parse(line["monStr"]);
        dex = int.Parse(line["monDex"]);
        intel = int.Parse(line["monInt"]);
        iconSprite = Resources.Load<Sprite>(line["Icon_ID"]);
    }
}
