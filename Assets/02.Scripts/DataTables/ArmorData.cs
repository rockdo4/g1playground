using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ArmorData : ItemData
{
    public ItemClass armorClass;
    public int reinforce;
    public int maxReinforce;

    public int sellable;
    public int sellPowder;
    public int addStr;
    public int addDex;
    public int addInt;
    public int addMeleeDef;
    public int addSkillDef;

    public override void Parse(Dictionary<string, string> line)
    {
        base.Parse(line);
        id = line["Armor_ID"];
        armorClass = (ItemClass)System.Enum.Parse(typeof(ItemClass), line["Class"]);
        reinforce = int.Parse(line["arm_reinforce"]);
        maxReinforce = int.Parse(line["max_arm_reinforce"]);
        sellable = int.Parse(line["sellable"]);
        sellPowder = int.Parse(line["sellPowder"]);
        addStr = int.Parse(line["addStr"]);
        addDex = int.Parse(line["addDex"]);
        addInt = int.Parse(line["addInt"]);
        addMeleeDef = int.Parse(line["addMeleeDef"]);
        addSkillDef = int.Parse(line["addSkillDef"]);
    }
}