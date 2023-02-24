using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class Projectile : MonoBehaviour
{
    private Rigidbody rb;
    private GameObject attacker;
    public System.Action<GameObject, GameObject> OnCollided;
    private Vector3 startPos;
    private float distance;
    public string[] detachedEffects;
    public List<GameObject> effects = new List<GameObject>();
    public string hitEffect;
    public string flashEffect;
    private IObjectPool<Projectile> pool;

    public void SetPool(IObjectPool<Projectile> pool) => this.pool = pool;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        foreach (var d in detachedEffects)
        {
            effects.Add(GameManager.instance.effectManager.GetEffect(d));
        }
    }

    private void FixedUpdate()
    {
        foreach(var effect in effects)
        {
            effect.transform.position = transform.position;
            effect.transform.forward = transform.forward;
        }
        if (Vector3.Distance(transform.position, startPos) > distance)
            pool.Release(this);
    }

    public void Fire(GameObject attacker, Vector3 startPos, Vector3 direction, float distance, float speed)
    {
        this.attacker = attacker;
        this.startPos = startPos;
        this.distance = distance;
        transform.position = startPos;
        transform.forward = direction;
        rb.velocity = transform.forward * speed;

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
        if (other.CompareTag(attacker.tag))
            return;
        if (OnCollided != null)
            OnCollided(attacker, other.gameObject);

        Vector3 pos = other.ClosestPoint(transform.position);
        Quaternion rot = Quaternion.LookRotation(transform.forward);
        //Vector3 pos = contact.point + contact.normal;
        rb.velocity = Vector3.zero;
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
        pool.Release(this);
    }
}
