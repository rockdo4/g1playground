using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;


public class SaveDungeonData : SaveData
{
    public SaveDungeonData()
    {
        type = Types.Dungeon;
    }
}

public class SaveDugeonDataVer1 : SaveDungeonData
{
    public string mondaylv;
    public string tuesdaylv;
    public string wednesdaylv;
    public string thursdaylv;
    public string fridaylv;
    public string playedtime;
    public string playedday;
    public string weekend;

    public SaveDugeonDataVer1()
    {
        version = 1;
    }
    public override SaveData VersionUp()
    {
        return this;
    }

    public override SaveData VersionDown()
    {
        return this;
    }

    
}
