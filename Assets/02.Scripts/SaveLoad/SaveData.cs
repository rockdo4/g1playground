using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveData
{
    public int Version;
    public virtual SaveData VersionUp() { return null; }
    public virtual SaveData VersionDown() { return null; }
}

public class SaveDataVer1 : SaveData
{
    public SaveDataVer1()
    {
        Version = 1;
    }

    public string playerName;
    public int playerCurrHp;
    public int lastMapId;
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
