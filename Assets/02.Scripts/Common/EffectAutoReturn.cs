using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectAutoReturn : MonoBehaviour
{
    public string effectName;
    private float timeLeft;

    private void Awake()
    {
        ParticleSystem system = GetComponent<ParticleSystem>();
        var main = system.main;
        timeLeft = main.startLifetimeMultiplier + main.duration;
        GameManager.instance.effectManager.ReturnEffectOnTime(effectName, gameObject, timeLeft);
    }
}
