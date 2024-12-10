using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerJumpSound", menuName = "ScriptableSound/PlayerJumpSound")]
public class PlayerJumpSound : ScriptableObject
{
    public GroundType.Type groundType;
    public string jumpClip;
    public string landingClip;
}


