using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PurpleRichSound", menuName = "ScriptableSound/PurpleRichSound")]
public class PurpleRichSound : ScriptableObject
{
    public string normalAttackClip;
    public string areaAttackClip;
    public string projectileAttackClip;
    public string spawnEnemyClip;
    public string groggyClip;
    public string dieClip;
}
