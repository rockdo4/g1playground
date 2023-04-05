using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyControllerAR : EnemyController, IAttackable
{
    public SkillAttack projectileSkill;
    public GameObject skillPivot;
    protected override void Attack()
    {
        switch (projectileSkill)
        {
            case SkillAttack:
                {
                    enemySound.PlayAttackSound();
                    ((EnemyStraightSpell)projectileSkill).Fire(gameObject, skillPivot.transform.position, transform.forward);
                }
                break;
        }
    }
    protected override void Awake()
    {
        enemySound = GetComponent<EnemySoundPlayer>();
        status = GetComponent<Status>();
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        agent = GetComponent<NavMeshAgent>();
        enemyBody = GameObject.Find(gameObject.name + "/EnemyBody");
        mySpawnPos = transform.position;
        mySpawnDir = transform.rotation;
        mainColl = GetComponent<CapsuleCollider>();
        preGoingRight = isGoingRight;
        linkMover = GetComponent<AgentLinkMover>();
        onGround = GetComponentInChildren<OnGround>();
        skillPivot = GameObject.Find(gameObject.name + "/AttackPivot");
    }

    public void OnAttack(GameObject attacker, Attack attack, Vector3 attackPos)
    {
        if (State == EnemyState.Die)
            return;

        State = EnemyState.TakeDamage;
        animator.SetTrigger("TakeDamage");
    }

    protected virtual void AttackDone()
    {
        State = EnemyState.Chase;
    }

    protected override void OnTriggerEnter(Collider collider)
    {
    }
    //protected override void Start()
    //{
    //    base.Start();
    //}

    //protected override void OnEnable()
    //{
    //    base.OnEnable();
    //}
    //protected virtual void RemoveAgentLinkMover()
    //{
    //    base.RemoveAgentLinkMover();
    //}

    //protected virtual void AddAgentLinkMover()
    //{
    //    base.AddAgentLinkMover();
    //}

    protected virtual void Update()
    {
        base.Update();
    }
    //    protected virtual void None()
    //    {
    //ba
    //    }
    //    protected virtual void IdleUpdate()
    //    {
    //        ChangePattern();
    //        RayShooter(searchRange, isGoingRight);
    //    }
    //    protected virtual void PatrolUpdate()
    //    {
    //        ChangePattern();
    //        RayShooter(searchRange, isGoingRight);

    //        if (isGoingRight)
    //        {
    //            if (Vector3.Distance(transform.position, endPos) < 0.1f)
    //            {
    //                agent.velocity = Vector3.zero;

    //                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(Vector3.left), Time.deltaTime * 10f);
    //                if (Quaternion.Angle(transform.rotation, Quaternion.LookRotation(startPos - transform.position).normalized) <= 5f)
    //                    isGoingRight = false;

    //                return;
    //            }
    //            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(endPos - transform.position).normalized, Time.deltaTime * 10f);
    //            agent.SetDestination(endPos);
    //        }
    //        else
    //        {
    //            if (Vector3.Distance(transform.position, startPos) < 0.1f)
    //            {
    //                agent.velocity = Vector3.zero;

    //                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(Vector3.right).normalized, Time.deltaTime * 10f);
    //                if (Quaternion.Angle(transform.rotation, Quaternion.LookRotation(endPos - transform.position).normalized) <= 5f)
    //                    isGoingRight = true;

    //                return;
    //            }
    //            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(startPos - transform.position).normalized, Time.deltaTime * 10f);
    //            agent.SetDestination(startPos);
    //        }
    //    }

    //    private bool currGoingRight;
    //    private float findingpathtime;

    //    protected virtual void ChaseUpdate()
    //    {
    //        NavMeshPath navMeshPath = new NavMeshPath();
    //        var areaMask = NavMesh.GetAreaFromName("Walkable") | NavMesh.GetAreaFromName("Not Walkable");
    //        if (NavMesh.CalculatePath(transform.position, player.transform.position, areaMask, navMeshPath)
    //            && navMeshPath.status == NavMeshPathStatus.PathComplete)
    //        {
    //            Vector3 targetDirection = player.transform.position - transform.position;
    //            targetDirection.y = 0;
    //            targetDirection.Normalize();
    //            if (player.transform.position.x - transform.position.x != 0f)
    //            {
    //                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(targetDirection), Time.deltaTime * 10f);
    //                findingpathtime = 0;

    //                if (player.transform.position.x - transform.position.x > 0)
    //                    isGoingRight = true;
    //                else
    //                    isGoingRight = false;

    //                if (currGoingRight != isGoingRight)
    //                {
    //                    agent.velocity = Vector3.zero;
    //                    currGoingRight = isGoingRight;
    //                }

    //                if (AngleIgnoringHeight(10f))
    //                {
    //                    agent.isStopped = false;
    //                    agent.SetDestination(player.transform.position);
    //                }
    //                else
    //                {
    //                    agent.isStopped = true;
    //                }

    //                if (RayShooter(attackRange, isGoingRight))
    //                {
    //                    if (attackTime >= attackCool)
    //                        State = EnemyState.Attack;
    //                    else
    //                    {
    //                        agent.isStopped = true;
    //                        agent.velocity = Vector3.zero;
    //                    }
    //                }
    //                else
    //                    agent.isStopped = false;
    //            }
    //        }
    //        else
    //        {

    //            findingpathtime += Time.deltaTime;
    //            if (findingpathtime >= 1.5f)
    //            {
    //                State = EnemyState.None;
    //                ResetPattern();
    //            }
    //        }


    //    }

    //    protected virtual void AttackUpdate()
    //    {
    //        if (attackTime >= attackCool)
    //        {
    //            animator.SetTrigger("Attack");
    //            attackTime = 0f;
    //            return;
    //        }
    //    }

    //    protected virtual void TakeDamageUpdate()
    //    {

    //    }
    //    protected virtual void KnockBackUpdate()
    //    {
    //        if (onGround.isGround && !isKbAnimation)
    //        {
    //            State = EnemyState.Chase;
    //        }
    //    }

    //    protected virtual void StunUpdate()
    //    {
    //        if (stunCoolTime >= stunCool)
    //        {
    //            stunCool = 0f;
    //            stunCoolTime = 0f;
    //            isStun = false;
    //            State = EnemyState.Chase;
    //        }
    //    }

    //    //public void OnAttack(GameObject attacker, Attack attack, Vector3 attackPos)
    //    //{
    //    //    if (State == EnemyState.Die)
    //    //        return;

    //    //    State = EnemyState.TakeDamage;
    //    //    animator.SetTrigger("TakeDamage");
    //    //}
    //    protected virtual void DieUpdate()
    //    {
    //    }

    //    //protected float floorLength;
    //    protected Vector3 startPos;
    //    protected Vector3 endPos;

    //    protected virtual void ChangePattern()
    //    {
    //        patternTime += Time.deltaTime;

    //        if (EnemyStatePattern[curCountPattern].second > patternTime)
    //            return;

    //        patternTime = 0f;

    //        if (curCountPattern == EnemyStatePattern.Count - 1)
    //        {
    //            curCountPattern = 0;
    //        }
    //        else
    //        {
    //            ++curCountPattern;
    //        }

    //        State = EnemyStatePattern[curCountPattern].state;
    //    }

    //    protected virtual void Attack()
    //    {
    //        switch (meleeAttack)
    //        {
    //            case EnemyMeleeAttack:
    //                {
    //                    attackBox.SetActive(true);
    //                    //Play Attack Sound
    //                    enemySound.PlayAttackSound();
    //                }
    //                break;
    //        }
    //    }

    //    protected virtual void ResetPattern()
    //    {
    //        patternTime = 0f;
    //        curCountPattern = 0;
    //        isGoingRight = preGoingRight;
    //    }

    //    protected virtual void OnTriggerEnter(Collider collider)
    //    {
    //        if (!attackBox.activeSelf)
    //            return;

    //        if (collider.gameObject.CompareTag("Player") && collider.gameObject.GetComponent<ObjectMass>() != null)
    //        {
    //#if UNITY_EDITOR
    //            //Debug.Log(State);
    //#endif
    //            meleeAttack.ExecuteAttack(gameObject, player.gameObject, transform.position);
    //            attackBox.SetActive(false);
    //        }
    //    }

    //    protected virtual void AttackDone()
    //    {
    //        State = EnemyState.Chase;
    //        attackBox.SetActive(false);
    //    }
    //    public bool isKbAnimation;
    //    protected virtual void TakeDamageDone()
    //    {
    //        if (State == EnemyState.Die)
    //            return;

    //        isKbAnimation = false;
    //        //State = EnemyState.Chase;
    //    }

    //    protected virtual void DieDone()
    //    {
    //        gameObject.SetActive(false);
    //    }

    //    private void BompDieDone()
    //    {
    //        enemySound.PlayDeathSound();
    //        State = EnemyState.Die;
    //        animator.ResetTrigger("TakeDamage");
    //        animator.SetTrigger("Die");
    //        isLive = false;
    //        enemyBody.SetActive(false);

    //        player.GetComponent<PlayerLevelManager>().CurrExp += DataTableMgr.GetTable<MonsterData>().Get(status.id).exp;

    //        gameObject.SetActive(false);
    //    }
    //    protected virtual void KnockBack()
    //    {
    //        if (State == EnemyState.Die)
    //            return;
    //        isKbAnimation = true;
    //        State = EnemyState.KnockBack;
    //        animator.SetTrigger("TakeDamage");
    //    }

    //    private bool isStun = false;
    //    private int stunCount = 0;
    //    private float stunCool;
    //    private float stunCoolTime = 0;
    //    protected virtual void Stun(float stunCool)
    //    {
    //        if (isStun)
    //            this.stunCool += SetStunTime(stunCool, stunCount);
    //        else
    //            this.stunCool = SetStunTime(stunCool, stunCount);

    //        ++stunCount;

    //        if (stunCoolTime <= 0f)
    //            return;

    //        isStun = true;

    //        var effect = GameManager.instance.effectManager.GetEffect("Stun");
    //        Bounds bounds = transform.GetComponent<Collider>().bounds;
    //        effect.transform.position = new Vector3(transform.position.x, bounds.center.y + bounds.max.y + 5, transform.position.z);
    //        GameManager.instance.effectManager.ReturnEffectOnTime("Stun", effect, stunCool);
    //        effect.transform.SetParent(transform);

    //    }

    //    private bool onSlowDown;
    //    private float slowDownTime = 0;
    //    private float slowDownTimer;

    //    protected virtual void SlowDown(float newSlowDown, float newSlowTime)
    //    {
    //        onSlowDown = true;
    //        slowDownTime = newSlowTime;
    //        slowDownTimer = 0f;
    //        patrolSpeed = defaultPatrolSpeed * (1f - newSlowDown);
    //        chaseSpeed = defaultChaseSpeed * (1f - newSlowDown);
    //        SetSpeed();
    //        GameObject effect = GameManager.instance.effectManager.GetEffect("Fog_speedSlow(blue)");
    //        effect.transform.position = transform.position;
    //        GameManager.instance.effectManager.ReturnEffectOnTime("Fog_speedSlow(blue)", effect, newSlowTime);
    //        effect.transform.SetParent(transform);

    //    }

    //    protected virtual void SetSpeed()
    //    {
    //        switch (State)
    //        {
    //            case EnemyState.Chase:
    //                agent.speed = chaseSpeed;
    //                break;
    //            case EnemyState.Patrol:
    //                agent.speed = patrolSpeed;
    //                break;
    //        }
    //    }

    //    protected virtual void EndSlowDown()
    //    {
    //        slowDownTimer = 0f;
    //        patrolSpeed = defaultPatrolSpeed;
    //        chaseSpeed = defaultChaseSpeed;
    //        SetSpeed();
    //    }
}
