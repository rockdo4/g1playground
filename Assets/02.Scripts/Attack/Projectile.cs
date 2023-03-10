using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Projectile : AttackCollider
{
    private Rigidbody rb;
    private bool isPenetrable = false;
    private bool isReturnable = false;
    private bool isReturning = false;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    protected override void Update()
    {
        base.Update();
        if (isReturnable && !isReturning && timer > lifeTime * 0.5f)
        {
            isReturning = true;
            attackedList.Clear();
            rb.velocity = -rb.velocity;
            transform.forward = -transform.forward;
        }
    }

    private void FixedUpdate()
    {
        foreach(var effect in effects)
        {
            effect.transform.position = transform.position;
            effect.transform.forward = transform.forward;
        }
    }

    public void Fire(GameObject attacker, Vector3 startPos, Vector3 direction, float distance, float lifeTime, bool isPenetrable, bool isReturnable)
    {
        isReturning = false;
        this.attacker = attacker;
        this.lifeTime = lifeTime;
        this.isPenetrable = isPenetrable;
        this.isReturnable = isReturnable;
        transform.position = startPos;
        transform.forward = direction;
        rb.velocity = distance / lifeTime * transform.forward;
        if (isReturnable)
            rb.velocity *= 2f;

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
    }

    protected override bool OnTriggerStay(Collider other)
    {
        if (!base.OnTriggerStay(other))
            return false;

        var isAttackable = other.CompareTag("Player") || other.CompareTag("Enemy");
        if (!isPenetrable || !isAttackable)
            GameManager.instance.attackColliderManager.Release(this);
        return true;
    }
}
