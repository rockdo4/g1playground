using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class BossPupleRich : Enemy
{
    private CapsuleCollider mainColl;
    public GameObject attackBox;
    public GameObject skillPivot;


    public BasicAttack meleeAttack;
    public SkillAttack projectileSkill;

    public bool isGoingRight;

    public float patrolSpeed;
    public float chaseSpeed;
    public float searchRange;
    public float attackRange;
    public float projectileTime;
    private float projectileCoolTime;
    public float areaTime;
    private float areaCoolTime;
    public float spawnTime;
    private float spawnCoolTime;
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
                case EnemyState.Patrol:
                    agent.enabled = true;
                    agent.isStopped = false;
                    agent.speed = patrolSpeed;
                    rb.isKinematic = true;
                    break;
                case EnemyState.Return:
                    agent.velocity = Vector3.zero;
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
                    agent.velocity = Vector3.zero;
                    agent.isStopped = true;
                    rb.isKinematic = true;
                    break;
                case EnemyState.Attack:
                    agent.enabled = true;
                    agent.velocity = Vector3.zero;
                    agent.isStopped = true;
                    rb.isKinematic = false;
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
        player = GameManager.instance.player;
        GetComponent<DestructedEvent>().OnDestroyEvent = () =>
        {
            State = EnemyState.Die;
            animator.SetTrigger("Die");
            isLive = false;

            enemyBody.SetActive(false);

        };
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        State = EnemyState.None;
        projectileCoolTime = 0f;
        areaCoolTime = 0f;
    }

    public float groggy1;
    public float groggy2;
    public float groggy3;
    private bool isGroggy1;
    private bool isGroggy2;
    private bool isGroggy3;
    private void CheackGroggy()
    {
        if (isGroggy1 && isGroggy2 && isGroggy3)
            return;

        if (!isGroggy1 && status.CurrHp <= status.FinalValue.maxHp * groggy1)
        {
            isGroggy1 = true;
            animator.SetTrigger("Groggy");
            State = EnemyState.Groggy;
            return;
        }
        else if (!isGroggy2 && status.CurrHp <= status.FinalValue.maxHp * groggy2)
        {
            isGroggy2 = true;
            animator.SetTrigger("Groggy");
            State = EnemyState.Groggy;
            return;
        }
        else if (!isGroggy3 && status.CurrHp <= status.FinalValue.maxHp * groggy3)
        {
            isGroggy3 = true;
            animator.SetTrigger("Groggy");
            State = EnemyState.Groggy;
            return;
        }
    }
    protected void Update()
    {
        //attackTime += Time.deltaTime;

        if (state != EnemyState.None)
        {
            projectileCoolTime += Time.deltaTime;
            areaCoolTime += Time.deltaTime;
        }
        CheackGroggy();

        switch (State)
        {
            case EnemyState.None:
                NoneUpdate();
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
            case EnemyState.Skill:
                SkillUpdate();
                break;
            case EnemyState.Die:
                break;
        }

        animator.SetFloat("Move", agent.velocity.magnitude / chaseSpeed);
        Debug.Log(State);

        if (Input.GetKeyDown(KeyCode.F))
        {
            status.CurrHp -= 1000;
        }
    }

    private void NoneUpdate()
    {
        State = EnemyState.Patrol;
        SaveFloorLength(ref startPos, ref endPos);
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
    protected  void SkillUpdate()
    {
        LookAtTarget();
    }

    protected Vector3 startPos;
    protected Vector3 endPos;

    private int attackCount;
    void BattleProcess()
    {
        if (areaCoolTime >= areaTime)
        {
            areaCoolTime = 0f;
            animator.SetTrigger("Area");
            return;
        }


        if (projectileCoolTime >= projectileTime && Vector3.Distance(transform.position, player.transform.position) >= attackRange)
        {
            projectileCoolTime = 0f;
            State = EnemyState.Skill;
            animator.SetTrigger("Projectile");
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
        ++attackCount;
    }

    private void AttackDone()
    {
        attackBox.SetActive(false);

        if (attackCount >= 3)
            State = EnemyState.Chase;
    }

    private bool isHit = false;
    private void OnTriggerEnter(Collider collider)
    {
        if (!attackBox.activeSelf)
            return;

        if (collider.gameObject.CompareTag("Player") && collider.gameObject.GetComponent<ObjectMass>() != null)
        {
#if UNITY_EDITOR
            Debug.Log(State);
#endif
            meleeAttack.ExecuteAttack(gameObject, player.gameObject, transform.position);
            attackBox.SetActive(false);
        }
    }

    private void Projectile()
    {
        ((EnemyStraightSpell)projectileSkill).Fire(gameObject, skillPivot.transform.position, transform.forward);
        //var playerDir = (player.transform.position - skillPivot.transform.position).normalized;
        Debug.Log("shot");
        //((EnemyStraightSpell)projectileSkill).Fire(gameObject, skillPivot.transform.position, playerDir);
    }
    private void ProjectileDone()
    {
        State = EnemyState.Chase;
    }

    private void Area()
    {
        ((EnemyStraightSpell)projectileSkill).Fire(gameObject, skillPivot.transform.position, Vector3.down);
    }
    private void AreaDone()
    {
        State = EnemyState.Chase;
    }
    private void GroggyDone()
    {
        State = EnemyState.Chase;
    }
    private void DieDone()
    {
        gameObject.SetActive(false);
    }
}
