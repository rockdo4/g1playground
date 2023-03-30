using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ScorpionSound", menuName = "ScriptableSound/ScorpionSound")]
public class ScorpionSound : ScriptableObject
{    
    public string normalAttackClip;
    public string tailAttackClip;
    public string areaAttackClip;
    public string projectileAttackClip;
    public string groggyClip;
    public string dieClip;
}
