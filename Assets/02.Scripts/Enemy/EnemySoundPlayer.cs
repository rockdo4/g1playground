using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySoundPlayer : MonoBehaviour
{
    public enum EnemyType
    {
        None = -1,
        Death,
        EarthWorm,
        EvilMage,
        Golem,
        GreenBug,
        GreenOrc,
        GreenPlant,
        GrimReaper,
        LavaSpirit,
        MerryBee,
        PurpleBomb,
    }
    
    [SerializeField] private EnemyEffectSound effectSound;

    public void PlayAttackSound()
    {
        var clip = effectSound.attackClip;
        SoundManager.instance.PlayEnemyEffect(clip);
    }

    public void PlayDeathSound()
    {
        var clip = effectSound.deathClip;
        SoundManager.instance.PlayEnemyEffect(clip);
    }
}
