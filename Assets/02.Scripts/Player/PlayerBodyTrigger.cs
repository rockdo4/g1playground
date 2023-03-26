using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBodyTrigger : MonoBehaviour
{
    [Serializable]
    public struct InvisibleRendering
    {
        public Renderer renderer;
        public Material normalRendering;
        public Material invisibleRendering;

        public void Opaque() => renderer.material = normalRendering;
        public void Transparent() => renderer.material = invisibleRendering;
    }

    public float damagedDelay;
    private float timer;
    private float switchTimer;
    private bool isOnDelay = false;
    private bool transparentMode = false;
    public int damage;
    [SerializeField] public InvisibleRendering player;
    [SerializeField] public InvisibleRendering weapon;

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
        Attack attack = new Attack(damage, newCC);
        var attackables = transform.parent.GetComponents<IAttackable>();
        var attacker = other.transform.parent.gameObject;
        var attackPos = other.transform.position;
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
            weapon.Transparent();
        }
        else
        {
            player.Opaque();
            weapon.Opaque();
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
