using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using static UnityEngine.Rendering.DebugUI;
using static UnityEngine.RuleTile.TilingRuleOutput;


public class EnemyController : MonoBehaviour, IAttackable
{
    [System.Serializable]
    public class EnemyStateData
    {
        public EnemyState state;
        public float second;
    }

    [System.Serializable]
    public enum EnemyState
    {
        None,
        Idle,
        Chase,
        Patrol,
        Attack,
        TakeDamage,
        Stun,
        Die,
    }

    private Rigidbody rb;
    private NavMeshAgent agent;
    private Animator animator;

    private EnemyState state;

    public float chaseSpeed;
    public float patrolSpeed;
    public float attackRange;
    public float searchRange;
    public float attackCool;
    private float time = 0f;

    private int curCountPattern;
    private int countPattern;
    private bool isPattern;

    private GameObject player;


    private float floorLength;
    private Vector3 startPos;
    private Vector3 endPos;
    private bool isGoingRight;

    public BasicAttack basicAttack;
    public Status status;

    [Header("<Only Idle and Patrol>")]
    [SerializeField]
    public List<EnemyStateData> EnemyStatePattern = new List<EnemyStateData>();

    public EnemyState State
    {
        get { return state; }
        private set
        {
            var prevState = state;
            state = value;

            if (prevState != EnemyState.Idle && prevState != EnemyState.Patrol)
            {
                if (value == EnemyState.Idle || value == EnemyState.Patrol)
                {
                    SaveFloorLength();
                    isPattern = true;
                }
            }
            if (State != EnemyState.TakeDamage)
            {
                //rb.isKinematic = true;
                //rb.velocity = Vector3.zero;
                agent.enabled = true;
            }

            if (prevState == state)
                return;


            switch (State)
            {
                case EnemyState.Idle:
                    agent.isStopped = true;
                    rb.isKinematic = true;
                    break;
                case EnemyState.Patrol:
                    SaveFloorLength();
                    agent.speed = patrolSpeed;
                    agent.isStopped = false;
                    rb.isKinematic = true;
                    break;
                case EnemyState.Chase:
                    agent.speed = chaseSpeed;
                    agent.isStopped = false;
                    rb.isKinematic = true;
                    break;
                case EnemyState.Attack:
                    agent.velocity = Vector3.zero;
                    agent.isStopped = true;
                    rb.isKinematic = true;
                    break;
                case EnemyState.TakeDamage:
                    agent.velocity = Vector3.zero;
                    agent.isStopped = true;
                    rb.isKinematic = false;
                    agent.enabled = false;
                    takeDamageCoolTime = 0f;
                    break;
                case EnemyState.Die:
                    agent.enabled = true;
                    agent.isStopped = true;
                    rb.isKinematic = true;
                    break;

            }
        }
    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        status = GetComponent<Status>();
        agent.speed = chaseSpeed;
        //agent.stoppingDistance = attackRange;
        GetComponent<DestructedEvent>().OnDestroyEvent = OnDestroyObj;
    }

    void Start()
    {
        hitBoxColl = GetComponent<BoxCollider>();
        hitBoxColl.tag = "HitBox";
        player = GameManager.instance.player;
        State = EnemyStatePattern[0].state;
        countPattern = EnemyStatePattern.Count - 1;
        curCountPattern = 0;
        isPattern = true;
        SaveFloorLength();
    }


    void Update()
    {
        time += Time.deltaTime;

        if (isPattern)
        {
            ChangePatteurn();
        }

        switch (state)
        {
            case EnemyState.Idle:
                IdleUpdate();
                break;
            case EnemyState.Chase:
                ChaseUpdate();
                break;
            case EnemyState.Patrol:
                PatrolUpdate();
                break;
            case EnemyState.Attack:
                AttackUpdate();
                break;
            case EnemyState.TakeDamage:
                TakeDamageUpdate();
                break;
        }

        //Debug.Log(state);
        animator.SetFloat("Move", agent.velocity.magnitude);
    }

    private void IdleUpdate()
    {
        var pos = GetComponent<CapsuleCollider>();

        Ray ray = new Ray(pos.bounds.max, pos.transform.forward);
        RaycastHit hit;
        //Debug.DrawRay(ray.origin, ray.direction * searchRange, Color.red);
        if (Physics.Raycast(ray, out hit, searchRange))
        {
            if (hit.collider.tag == "Player")
            {
                State = EnemyState.Chase;
                return;
            }
        }
    }

    private void PatrolUpdate()
    {
        var pos = GetComponent<CapsuleCollider>();

        Ray ray = new Ray(pos.bounds.max, pos.transform.forward);
        RaycastHit hit;
        //Debug.DrawRay(ray.origin, ray.direction * searchRange, Color.red);
        if (Physics.Raycast(ray, out hit, searchRange))
        {
            if (hit.collider.tag == "Player")
            {
                State = EnemyState.Chase;
                return;
            }
        }

        if (isGoingRight)
        {
            agent.SetDestination(endPos);
            if (Vector3.Distance(transform.position, endPos) < 3f)
            {
                transform.rotation = Quaternion.LookRotation(-transform.forward);
                agent.velocity = Vector3.zero;
                isGoingRight = false;
            }
        }
        else
        {
            agent.SetDestination(startPos);
            if (Vector3.Distance(transform.position, startPos) < 3f)
            {
                transform.rotation = Quaternion.LookRotation(-transform.forward);
                agent.velocity = Vector3.zero;
                isGoingRight = true;
            }
        }
    }

    private void ChaseUpdate()
    {
        //if (Vector3.Distance(transform.position, player.transform.position) <= attackRange + 0.5f)
        //{
        //    State = EnemyState.Attack;
        //    return;
        //}

        if (Vector3.Distance(transform.position, player.transform.position) >= searchRange * 3)
        {
            State = EnemyState.Idle;
            return;
        }
        var isGround = player.GetComponent<PlayerController>().isGrounded;

        if (!isGround) { return; }
        agent.SetDestination(player.transform.position);
        transform.LookAt(transform.position + agent.desiredVelocity);
    }
    private void AttackUpdate()
    {
        if (Vector3.Distance(transform.position, player.transform.position) >= attackRange + 0.5f)
        {
            State = EnemyState.Chase;
        }

        var lookDirection = (player.transform.position - transform.position).normalized;
        lookDirection.y = 0f;
        transform.forward = lookDirection;

        if (time > attackCool)
        {
            animator.SetTrigger("Attack");
            time = 0f;
        }
    }

    //private void OnTriggerEnter(Collider collider)
    //{
    //    if (state == EnemyState.Attack)
    //        return;

    //    if (attackBoxColl.tag == "HitBox")
    //    {
    //        if (collider.tag == "Player")
    //        {
    //            State = EnemyState.Attack;
    //            return;
    //        }
    //    }
    //}

    //private void OnTriggerExit(Collider collider)
    //{
    //    if (state == EnemyState.Chase)
    //        return;

    //    if (attackBoxColl.tag == "HitBox")
    //    {
    //        if (collider.tag == "Player")
    //        {
    //            State = EnemyState.Chase;
    //            return;
    //        }
    //    }
    //}

    private void SaveFloorLength()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, -Vector3.up, out hit))
        {
            Collider collider = hit.collider;
            floorLength = collider.bounds.size.x;

            startPos = collider.bounds.center - (new Vector3((floorLength / 2), 0, 0));
            endPos = collider.bounds.center + (new Vector3((floorLength / 2), 0, 0));

            //isGoingRight = true;
        }
        else
        {
#if UNITY_EDITOR
            Debug.Log("NULL!!");
#endif
        }
    }

    void ChangePatteurn()
    {
        StartCoroutine(CoPatternDelay(EnemyStatePattern[curCountPattern].second));
        isPattern = false;
    }

    IEnumerator CoPatternDelay(float delayTime)
    {
        yield return new WaitForSeconds(delayTime);

        if (state != EnemyState.Idle && state != EnemyState.Patrol)
        {
            isPattern = false;
            curCountPattern = 0;

            yield break;
        }

        if (countPattern == curCountPattern)
        {
            curCountPattern = 0;
        }
        else
        {
            ++curCountPattern;
        }

        isPattern = true;
        State = EnemyStatePattern[curCountPattern].state;
    }

    public void Attack()
    {
        switch (basicAttack)
        {
            case EnemyMeleeAttack:
                {

                    if (Vector3.Distance(transform.position, player.transform.position) <= attackRange + 0.5f)
                    {
                        basicAttack.ExecuteAttack(gameObject, player.gameObject);
                        return;
                    }
                }
                break;
        }
    }

    private float takeDamageCool = 0.8f;
    private float takeDamageCoolTime = 0f;
    public void TakeDamageUpdate()
    {
        takeDamageCoolTime += Time.deltaTime;

        if (takeDamageCool < takeDamageCoolTime)
        {
            State = EnemyState.Chase;
        }
    }

    public void OnAttack(GameObject attacker, Attack attack)
    {
        State = EnemyState.TakeDamage;
        animator.SetTrigger("TakeDamage");
    }

    public void OnDestroyObj()
    {
        State = EnemyState.Die;
        animator.SetTrigger("Die");
    }

    private void EnemyDie()
    {
        gameObject.SetActive(false);
    }

    public void GetAttackBoxCollEnter(Collider collider, Collider attackBoxColl)
    {
        if (state == EnemyState.Attack)
            return;

        if (attackBoxColl.tag == "AttackBox")
        {
            if (collider.tag == "Player")
            {
                State = EnemyState.Attack;
                return;
            }
        }
    }
    public void GetAttackBoxCollExit(Collider collider, Collider attackBoxColl)
    {
        if (state == EnemyState.Chase)
            return;

        if (attackBoxColl.tag == "AttackBox")
        {
            if (collider.tag == "Player")
            {
                State = EnemyState.Chase;
                return;
            }
        }
    }
}