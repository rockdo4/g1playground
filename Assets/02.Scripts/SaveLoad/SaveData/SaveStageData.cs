using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;


public class SaveStageData : SaveData
{
    public SaveStageData()
    {
        type = Types.Stage;
    }
}

public class SaveStageDataVer1 : SaveStageData
{
    public Dictionary<string, bool> unlock;
    public Dictionary<string, bool> isStoryStage;
   
    public SaveStageDataVer1()
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
