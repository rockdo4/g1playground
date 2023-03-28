using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerFootSound", menuName = "ScriptableSound/PlayerFootSound")]
public class PlayerFootSound : ScriptableObject
{
    public GroundType.Type groundType;
    public string[] soundClips;
}
