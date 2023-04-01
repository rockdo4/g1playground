using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBodyTrigger : MonoBehaviour
{
    [Serializable]
    public struct InvisibleRendering
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

    public float damagedDelay;
    private float timer;
    private float switchTimer;
    private bool isOnDelay = false;
    private bool transparentMode = false;
    public int damage;
    private Vector3 bodyCenter = new Vector3(0f, 0.5f, 0f);
    [SerializeField] public InvisibleRendering player;
    [SerializeField] public InvisibleRendering weapons;

    private void Start()
    {
        Transparent(false);
    }

    private void Update()
    {
        if (isOnDelay)
        {
            switchTimer += Time.deltaTime;
            if (switchTimer > 0.2f)
            {
                SwitchMode();
                switchTimer = 0f;
            }
            timer += Time.deltaTime;
            if (timer > damagedDelay)
                End();
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (isOnDelay)
            return;
        Attack.CC newCC = Attack.CC.None;
        newCC.knockBackForce = 7f;
        Attack attack = new Attack(damage, newCC, false);
        var attackables = transform.parent.GetComponents<IAttackable>();
        var attacker = other.transform.parent.gameObject;
        var bodyPos = transform.position + bodyCenter;
        var attackPos = other.ClosestPoint(bodyPos);
        foreach (var attackable in attackables)
        {
            attackable.OnAttack(attacker, attack, attackPos);
        }
        isOnDelay = true;
        Transparent(true);
    }

    private void SwitchMode() => Transparent(!transparentMode);

    private void Transparent(bool transparent)
    {
        transparentMode = transparent;
        if (transparent)
        {
            player.Transparent();
            weapons.Transparent();
        }
        else
        {
            player.Opaque();
            weapons.Opaque();
        }
    }

    private void End()
    {
        isOnDelay = false;
        switchTimer = 0f;
        timer = 0f;
        Transparent(false);
    }
}
