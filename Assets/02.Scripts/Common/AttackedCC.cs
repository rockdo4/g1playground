using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static PlayerController;

public class AttackedCC : MonoBehaviour, IAttackable
{
    public struct Debuff
    {
        public float value;
        public float timer;

        public void Update(float deltaTime)
        {
            timer -= deltaTime;
        }
    }

    private Rigidbody rb;
    private Status status;
    private MonoBehaviour controller;

    private GameObject slowDownEffect;
    private GameObject reduceEffect;
    private GameObject stunEffect;

    public bool canKnockBack = true;
    private float up = 1f;
    public float kBResistTime = 1f;
    private float kBResistTimer = 0f;

    private bool onStunned = false;
    private float stunTimer = 0f;

    private LinkedList<Debuff> slowDowns = new LinkedList<Debuff>();
    private bool onSlowDown;
    private float slowDownValue = 0f;

    private LinkedList<Debuff> reduceDefs = new LinkedList<Debuff>();
    private bool onReduceDef;
    private float reduceDefValue = 0f;

    private void OnEnable()
    {
        Reset();
    }

    public void Reset()
    {
        EndKnockBack();

        EndStun();

        slowDowns.Clear();
        EndSlowDown();

        reduceDefs.Clear();
        EndReduceDef();
    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        status = GetComponent<Status>();
        if (CompareTag("Player"))
            controller = GetComponent<PlayerController>();
        if (CompareTag("Enemy"))
            controller = GetComponent<Enemy>();
    }

    private void Update()
    {
        if (!canKnockBack)
        {
            kBResistTimer += Time.deltaTime;
            if (kBResistTimer >= kBResistTime)
                EndKnockBack();
        }

        if (onStunned)
        {
            stunTimer -= Time.deltaTime;
            if (stunTimer <= 0f)
                EndStun();
        }

        if (onSlowDown)
        {
            float newSlowDownValue = 0f;
            var enumerator = slowDowns.GetEnumerator();
            enumerator.MoveNext();
            while (true)
            {
                var currentDebuff = enumerator.Current;
                if (currentDebuff.timer <= 0)
                {
                    if (!enumerator.MoveNext())
                    {
                        slowDowns.Remove(currentDebuff);
                        break;
                    }
                    slowDowns.Remove(currentDebuff);
                    continue;
                }

                if (currentDebuff.value > newSlowDownValue)
                    newSlowDownValue = currentDebuff.value;

                if (!enumerator.MoveNext())
                    break;
            }

            if (!Mathf.Approximately(slowDownValue, newSlowDownValue))
            {
                slowDownValue = newSlowDownValue;

                if (Mathf.Approximately(slowDownValue, 0f))
                    EndSlowDown();
                else
                {
                    //if (CompareTag("Player"))
                    //    ((PlayerController)controller).;
                    //if (CompareTag("Enemy"))
                    //    ((Enemy)controller).SlowDown(1 - slowDown, slowDownTime);
                }
            }
        }

        if (onReduceDef)
        {
            float newReduceDefValue = 0f;
            var enumerator = reduceDefs.GetEnumerator();
            enumerator.MoveNext();
            while (true)
            {
                var currentDebuff = enumerator.Current;
                if (currentDebuff.timer <= 0)
                {
                    if (!enumerator.MoveNext())
                    {
                        reduceDefs.Remove(currentDebuff);
                        break;
                    }
                    reduceDefs.Remove(currentDebuff);
                    continue;
                }

                if (currentDebuff.value > newReduceDefValue)
                    newReduceDefValue = currentDebuff.value;

                if (!enumerator.MoveNext())
                    break;
            }

            if (!Mathf.Approximately(reduceDefValue, newReduceDefValue))
            {
                reduceDefValue = newReduceDefValue;

                if (Mathf.Approximately(reduceDefValue, 0f))
                    EndReduceDef();
                else
                    status.ReduceDef(reduceDefValue);
            }
        }
    }

    public void OnAttack(GameObject attacker, Attack attack, Vector3 attackPos)
    {
        KnockBack(attackPos, attack.Cc.knockBackForce);
        Stun(attack.Cc.stunTime);
        SlowDown(attack.Cc.slowDown, attack.Cc.slowTime);
        ReduceDef(attack.Cc.reduceDef, attack.Cc.reduceDefTime);
    }

    private void KnockBack(Vector3 attackPos, float force)
    {
        if (!canKnockBack || Mathf.Approximately(force, 0f))
            return;
        if (CompareTag("Player"))
            ((PlayerController)controller).SetState<KnockBackState>();
        if (CompareTag("Enemy"))
            ((Enemy)controller).KnockBack();
        var dir = new Vector3(Mathf.Sign(transform.position.x - attackPos.x), 0f, 0f);
        dir.y = up;
        dir.Normalize();
        rb.velocity = Vector3.zero;
        rb.AddForce(dir * force, ForceMode.Impulse);
        canKnockBack = false;
        kBResistTimer = 0f;
    }

    private void EndKnockBack()
    {
        canKnockBack = true;
        kBResistTimer = 0f;
    }

    private void Stun(float stunTime)
    {
        if (stunTimer >= stunTime|| Mathf.Approximately(stunTime, 0f))
            return;
        stunTimer = stunTime;
        onStunned = true;
        //if (CompareTag("Player"))
        //    ((PlayerController)controller).;
        if (CompareTag("Enemy"))
            ((Enemy)controller).Stun(stunTime);

        if (stunEffect == null)
        {
            //stunEffect = GameManager.instance.effectManager.GetEffect("Stun");
            //var y = GetComponent<CapsuleCollider>().bounds.max.y + 1.5f;
            //stunEffect.transform.SetParent(transform);
            //stunEffect.transform.localPosition = new Vector3(0, y,0);
        }
    }

    private void EndStun()
    {
        onStunned = false;
        stunTimer = 0f;

        //if (CompareTag("Player"))
        //    ((PlayerController)controller).;
        //if (CompareTag("Enemy"))
        //((Enemy)controller).Stun(stunTime);

        if (stunEffect != null)
        {
            GameManager.instance.effectManager.ReturnEffect("Stun", stunEffect);
            stunEffect = null;
        }
    }

    private void SlowDown(float newSlowDownValue, float newSlowDownTime)
    {
        if (Mathf.Approximately(newSlowDownValue, 0f))
            return;
        Debuff newSlowDown;
        newSlowDown.value = newSlowDownValue;
        newSlowDown.timer = newSlowDownTime;
        slowDowns.AddLast(newSlowDown);

        foreach (var slowDown in slowDowns)
        {
            if (slowDown.value > slowDownValue)
                slowDownValue = slowDown.value;
        }

        //if (CompareTag("Player"))
        //    ((PlayerController)controller).;
        //if (CompareTag("Enemy"))
        //    ((Enemy)controller).SlowDown(1 - slowDown, slowDownTime);

        onSlowDown = true;

        if (slowDownEffect == null)
        {
            //slowDownEffect = GameManager.instance.effectManager.GetEffect("Fog_speedSlow(blue)");
            //slowDownEffect.transform.position = transform.position;
            //slowDownEffect.transform.SetParent(transform);

        }
    }

    private void EndSlowDown()
    {
        onSlowDown = false;
        slowDownValue = 0f;
        //if (CompareTag("Player"))
        //    ((PlayerController)controller).;
        //if (CompareTag("Enemy"))
        //    ((Enemy)controller).SlowDown(1 - slowDown, slowDownTime);
        if (slowDownEffect != null)
        {
            GameManager.instance.effectManager.ReturnEffect("Fog_speedSlow(blue)", slowDownEffect);
            slowDownEffect = null;
        }
    }

    private void ReduceDef(float newReduceDefValue, float newReduceDefTime)
    {
        if (Mathf.Approximately(newReduceDefValue, 0f))
            return;
        Debuff newReduceDef;
        newReduceDef.value = newReduceDefValue;
        newReduceDef.timer = newReduceDefTime;
        reduceDefs.AddLast(newReduceDef);

        foreach (var reduceDef in reduceDefs)
        {
            if (reduceDef.value > reduceDefValue)
                reduceDefValue = reduceDef.value;
        }

        status.ReduceDef(reduceDefValue);
        onReduceDef = true;

        if (reduceEffect == null)
        {
            reduceEffect = GameManager.instance.effectManager.GetEffect("Fog_speedSlow");
            reduceEffect.transform.position = transform.position;
            reduceEffect.transform.SetParent(transform);
        }
    }

    public void EndReduceDef()
    {
        onReduceDef = false;
        reduceDefValue = 0f;
        status.ReduceDef(reduceDefValue);
        if (reduceEffect != null)
        {
            GameManager.instance.effectManager.ReturnEffect("Fog_speedSlow", reduceEffect);
            reduceEffect = null;
        }
    }
}
