using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveData
{
    public enum Types
    {
        None = -1,
        Player,
        Option,
    }
    public Types type = Types.None;
    public int version;
    public virtual SaveData VersionUp() { return null; }
    public virtual SaveData VersionDown() { return null; }
}
