using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum WeaponTypes
{
    Knuckle,
    Sword,
    Blunt,
    Spear,
}
public class WeaponData : ItemData
{
    public WeaponTypes weaponType;
    public ItemClass weaponClass;
    public int reinforce;
    public int maxReinforce;
    public string iconId;
    public int maximumAttack;

    public int sellable;
    public int sellPowder;
    public int addStr;
    public int addDex;
    public int addInt;
    public int addMeleePower;
    public int addSkillPower;
    public float addMeleeCriChance;
    public float addMeleeCriDamage;
    public float addSkillCriChance;
    public float addSkillCriDamage;

    public override void Parse(Dictionary<string, string> line)
    {
        base.Parse(line);
        id = line["Weapon_ID"];
        weaponType = (WeaponTypes)System.Enum.Parse(typeof(WeaponTypes), line["Type"]);
        weaponClass = (ItemClass)System.Enum.Parse(typeof(ItemClass), line["Class"]);
        reinforce = int.Parse(line["Wea_Reinforce"]);
        maxReinforce = int.Parse(line["max_Wea_Reinforce"]);
        maximumAttack = int.Parse(line["Maximum_atk"]);

        sellable = int.Parse(line["Sellable"]);
        sellPowder = int.Parse(line["sellPowder"]);
        addStr = int.Parse(line["addStr"]);
        addDex = int.Parse(line["addDex"]);
        addInt = int.Parse(line["addInt"]);
        addMeleePower = int.Parse(line["addMeleepow"]);
        addSkillPower = int.Parse(line["addSkillpow"]);
    }
}