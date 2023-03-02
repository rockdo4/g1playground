using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private Rigidbody rb;
    private GameObject attacker;
    public System.Action<GameObject, GameObject, Vector3> OnCollided;
    private float lifeTime;
    private float timer;
    public string[] detachedEffects;
    public List<GameObject> effects = new List<GameObject>();
    public string hitEffect;
    public string flashEffect;
    private bool isPenetrable = false;
    private bool isReturnable = false;
    private bool isReturning = false;
    private List<GameObject> attackedList = new List<GameObject>();

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        timer += Time.deltaTime;
        if (isReturnable && !isReturning && timer > lifeTime * 0.5f)
        {
            isReturning = true;
            attackedList.Clear();
            rb.velocity = -rb.velocity;
            transform.forward = -transform.forward;
        }
        if (timer > lifeTime)
            GameManager.instance.projectileManager.Release(this);
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
        timer = 0f;
        isReturning = false;
        attackedList.Clear();
        effects.Clear();
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

        if (flashEffect != null)
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

    private void OnTriggerEnter(Collider other)
    {
        if (!other.gameObject.activeSelf ||
            other.CompareTag("AttackBox") ||
            (other.attachedRigidbody != null && other.attachedRigidbody.CompareTag(attacker.tag)) ||
            attackedList.Contains(other.gameObject))
            return;
        var isAttackable = other.CompareTag("Player") || other.CompareTag("Enemy");

        Vector3 pos = other.ClosestPoint(transform.position);
        Quaternion rot = Quaternion.LookRotation(transform.forward);

        if (OnCollided != null)
        {
            OnCollided(attacker, other.gameObject, pos);
            if (isAttackable)
                attackedList.Add(other.gameObject);
        }
        //Vector3 pos = contact.point + contact.normal;
        if (hitEffect != null)
        {
            var hit = GameManager.instance.effectManager.GetEffect(hitEffect);
            hit.transform.position = pos;
            hit.transform.rotation = rot;
            //hit.transform.LookAt(contact.point + contact.normal);

            var hitPs = hit.GetComponent<ParticleSystem>();
            if (hitPs != null)
            {
                GameManager.instance.effectManager.ReturnEffectOnTime(hitEffect, hit, hitPs.main.duration);
            }
            else
            {
                var hitPsParts = hit.transform.GetChild(0).GetComponent<ParticleSystem>();
                GameManager.instance.effectManager.ReturnEffectOnTime(hitEffect, hit, hitPsParts.main.duration);
            }
        }
        if (!isPenetrable || !isAttackable)
            GameManager.instance.projectileManager.Release(this);
    }
}
