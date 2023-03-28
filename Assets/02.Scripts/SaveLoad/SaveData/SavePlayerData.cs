using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static PlayerInventory;

public class SavePlayerData : SaveData
{
    public SavePlayerData()
    {
        type = Types.Player;
    }
}


public class SavePlayerDataVer1 : SavePlayerData
{
    public SavePlayerDataVer1()
    {
        version = 1;
    }

    public string playerName;
    public string lastMapId;
    public Vector3 lastPlayerPos;
    public string lastChapter;

    public int playerCurrHp;
    public int playerCurrMp;

    public List<string> weapons;
    public List<string> armors;
    public List<Consumable> consumables;
    public string currWeapon;
    public string currArmor;

    public List<string> possessedSkills;
    public int currskill1;
    public int currskill2;

    public int currLevel;
    public int currExp;

    public bool endTutorial = false;

    public override SaveData VersionUp()
    {
        return this;
    }

    public override SaveData VersionDown()
    {
        return this;
    }
}
