using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BossController : MonoBehaviour
{
    public enum BossState
    {
        None,
        Spawn,
        Idle,
        Patrol,
        Chase,
        Attack,
        Skill,
        TakeDamage,
        Groggy,
        Die,
    }
    private Animator animator;
    private Rigidbody rb;
    private NavMeshAgent agent;
    private CapsuleCollider collider;

    public virtual BossState State
    {
        get ;
        protected set;
    }

    private BossController boss;
    private void Awake()
    {
        //animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        agent = GetComponent<NavMeshAgent>();
        collider = GetComponent<CapsuleCollider>();
        boss = GetComponent<BossController>();
    }
    void Start()
    {

    }

    private void Update()
    {

        switch(boss.State)
        {

        }

        boss.Update();
    }

    protected virtual void Spawn()
    {

    }
    protected virtual void Idle()
    {

    }

    protected virtual void Patrol()
    {

    }

    protected virtual void Chase()
    {

    }

    protected virtual void Attack()
    {

    }
    protected virtual void Skill()
    {
    }
    protected virtual void TakeDamage()
    {

    }
    protected virtual void Groggy()
    {

    }
    protected virtual void Die()
    {

    }
}
