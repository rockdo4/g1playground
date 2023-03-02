using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmorData : ItemData
{
    public ItemClass armorClass;
    public int reinforce;
    public int maxReinforce;
    public string iconId;

    public int sellable;
    public int sellPowder;
    public int addStr;
    public int addDex;
    public int addInt;

    public override void Parse(Dictionary<string, string> line)
    {
        base.Parse(line);
        armorClass = (ItemClass)System.Enum.Parse(typeof(ItemClass), line["Class"]);
        reinforce = int.Parse(line["arm_reinforce"]);
        maxReinforce = int.Parse(line["max_arm_reinforce"]);

        sellable = int.Parse(line["Sellable"]);
        sellPowder = int.Parse(line["sellPowder"]);
        addStr = int.Parse(line["addStr"]);
        addDex = int.Parse(line["addDex"]);
        addInt = int.Parse(line["addInt"]);
    }
}