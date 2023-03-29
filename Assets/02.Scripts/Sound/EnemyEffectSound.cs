using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyEffectSound", menuName = "ScriptableSound/EnemyEffectSound")]
public class EnemyEffectSound : ScriptableObject
{
    public EnemySoundPlayer.EnemyType type;
    public string attackClip;
    public string deathClip;
    
}
