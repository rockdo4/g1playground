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
                case EnemyState.Idle:
                    rb.isKinematic = true;
                    agent.velocity = Vector3.zero;
                    agent.enabled = true;
                    agent.isStopped = true;
                    break;
                case EnemyState.Patrol:
                    agent.enabled = true;
                    agent.isStopped = false;
                    agent.speed = patrolSpeed;
                    rb.isKinematic = true;
                    break;
                case EnemyState.Return:
                    agent.enabled = true;
                    agent.isStopped = false;
                    agent.speed = chaseSpeed;
                    rb.isKinematic = true;
                    break;
                case EnemyState.Chase:
                    agent.enabled = true;
                    agent.isStopped = false;
                    agent.speed = chaseSpeed;
                    rb.isKinematic = true;
                    break;
                case EnemyState.Groggy:
                    agent.enabled = true;
                    agent.isStopped = false;
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

        if (state != EnemyState.None)
        {
            projectileCoolTime += Time.deltaTime;
            areaCoolTime += Time.deltaTime;
        }

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
        if (LookAtPos(mySpawnPos))
            agent.SetDestination(mySpawnPos);

        if (Vector3.Distance(transform.position, mySpawnPos) <= 0.5)
        {
            State = EnemyState.None;
            returnCoolTime = 0f;
        }
    }

    protected override void ChaseUpdate()
    {
        if (LookAtTarget())
            agent.SetDestination(player.transform.position);

        BattleProcess();
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

    void BattleProcess()
    {
        if (areaCoolTime >= areaTime)
        {
            areaCoolTime = 0f;
            animator.SetTrigger("Area");
            return;
        }


        if (projectileCoolTime >= projectileTime)
        {
            projectileCoolTime = 0f;

            animator.SetTrigger("Area");
            return;
        }

        if (Vector3.Distance(transform.position, player.transform.position) <= attackRange)
        {
            animator.SetTrigger("Attack");
            State = EnemyState.Attack;
            isHit = true;
            return;
        }
    }


    private void Attack()
    {
            attackBox.SetActive(true);
    }

    private void AttackDone()
    {
        if (!isHit)
        {
            State = EnemyState.Chase;
        }

    }

    private void LastAttackDone()
    {
        animator.SetBool("isAttack", false);

        State = EnemyState.Chase;
        isHit = true;
    }

    private bool isHit = true;
    private void OnTriggerEnter(Collider collider)
    {
        if (!attackBox.activeSelf)
            return;

        if (collider.gameObject.CompareTag("Player") && collider.gameObject.GetComponent<ObjectMass>() != null)
        {
#if UNITY_EDITOR
            Debug.Log(State);
#endif
            isHit = true;
            meleeAttack.ExecuteAttack(gameObject, player.gameObject, transform.position);
            attackBox.SetActive(false);
            animator.SetBool("isAttack", isHit);
        }
        else
        {
            isHit = false;
            animator.SetBool("isAttack", isHit);
        }
    }

}
