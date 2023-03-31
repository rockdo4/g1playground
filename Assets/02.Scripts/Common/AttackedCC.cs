using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static PlayerController;

public class AttackedCC : MonoBehaviour, IAttackable
{
    private Rigidbody rb;
    private Status status;
    private MonoBehaviour controller;

    public bool canKnockBack;
    public bool canStun;
    public bool canSlowDown;
    public bool canReduceDef;

    private float up = 1f;
    private bool knockBackedOnThisFrame;
    public int maxKBCount = 3;
    private int kBCount = 0;
    public float kBResistTime = 12f;
    private float kBResistTimer = 0f;

    public int maxStunCount = 3;
    private int stunCount = 0;
    public float stunResistTime = 12f;
    private float stunResistTimer = 0f;

    private bool onSlowDown;
    private float slowDown;
    private float slowDownTime;
    private float slowDownTimer = 0f;

    private bool onReduceDef;
    private float reduceDef;
    private float reduceDefTime;
    private float reduceDefTimer = 0f;

    private void OnEnable()
    {
        Reset();
    }

    public void Reset()
    {
        knockBackedOnThisFrame = false;
        kBCount = 0;
        kBResistTimer = 0f;

        stunCount = 0;
        stunResistTimer = 0f;

        onSlowDown = false;
        slowDownTimer = 0f;

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
        if (kBCount > 0)
        {
            kBResistTimer += Time.deltaTime;
            if (kBResistTimer > kBResistTime)
            {
                kBResistTimer = 0f;
                kBCount = 0;
            }
        }

        if (stunCount > 0)
        {
            stunResistTimer += Time.deltaTime;
            if (stunResistTimer > stunResistTime)
            {
                stunResistTimer = 0f;
                stunCount = 0;
            }
        }

        if (onSlowDown)
        {
            slowDownTimer += Time.deltaTime;
            if (slowDownTimer > slowDownTime)
            {
                onSlowDown = false;
                slowDownTimer = 0f;
                //if (CompareTag("Enemy"))
                //    ((Enemy)controller).EndSlowDown();
            }
        }

        if (onReduceDef)
        {
            reduceDefTimer += Time.deltaTime;
            if (reduceDefTimer > reduceDefTime)
                EndReduceDef();
        }
    }

    private void FixedUpdate()
    {
        knockBackedOnThisFrame = false;
    }

    public void OnAttack(GameObject attacker, Attack attack, Vector3 attackPos)
    {
        if (canKnockBack)
            KnockBack(attackPos, attack.Cc.knockBackForce);
        if (canStun)
            Stun(attack.Cc.stunTime);
        if (canSlowDown)
            SlowDown(attack.Cc.slowDown, attack.Cc.slowTime);
        if (canReduceDef)
            ReduceDef(attack.Cc.reduceDef, attack.Cc.reduceDefTime);
    }

    private void KnockBack(Vector3 attackPos, float force)
    {
        //Debug.Log(kBCount);
        if (kBCount >= maxKBCount || Mathf.Approximately(force, 0f) || knockBackedOnThisFrame)
            return;

        force *= Mathf.Pow(0.5f, kBCount);
        if (CompareTag("Player"))
            ((PlayerController)controller).SetState<KnockBackState>();
        if (CompareTag("Enemy"))
            ((Enemy)controller).KnockBack();
        var dir = new Vector3(Mathf.Sign(transform.position.x - attackPos.x), 0f, 0f);
        dir.y = up;
        dir.Normalize();
        rb.velocity = Vector3.zero;
        rb.AddForce(dir * force, ForceMode.Impulse);
        knockBackedOnThisFrame = true;
        ++kBCount;
        kBResistTimer = 0f;
    }

    private void Stun(float stunTime)
    {
        if (stunCount >= maxStunCount || Mathf.Approximately(stunTime, 0f))
            return;
        stunTime *= Mathf.Pow(0.5f, stunCount);
        //if (CompareTag("Player"))
        //    ((PlayerController)controller).;
        if (CompareTag("Enemy"))
            ((Enemy)controller).Stun(stunTime);
        ++stunCount;
        stunResistTimer = 0f;
    }

    private void SlowDown(float newSlowDown, float newSlowTime)
    {
        if (Mathf.Approximately(slowDown, newSlowDown))
            slowDownTime = slowDownTime > newSlowTime ? slowDownTime : newSlowTime;
        else if (newSlowDown > slowDown)
        {
            slowDown = newSlowDown;
            slowDownTime = newSlowTime;
        }
        else
            return;
     
        //if (CompareTag("Player"))
        //    ((PlayerController)controller).;
        //if (CompareTag("Enemy"))
        //    ((Enemy)controller).SlowDown(1 - slowDown, slowDownTime);
        onSlowDown = true;

        //GameObject effect = GameManager.instance.effectManager.GetEffect("Fog_speedSlow(blue)");
        //effect.transform.position = transform.position;
        //GameManager.instance.effectManager.ReturnEffectOnTime("Fog_speedSlow(blue)", effect, newSlowTime);
        //effect.transform.SetParent(transform);                                                                                                                                                                                                                                 
    }

    private void ReduceDef(float newReduceDef, float newReduceDefTime)
    {
        if (Mathf.Approximately(reduceDef, newReduceDef))
            reduceDefTime = reduceDefTime > newReduceDefTime ? reduceDefTime : newReduceDefTime;
        else if (newReduceDef > reduceDef)
        {
            reduceDef = newReduceDef;
            reduceDefTime = newReduceDefTime;
        }
        else
            return;
        status.ReduceDef(reduceDef);
        onReduceDef = true;

        //GameObject effect = GameManager.instance.effectManager.GetEffect("Fog_speedSlow");
        //effect.transform.position = transform.position;
        //GameManager.instance.effectManager.ReturnEffectOnTime("Fog_speedSlow", effect, newReduceDefTime);
        //effect.transform.SetParent(transform);
    }

    public void EndReduceDef()
    {
        onReduceDef = false;
        reduceDefTimer = 0f;
        status.ReduceDef(0f);

        // effect release
    }
}
