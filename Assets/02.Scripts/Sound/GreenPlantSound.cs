using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GreenPlantSound", menuName = "ScriptableSound/GreenPlantSound")]
public class GreenPlantSound : ScriptableObject
{
    public string spawnClip;
    public string biteAttackClip;
    public string dashAttackClip;
    public string projectileAttackClip;
    public string dieClip;
}
