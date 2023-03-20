using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossScorpionKing : Enemy
{
    private CapsuleCollider mainColl;
    public GameObject attackBox;
    public GameObject skillPivot;


    public BasicAttack meleeAttack;
    public SkillAttack projectileSkill;
    public SkillAttack FallingAreaSkill;

    public bool isGoingRight;

    public float patrolSpeed;
    public float chaseSpeed;
    public float searchRange;
    public float attackRange;
    public float projectileTime;
    private float projectileCoolTime;
    public float areaTime;
    private float areaCoolTime;
    public float returnTime;
    private float returnCoolTime;
    public override EnemyState State
    {
        get { return state; }
        protected set
        {
            var prevState = state;
            state = value;
            if (EnemyState.Die == prevState)
                return;

            if (prevState == state)
                return;

            switch (State)
            {
                case EnemyState.None:
                    rb.isKinematic = true;
                    agent.enabled = true;
                    break;
                case EnemyState.Spawn:
                    rb.isKinematic = true;
                    agent.enabled = true;
                    break;
                case EnemyState.Motion:
                    agent.enabled = true;
                    agent.velocity = Vector3.zero;
                    agent.isStopped = true;
                    rb.isKinematic = true;
                    break;
                case EnemyState.Idle:
                    rb.isKinematic = true;
                    agent.velocity = Vector3.zero;
                    agent.enabled = true;
                    agent.isStopped = true;
                    break;
                case EnemyState.Return:
                    agent.enabled = true;
                    agent.isStopped = false;
                    rb.isKinematic = true;
                    break;
                case EnemyState.Chase:
                    agent.enabled = true;
                    agent.isStopped = false;
                    rb.isKinematic = true;
                    break;
                case EnemyState.TakeDamage:
                    agent.enabled = true;
                    rb.isKinematic = true;
                    break;
                case EnemyState.Attack:
                    agent.isStopped = true;
                    rb.isKinematic = false;
                    agent.enabled = false;
                    break;
                case EnemyState.Skill:
                    agent.isStopped = true;
                    agent.velocity = Vector3.zero;
                    rb.isKinematic = false;
                    agent.enabled = false;
                    break;
                case EnemyState.Die:
                    agent.velocity = Vector3.zero;
                    agent.enabled = true;
                    rb.isKinematic = true;
                    mainColl.enabled = false;
                    break;
            }
        }
    }

    public bool preGoingRight;
    protected override void Awake()
    {
        base.Awake();
        mainColl = GetComponent<CapsuleCollider>();
        attackBox = GameObject.Find(gameObject.name + "/AttackBox");
        attackBox.SetActive(false);
        State = EnemyState.None;
    }
    protected override void Start()
    {
        base.Start();
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        State = EnemyState.None;
    }

    protected void Update()
    {
        //attackTime += Time.deltaTime;

        switch (State)
        {
            case EnemyState.None:
                None();
                break;
            case EnemyState.Idle:
                IdleUpdate();
                break;
            case EnemyState.Patrol:
                PatrolUpdate();
                break;
            case EnemyState.Return:
                ReturnUpdate();
                break;
            case EnemyState.Chase:
                ChaseUpdate();
                break;
            case EnemyState.Attack:
                AttackUpdate();
                break;
            case EnemyState.Die:
                break;
        }

        animator.SetFloat("Move", agent.velocity.magnitude / chaseSpeed);
        Debug.Log(State);
    }

    private void None()
    {
        State = EnemyState.Patrol;
        SaveFloorLength(ref startPos, ref endPos);
    }

    protected override void IdleUpdate()
    {

    }
    protected override void PatrolUpdate()
    {
        returnCoolTime += Time.deltaTime;
        if (returnTime < returnCoolTime)
        {
            State = EnemyState.Return;
        }

        RayShooter(searchRange, isGoingRight);

        if (isGoingRight)
        {
            if (Vector3.Distance(transform.position, endPos) < 0.1f)
            {
                agent.velocity = Vector3.zero;

                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(Vector3.left), Time.deltaTime * 10f);
                if (Quaternion.Angle(transform.rotation, Quaternion.LookRotation(startPos - transform.position).normalized) <= 5f)
                    isGoingRight = false;

                return;
            }
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(endPos - transform.position).normalized, Time.deltaTime * 10f);
            agent.SetDestination(endPos);
        }
        else
        {
            if (Vector3.Distance(transform.position, startPos) < 0.1f)
            {
                agent.velocity = Vector3.zero;

                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(Vector3.right).normalized, Time.deltaTime * 10f);
                if (Quaternion.Angle(transform.rotation, Quaternion.LookRotation(endPos - transform.position).normalized) <= 5f)
                    isGoingRight = true;

                return;
            }
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(startPos - transform.position).normalized, Time.deltaTime * 10f);
            agent.SetDestination(startPos);
        }

    }
    private void ReturnUpdate()
    {
        agent.SetDestination(mySpawnPos);
        if (Vector3.Distance(transform.position, mySpawnPos) <= 0.1)
        {
            State = EnemyState.None;
            returnCoolTime = 0f;
        }
    }

    protected override void ChaseUpdate()
    {
        if (LookAtTarget())
            agent.SetDestination(player.transform.position);

        if (Vector3.Distance(transform.position, player.transform.position) <= attackRange)
        {

        }
    }

    protected override void AttackUpdate()
    {
        LookAtTarget();
    }

    protected override void Skill()
    {

    }

    protected Vector3 startPos;
    protected Vector3 endPos;


}
