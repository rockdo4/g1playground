using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AttackCollider : MonoBehaviour
{
    protected PlayerSound soundEffect;
    protected GameObject attacker;
    public System.Action<GameObject, GameObject, Vector3> OnCollided;
    protected float lifeTime;
    protected float timer;
    public string[] detachedEffects;
    [NonSerialized] public List<GameObject> effects = new List<GameObject>();
    public string hitEffect;
    public string flashEffect;
    protected string fireSoundEffect;
    protected string inUseSoundEffect;
    protected string hitSoundEffect;
    protected List<GameObject> attackedList = new List<GameObject>();
    protected bool onlyCollideLivings = false;

    public virtual void Reset()
    {
        hitEffect = null;//temp
        OnCollided = null;
        gameObject.SetActive(true);
        timer = 0f;
        attackedList.Clear();
        effects.Clear();
    }

    protected virtual void Update()
    {
        timer += Time.deltaTime;
        if (timer > lifeTime)
        {
            // In Use Sound Effect ���ֱ�
            if (soundEffect.audioSource.name == inUseSoundEffect)
            {
                

            }
            soundEffect.Release();
            GameManager.instance.attackColliderManager.Release(this);
        }
    }

    protected virtual bool OnTriggerStay(Collider other)
    {
        if (!other.gameObject.activeSelf ||
            other.CompareTag("AttackBox") ||
            (other.attachedRigidbody != null && other.attachedRigidbody.CompareTag(attacker.tag)) ||
            attackedList.Contains(other.gameObject))
            return false;

        if (other.CompareTag("Enemy") && !other.GetComponent<Enemy>().GetIsLive())
            return false;

        var isAttackable = other.CompareTag("Player") || other.CompareTag("Enemy");

        if (onlyCollideLivings && !isAttackable)
            return false;

        Vector3 pos = other.ClosestPoint(transform.position);
        Quaternion rot = Quaternion.LookRotation(transform.forward);

        if (OnCollided != null)
        {
            OnCollided(attacker, other.gameObject, pos);
            if (isAttackable)
                attackedList.Add(other.gameObject);
        }
        //Vector3 pos = contact.point + contact.normal;
        if (!string.IsNullOrEmpty(hitEffect))
        {
            var hit = GameManager.instance.effectManager.GetEffect(hitEffect);
            if (hit != null)
            {
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
        }
        if (!string.IsNullOrEmpty(hitSoundEffect))
        {
            //SoundManager.instance.PlaySoundEffect(hitSoundEffect);
            soundEffect = SoundManager.instance.PlayerSoundPool.Get();
            var clip = SoundManager.instance.GetAudioClip(hitSoundEffect);
            soundEffect.InitSound(clip);
        }
        return true;
    }
}
