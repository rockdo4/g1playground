using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using static EnemyController;

public class Boss4GreenFruit : BossController
{
    private BossState state;
    public GameObject mask;

    public float attackRange;
    public float idleTime;


    public override BossState State
    {
        get { return state; }
        protected set
        {
            var prevState = state;
            state = value;

            if (prevState == state)
                return;

            switch (State)
            {
                case BossState.None:
                    break;
                case BossState.Spawn:
                    break;
                case BossState.Idle:
                    agent.velocity = Vector3.zero;
                    agent.isStopped = true;
                    break;
                case BossState.Chase:
                    agent.isStopped = false;
                    break;
                case BossState.TakeDamage:
                    break;
                case BossState.Attack:
                    agent.velocity = Vector3.zero;
                    agent.isStopped = true;
                    break;
                case BossState.Skill:
                    break;
                case BossState.Die:
                    animator.SetTrigger("Die");
                    break;
            }

        }
    }

    protected override void Awake()
    {
        base.Awake();
        State = BossState.None;
    }

    protected override void Start()
    {
        base.Start();
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            State = BossState.Spawn;
        }
        if (Input.GetKeyDown(KeyCode.F))
        {
            State = BossState.Die;
        }
        switch (State)
        {

            case BossState.Spawn:
                Spawn();
                break;
            case BossState.Idle:
                Idle();
                break;
            case BossState.Patrol:
                Patrol();
                break;
            case BossState.Chase:
                Chase();
                break;
            case BossState.Attack:
                Attack();
                break;
            case BossState.Skill:
                Skill();
                break;
            case BossState.TakeDamage:
                TakeDamage();
                break;
            case BossState.Die:
                Die();
                break;
        }
        animator.SetFloat("Move", agent.velocity.magnitude);
        Debug.Log(State);
    }
    protected override void Spawn()
    {
        animator.SetTrigger("Spawn");
    }

    private float idleCool = 0f;
    protected override void Idle()
    {

        idleCool += Time.deltaTime;

        if (LookAtTarget())
            if (idleCool >= idleTime)
            {
                idleCool = 0f;
                State = BossState.Chase;
            }
    }

    protected override void Chase()
    {
        if (LookAtTarget())
            agent.SetDestination(player.transform.position);

        if (Vector3.Distance(transform.position, player.transform.position) <= attackRange)
        {
            if (!LookAtTarget())
                return;

            State = BossState.Attack;
            animator.SetTrigger("Attack");
            return;
        }
    }

    protected override void Attack()
    {
        LookAtTarget();
    }

    protected override void Skill()
    {

    }

    protected override void TakeDamage()
    {

    }

    protected override void Die()
    {

    }

    private void SpawnDone()
    {
        mask.active = false;
        State = BossState.Chase;
    }

    private void DieDone()
    {
        gameObject.active = false;
    }

    private void AttackDone()
    {
        State = BossState.Idle;
    }
}