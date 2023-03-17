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

    private int mode;
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
        if (mode < 1)
            return;
        if (isOnDelay || !other.CompareTag("EnemyBody"))
            return;
        Attack attack;
        if (mode > 1)
            attack = new Attack(damage);
        else
            attack = new Attack(damage, false);
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

    public void SetMode(int mode)
    {
        this.mode = mode;
        End();
    }

    private void End()
    {
        isOnDelay = false;
        switchTimer = 0f;
        timer = 0f;
        Transparent(false);
    }
}
