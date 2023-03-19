using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "RotateAttacker", menuName = "SkillAttack/RotateAttacker")]
public class RotateAttacker : SkillAttack
{
    private GameObject attacker;
    private Status aStat;
    private Transform attackPivot;
    public string rangeColliderId;
    public int maxCount = 5;
    public float cycle = 1f;
    private float timer = 0f;
    public float reqManaInterval = 1f;
    private float reqManaTimer = 0f;
    private bool isAttacking = false;
    private List<RangeCollider> rangeColliders = new List<RangeCollider>();
    public bool isContinuousAttack = false;
    public float interval;

    public void Rotate(GameObject attacker, Transform attackPivot)
    {
        isAttacking = true;
        this.attacker = attacker;
        this.attackPivot = attackPivot;
        timer = 0f;
        reqManaTimer = 0f;
        aStat = attacker.GetComponent<Status>();
        if (aStat == null || aStat.CurrMp < reqMana)
            return;
        aStat.CurrMp -= reqMana;
        if (attacker.CompareTag("Player"))
            aStat.SetMpUi();
        rangeColliders.Clear();
        GetCollider();
    }

    public override void Update()
    {
        if (!isAttacking)
            return;
        reqManaTimer += Time.deltaTime;
        if (reqManaTimer > reqManaInterval)
        {
            reqManaTimer = 0f;
            if (aStat.CurrMp < reqMana)
            {
                ReleaseCollider();
                return;
            }
            if (rangeColliders.Count == 0)
                GetCollider();
            aStat.CurrMp -= reqMana;
            if (attacker.CompareTag("Player"))
                aStat.SetMpUi();
        }
        else if (rangeColliders.Count == 0)
            return;

        timer += Time.deltaTime;
        if (timer > cycle)
            timer = 0f;
        var angle = 2 * Mathf.PI * timer / cycle;
        var add = 2 * Mathf.PI / maxCount;
        for (int i = 0; i < maxCount; ++i)
        {
            var newAngle = angle + add * i;
            if (attacker.transform.forward.x < 0f)
                newAngle = -newAngle;
            var pos = rangeColliders[i].transform.position;
            var newPos = attackPivot.position + new Vector3(Mathf.Sin(newAngle), Mathf.Cos(newAngle), 0f) * range;
            rangeColliders[i].transform.position = newPos;
            if (attacker.transform.forward.x < 0f)
                rangeColliders[i].transform.forward = new Vector3(-Mathf.Cos(newAngle), Mathf.Sin(newAngle), 0f);
            else
                rangeColliders[i].transform.forward = new Vector3(Mathf.Cos(newAngle), -Mathf.Sin(newAngle), 0f);
        }
    }

    public void Stop()
    {
        isAttacking = false;
        ReleaseCollider();
    }

    private void GetCollider()
    {
        for (int i = 0; i < maxCount; ++i)
        {
            var rangeCollider = GameManager.instance.attackColliderManager.Get<RangeCollider>(rangeColliderId);
            rangeCollider.OnCollided = ExecuteAttack;
            rangeCollider.Fire(attacker, attackPivot.position, attackPivot.forward, false, lifeTime, isContinuousAttack, interval);
            rangeColliders.Add(rangeCollider);
        }
    }

    private void ReleaseCollider()
    {
        if (rangeColliders.Count > 0)
        {
            foreach (var collider in rangeColliders)
            {
                GameManager.instance.attackColliderManager.Release(collider);
            }
            rangeColliders.Clear();
        }
    }
}
