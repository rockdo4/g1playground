using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class EnemyController : Enemy
{
    GameObject attackBox;
    private CapsuleCollider mainColl;
    public BasicAttack meleeAttack;
    private OnGround onGround;
    [System.Serializable]
    public class EnemyStateData
    {
        public EnemyState state;
        public float second;
    }

    [Header("<Only Idle and Patrol>")]
    [SerializeField]
    public List<EnemyStateData> EnemyStatePattern = new List<EnemyStateData>();
    private int curCountPattern;
    private float patternTime = 0f;
    public bool isGoingRight;


    public float defaultPatrolSpeed;
    public float defaultChaseSpeed;
    private float patrolSpeed;
    private float chaseSpeed;
    public float searchRange;
    public float attackRange;
    public float attackCool;
    private float attackTime;
    public override EnemyState State
    {
        get { return state; }
        protected set
        {
            var prevState = state;
            if (EnemyState.Die == prevState && EnemyState.None != value)
                return;

            state = value;
            if (prevState == state)
                return;

            switch (State)
            {
                case EnemyState.None:
                    agent.enabled = true;
                    agent.isStopped = true;
                    agent.velocity = Vector3.zero;
                    rb.isKinematic = true;
                    mainColl.enabled = true;
                    break;
                case EnemyState.Idle:
                    agent.isStopped = true;
                    rb.isKinematic = true;
                    break;
                case EnemyState.Patrol:
                    agent.isStopped = false;
                    agent.speed = patrolSpeed;
                    rb.isKinematic = true;
                    break;
                case EnemyState.Chase:
                    agent.speed = chaseSpeed;
                    agent.enabled = true;
                    agent.isStopped = false;
                    rb.isKinematic = true;
                    break;
                case EnemyState.Attack:
                    agent.isStopped = true;
                    agent.velocity = Vector3.zero;
                    rb.isKinematic = true;
                    break;
                case EnemyState.TakeDamage:
                    agent.speed = chaseSpeed;
                    agent.enabled = true;
                    agent.isStopped = false;
                    rb.isKinematic = true;
                    //agent.isStopped = true;
                    //agent.enabled = false;
                    //rb.isKinematic = false;
                    break;
                case EnemyState.KnockBack:
                    agent.isStopped = true;
                    agent.enabled = false;
                    rb.isKinematic = false;
                    break;
                case EnemyState.Stun:
                    agent.isStopped = true;
                    agent.enabled = true;
                    rb.isKinematic = true;
                    break;
                case EnemyState.Die:
                    agent.velocity = Vector3.zero;
                    agent.enabled = true;
                    agent.isStopped = true;
                    rb.isKinematic = true;
                    mainColl.enabled = false;
                    break;
            }

            //Debug.Log(State);
        }
    }


    public bool preGoingRight;

    protected override void Awake()
    {
        base.Awake();
        mainColl = GetComponent<CapsuleCollider>();
        attackBox = GameObject.Find(gameObject.name + "/AttackBox");
        attackBox.SetActive(false);
        preGoingRight = isGoingRight;
        linkMover = GetComponent<AgentLinkMover>();
        onGround = GetComponentInChildren<OnGround>();
    }
    private IEnumerator RestorePosition()
    {
        yield return new WaitForEndOfFrame();

    }
    protected override void Start()
    {
        animator.ResetTrigger("TakeDamage");
        base.Start();
        patrolSpeed = defaultPatrolSpeed;
        chaseSpeed = defaultChaseSpeed;

    }

    protected AgentLinkMover linkMover;
    protected override void OnEnable()
    {
        base.OnEnable();

        State = EnemyState.None;
        //RemoveAgentLinkMover();
        //AddAgentLinkMover();
        ResetPattern();
        //agent.isOnOffMeshLink = true;
        //linkMover.YourLogicCoroutine();
    }
    void RemoveAgentLinkMover()
    {
        AgentLinkMover agentLinkMover = GetComponent<AgentLinkMover>();
        if (agentLinkMover != null)
        {
            Destroy(agentLinkMover);
        }
    }

    void AddAgentLinkMover()
    {
        gameObject.AddComponent<AgentLinkMover>();
    }

    IEnumerator WaitForAgentInitialization()
    {
        while (agent.enabled && agent.pathPending)
        {
            yield return null;
        }

        if (linkMover == null)
        {
            Debug.LogError("AgentLinkMover not found.");
            yield break;
        }

        // NavMeshAgent�� �ʱ�ȭ �� ����� ���Ŀ� AgentLinkMover�� Ȱ��ȭ
        linkMover.enabled = true;
    }

    protected void Update()
    {
        attackTime += Time.deltaTime;


        if (isStun)
        {
            stunCoolTime += Time.deltaTime;

            if (State != EnemyState.KnockBack && State != EnemyState.Stun)
            {
                State = EnemyState.Stun;
            }
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
            case EnemyState.Chase:
                ChaseUpdate();
                break;
            case EnemyState.Attack:
                AttackUpdate();
                break;
            case EnemyState.TakeDamage:
                TakeDamageUpdate();
                break;
            case EnemyState.KnockBack:
                KnockBackUpdate();
                break;
            case EnemyState.Stun:
                StunUpdate();
                break;
            case EnemyState.Die:
                DieUpdate();
                break;
        }

        if (Input.GetKeyDown(KeyCode.F))
        {
            State = EnemyState.Die;
            animator.SetTrigger("Die");
            status.CurrHp -= 19000;

        }

        animator.SetFloat("Move", agent.velocity.magnitude / chaseSpeed);
    }
    protected void None()
    {
        SaveFloorLength(ref startPos, ref endPos);
        State = EnemyStatePattern[0].state;
    }
    protected override void IdleUpdate()
    {
        ChangePattern();
        RayShooter(searchRange, isGoingRight);
    }
    protected override void PatrolUpdate()
    {
        ChangePattern();
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

    private bool currGoingRight;
    private float findingpathtime;

    protected override void ChaseUpdate()
    {
        NavMeshPath navMeshPath = new NavMeshPath();

        if (NavMesh.CalculatePath(transform.position, player.transform.position, NavMesh.AllAreas, navMeshPath)
            && navMeshPath.status == NavMeshPathStatus.PathComplete)
        {
            Vector3 targetDirection = player.transform.position - transform.position;
            targetDirection.y = 0;
            targetDirection.Normalize();
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(targetDirection), Time.deltaTime * 10f);
            findingpathtime = 0;

            if (player.transform.position.x - transform.position.x > 0)
                isGoingRight = true;
            else
                isGoingRight = false;

            if (currGoingRight != isGoingRight)
            {
                agent.velocity = Vector3.zero;
                currGoingRight = isGoingRight;
            }

            if (AngleIgnoringHeight(10f))
            {
                agent.isStopped = false;
                agent.SetDestination(player.transform.position);
            }
            else
            {
                agent.isStopped = true;
            }

            if (RayShooter(attackRange, isGoingRight))
            {
                if (attackTime >= attackCool)
                    State = EnemyState.Attack;
                else
                {
                    agent.isStopped = true;
                    agent.velocity = Vector3.zero;
                }
            }
            else
                agent.isStopped = false;

        }
        else
        {

            findingpathtime += Time.deltaTime;
            if (findingpathtime >= 1.5f)
            {
                State = EnemyState.None;
                ResetPattern();
            }
        }


    }

    protected override void AttackUpdate()
    {
        if (attackTime >= attackCool)
        {
            animator.SetTrigger("Attack");
            attackTime = 0f;
            return;
        }
    }

    protected override void TakeDamageUpdate()
    {

    }
    protected override void KnockBackUpdate()
    {
        if (onGround.isGround && !isKbAnimation) 
        {
            State = EnemyState.Chase;
        }
    }

    private void StunUpdate()
    {
        if (stunCoolTime >= stunCool)
        {
            stunCool = 0f;
            stunCoolTime = 0f;
            isStun = false;
            State = EnemyState.Chase;
        }
    }

    //public void OnAttack(GameObject attacker, Attack attack, Vector3 attackPos)
    //{
    //    if (State == EnemyState.Die)
    //        return;

    //    State = EnemyState.TakeDamage;
    //    animator.SetTrigger("TakeDamage");
    //}
    protected override void DieUpdate()
    {
    }

    //protected float floorLength;
    protected Vector3 startPos;
    protected Vector3 endPos;

    private void ChangePattern()
    {
        patternTime += Time.deltaTime;

        if (EnemyStatePattern[curCountPattern].second > patternTime)
            return;

        patternTime = 0f;

        if (curCountPattern == EnemyStatePattern.Count - 1)
        {
            curCountPattern = 0;
        }
        else
        {
            ++curCountPattern;
        }

        State = EnemyStatePattern[curCountPattern].state;
    }

    private void Attack()
    {
        switch (meleeAttack)
        {
            case EnemyMeleeAttack:
                {
                    attackBox.SetActive(true);
                    //Play Attack Sound
                }
                break;
        }
    }

    protected void ResetPattern()
    {
        patternTime = 0f;
        curCountPattern = 0;
        isGoingRight = preGoingRight;
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (!attackBox.activeSelf)
            return;

        if (collider.gameObject.CompareTag("Player") && collider.gameObject.GetComponent<ObjectMass>() != null)
        {
#if UNITY_EDITOR
            //Debug.Log(State);
#endif
            meleeAttack.ExecuteAttack(gameObject, player.gameObject, transform.position);
            attackBox.SetActive(false);
        }
    }

    private void AttackDone()
    {
        State = EnemyState.Chase;
        attackBox.SetActive(false);
    }
    public bool isKbAnimation;
    private void TakeDamageDone()
    {
        if (State == EnemyState.Die)
            return;
        isKbAnimation = false;
        //State = EnemyState.Chase;
    }

    private void DieDone()
    {
        gameObject.SetActive(false);
    }

    public override void KnockBack()
    {
        if (State == EnemyState.Die)
            return;
        isKbAnimation = true;
        State = EnemyState.KnockBack;
        animator.SetTrigger("TakeDamage");
    }

    private bool isStun = false;
    private int stunCount = 0;
    private float stunCool;
    private float stunCoolTime = 0;
    public override void Stun(float stunCool)
    {
        if (isStun)
            this.stunCool += SetStunTime(stunCool, stunCount);
        else
            this.stunCool = SetStunTime(stunCool, stunCount);

        ++stunCount;

        if (stunCoolTime <= 0f)
            return;

        isStun = true;

        var effect=GameManager.instance.effectManager.GetEffect("Stun");
        Bounds bounds = transform.GetComponent<Collider>().bounds;
        effect.transform.position = new Vector3(transform.position.x, bounds.center.y + bounds.max.y + 5, transform.position.z);
        GameManager.instance.effectManager.ReturnEffectOnTime("Stun", effect, stunCool);
        effect.transform.SetParent(transform);
       
    }

    private bool onSlowDown;
    private float slowDownTime = 0;
    private float slowDownTimer;

    public void SlowDown(float newSlowDown, float newSlowTime)
    {
        onSlowDown = true;
        slowDownTime = newSlowTime;
        slowDownTimer = 0f;
        patrolSpeed = defaultPatrolSpeed * (1f - newSlowDown);
        chaseSpeed = defaultChaseSpeed * (1f - newSlowDown);
        SetSpeed();
        GameObject effect = GameManager.instance.effectManager.GetEffect("Fog_speedSlow(blue)");
        effect.transform.position = transform.position;
        GameManager.instance.effectManager.ReturnEffectOnTime("Fog_speedSlow(blue)", effect, newSlowTime);
        effect.transform.SetParent(transform);

    }

    private void SetSpeed()
    {
        switch (State)
        {
            case EnemyState.Chase:
                agent.speed = chaseSpeed;
                break;
            case EnemyState.Patrol:
                agent.speed = patrolSpeed;
                break;
        }
    }

    public void EndSlowDown()
    {
        slowDownTimer = 0f;
        patrolSpeed = defaultPatrolSpeed;
        chaseSpeed = defaultChaseSpeed;
        SetSpeed();
    }
}
