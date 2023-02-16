using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SaveData
{
    public abstract int Version { get; }
    public abstract SaveData VersionUp();
    public abstract SaveData VersionDown();
}

public class SaveDataVer1 : SaveData
{
    public override int Version => 1;

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
