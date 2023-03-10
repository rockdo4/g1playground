using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveCheckPointData : SaveData
{
    public SaveCheckPointData()
    {
        type = Types.CheckPoint;
    }
}

public class SaveCheckPointDataVer1 : SaveCheckPointData
{
    public string currentMapName;
    public string x;
    public string y;
    public string z;

    public SaveCheckPointDataVer1()
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
