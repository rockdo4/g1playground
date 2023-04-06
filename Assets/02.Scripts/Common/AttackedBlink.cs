using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static PlayerAttack;

public class AttackedBlink : MonoBehaviour, IAttackable
{
    [Serializable]
    public struct BlinkRendering
    {
        public Renderer[] renderers;
        public Material normalRendering;
        public Material invisibleRendering;

        public void Opaque()
        {
            foreach (var renderer in renderers)
            {
                renderer.material = normalRendering;
            }
        }
        public void Transparent()
        {
            foreach (var renderer in renderers)
            {
                renderer.material = invisibleRendering;
            }
        }
    }
    [SerializeField] public BlinkRendering[] blinks;
    private bool onBlink = false;
    private bool transparentMode = false;
    private float duration = 2f;
    private float timer = 0f;
    private float blinkTimer = 0f;

    public void OnAttack(GameObject attacker, Attack attack, Vector3 attackPos)
    {
        onBlink = true;
        timer = 0f;
        Transparent(true);
    }

    private void OnEnable()
    {
        End();
    }

    private void Update()
    {
        if (onBlink)
        {
            timer += Time.deltaTime;
            blinkTimer += Time.deltaTime;
            if (blinkTimer > 0.2f)
            {
                SwitchMode();
                blinkTimer = 0f;
            }
            if (timer > duration)
                End();
        }
    }

    private void SwitchMode() => Transparent(!transparentMode);

    private void Transparent(bool transparent)
    {
        transparentMode = transparent;
        if (transparent)
        {
            foreach (var blink in blinks)
            {
                blink.Transparent();
            }
        }
        else
        {
            foreach (var blink in blinks)
            {
                blink.Opaque();
            }
        }
    }

    private void End()
    {
        onBlink = false;
        timer = 0f;
        blinkTimer = 0f;
        Transparent(false);
    }
}
