using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;


public class SavePlayedData : SaveData
{
    public SavePlayedData()
    {
        type = Types.Story;
    }
}

public class SavePlayedDataVer1 : SavePlayedData
{
    public List<int> playedId=new List<int>();

    public SavePlayedDataVer1()
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
