using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectAutoReturn : MonoBehaviour
{
    public string effectName;
    private ParticleSystem system;
    private float timeLeft;

    private void Awake()
    {
        system = GetComponent<ParticleSystem>();
    }

    private void OnEnable()
    {
        var main = system.main;
        timeLeft = main.startLifetimeMultiplier + main.duration;
        GameManager.instance.effectManager.ReturnEffectOnTime(effectName, gameObject, timeLeft);
    }

    private void Update()
    {
        if (system.isStopped)
            GameManager.instance.effectManager.ReturnEffect(effectName, gameObject);
    }
}
