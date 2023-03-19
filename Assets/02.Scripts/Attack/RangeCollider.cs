using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeCollider : AttackCollider
{
    protected Transform attackPivot;
    public bool isContinuousAttack = false;
    protected float attackInterval = 0f;
    protected float attackTimer = 0f;
    protected bool useLifetime = true;

    private void Start()
    {
        onlyCollideLivings = true;
    }

    public override void Reset()
    {
        base.Reset();
        attackPivot = null;
        isContinuousAttack = false;
        attackTimer = 0f;
    }

    protected override void Update()
    {
        if (useLifetime)
            base.Update();
        if (isContinuousAttack)
        {
            attackTimer += Time.deltaTime;
            if (attackTimer > attackInterval)
            {
                attackTimer = 0f;
                attackedList.Clear();
            }
        }
    }

    private void FixedUpdate()
    {
        if (attackPivot != null)
        {
            transform.position = attackPivot.position;
            transform.forward = attackPivot.forward;
        }
    }

    public void Fire(GameObject attacker, Transform attackPivot, bool useLifeTime, float lifeTime, bool isContinuousAttack, float interval, string fireSoundEffect, string inUseSoundEffect, string hitSoundEffect)
    {
        this.attackPivot = attackPivot;
        Fire(attacker, attackPivot.position, attackPivot.forward, useLifeTime, lifeTime, isContinuousAttack, interval, fireSoundEffect, inUseSoundEffect, hitSoundEffect);
    }

    public void Fire(GameObject attacker, Vector3 startPos, Vector3 direction, bool useLifeTime, float lifeTime, bool isContinuousAttack, float interval, string fireSoundEffect, string inUseSoundEffect, string hitSoundEffect)
    {
        this.attacker = attacker;
        this.useLifetime = useLifeTime;
        this.lifeTime = lifeTime;
        this.isContinuousAttack = isContinuousAttack;
        this.attackInterval = interval;
        this.fireSoundEffect = fireSoundEffect;
        this.inUseSoundEffect = inUseSoundEffect;
        this.hitSoundEffect = hitSoundEffect;
        transform.position = startPos;
        transform.forward = direction;

        foreach (var d in detachedEffects)
        {
            effects.Add(GameManager.instance.effectManager.GetEffect(d));
        }

        if (!string.IsNullOrEmpty(flashEffect))
        {
            var flash = GameManager.instance.effectManager.GetEffect(flashEffect);
            flash.transform.position = transform.position;
            flash.transform.forward = gameObject.transform.forward;
            var flashPs = flash.GetComponent<ParticleSystem>();
            if (flashPs != null)
            {
                GameManager.instance.effectManager.ReturnEffectOnTime(flashEffect, flash, flashPs.main.duration);
            }
            else
            {
                var flashPsParts = flash.transform.GetChild(0).GetComponent<ParticleSystem>();
                GameManager.instance.effectManager.ReturnEffectOnTime(flashEffect, flash, flashPsParts.main.duration);
            }
        }
        if (!string.IsNullOrEmpty(fireSoundEffect))
            SoundManager.instance.PlaySoundEffect(fireSoundEffect);
        if (!string.IsNullOrEmpty(inUseSoundEffect))
            SoundManager.instance.PlaySoundEffect(inUseSoundEffect);
    }
}
