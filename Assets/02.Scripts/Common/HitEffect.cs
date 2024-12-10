using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "HitEffect", menuName = "Effects/HitEffect")]
public class HitEffect : ScriptableObject
{
    public string hitEffect;
    public string criticalHitEffect;
}
