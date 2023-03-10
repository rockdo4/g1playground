using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    public int playerCurrHp;
    public string lastMapId;
    public Vector3 lastPlayerPos;

    public override SaveData VersionUp()
    {
        return this;
    }

    public override SaveData VersionDown()
    {
        return this;
    }
}
