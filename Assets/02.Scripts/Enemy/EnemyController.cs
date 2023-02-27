using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.AI;


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
    private EnemyState state;
    private NavMeshAgent agent;
    private Animator animator;

    public float moveSpeed;
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
            if(State!= EnemyState.TakeDamage)
            {
                //rb.isKinematic = true;
                //rb.velocity = Vector3.zero;
                agent.enabled = false;
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
                    agent.isStopped = false;
                    rb.isKinematic = false;
                    break;
                case EnemyState.Chase:
                    agent.isStopped = false;
                    rb.isKinematic = true;
                    break;
                case EnemyState.Attack:
                    agent.isStopped = true;
                    rb.isKinematic = true;
                    break;
                case EnemyState.TakeDamage:
                    agent.isStopped = true;
                    rb.isKinematic = false;
                    agent.enabled = true;
                    takeDamageCoolTime = 0f;
                    break;
                case EnemyState.Die:
                    agent.isStopped = true;
                    rb.isKinematic = false;
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
        agent.speed = moveSpeed;
        //agent.stoppingDistance = attackRange;
        GetComponent<DestructedEvent>().OnDestroyEvent = OnDestroyObj;
    }

    void Start()
    {
        player = GameObject.FindWithTag("Player");
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

        Debug.Log(state);
        animator.SetFloat("Move", agent.velocity.magnitude);
    }
    //private void FixedUpdate()
    //{
    //    if (Input.GetKeyDown(KeyCode.Z))
    //    {
    //        State = EnemyState.TakeDamage;

    //        agent.enabled = false;

    //        rb.isKinematic = false;
    //        rb.velocity = Vector3.zero;

    //        var force = -transform.forward;
    //        force.y = 3.5f;
    //        force = force.normalized * 10;

    //        rb.AddForce(force, ForceMode.Impulse);

    //        //State = EnemyState.Die;
    //        //animator.SetTrigger("Die");
    //        //State = EnemyState.TakeDamage;
    //        //animator.SetTrigger("TakeDamage");

    //        //Vector3 pushBackDirection = transform.position - player.transform.position;
    //        //pushBackDirection.y += 5f;
    //        //pushBackDirection = pushBackDirection.normalized;
    //        //agent.velocity = pushBackDirection * 10;
    //        //rb.AddForce(Vector3.up * 30, ForceMode.Impulse);

    //        //var dir = transform.position - player.transform.position;
    //        //dir.y += 5;
    //        //dir.Normalize();
    //        //agent.destination = Vector3.zero;
    //        //agent.updatePosition = false;
    //        //agent.updateRotation = false;
    //        //agent.isStopped = true;
    //        //agent.enabled = false;
    //        //agent.Move(dir * 10);
    //        //rb.isKinematic = false;
    //        //rb.AddForce(dir * 10, ForceMode.Impulse);

    //    }

    //    time += Time.deltaTime;

    //    //if (State == EnemyState.TakeDamage)
    //    //{
    //    //    var dir = transform.position - player.transform.position;
    //    //    dir.y += 35;
    //    //    dir.Normalize();
    //    //    rb.AddForce(dir * 10, ForceMode.Impulse);
    //    //    //agent.Move(dir);
    //    //}

    //    if (isPattern)
    //    {
    //        ChangePatteurn();
    //    }

    //    switch (state)
    //    {
    //        case EnemyState.Idle:
    //            IdleUpdate();
    //            break;
    //        case EnemyState.Chase:
    //            ChaseUpdate();
    //            break;
    //        case EnemyState.Patrol:
    //            PatrolUpdate();
    //            break;
    //        case EnemyState.Attack:
    //            AttackUpdate();
    //            break;
    //        case EnemyState.TakeDamage:
    //            TakeDamageUpdate();
    //            break;
    //    }

    //    //Debug.Log(state);
    //    Debug.Log(state);
    //    animator.SetFloat("Move", agent.velocity.magnitude);
    //}

    private void IdleUpdate()
    {
        if (Vector3.Distance(transform.position, player.transform.position) < searchRange)
        {
            State = EnemyState.Chase;
            return;
        }
    }

    private void PatrolUpdate()
    {
        if (Vector3.Distance(transform.position, player.transform.position) < searchRange)
        {
            State = EnemyState.Chase;
            return;
        }

        if (isGoingRight)
        {
            agent.SetDestination(endPos);
            if (Vector3.Distance(transform.position, endPos) < 3f)
            {
                transform.rotation = Quaternion.LookRotation(-transform.forward);
                isGoingRight = false;
            }
        }
        else
        {
            agent.SetDestination(startPos);
            if (Vector3.Distance(transform.position, startPos) < 3f)
            {
                transform.rotation = Quaternion.LookRotation(-transform.forward);
                isGoingRight = true;
            }
        }
    }

    private void ChaseUpdate()
    {
        if (Vector3.Distance(transform.position, player.transform.position) <= attackRange + 0.5f)
        {
            State = EnemyState.Attack;
            return;
        }

        if (Vector3.Distance(transform.position, player.transform.position) >= searchRange)
        {
            State = EnemyState.Idle;
            return;
        }

        agent.SetDestination(player.transform.position);
        transform.LookAt(transform.position + agent.desiredVelocity);
    }
    private void AttackUpdate()
    {
        var isGround = player.GetComponent<PlayerController>().isGrounded;

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

    private void SaveFloorLength()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, -Vector3.up, out hit))
        {
            Collider collider = hit.collider;
            floorLength = collider.bounds.size.x;

            startPos = collider.bounds.center - (new Vector3((floorLength / 2), 0, 0));
            endPos = collider.bounds.center + (new Vector3((floorLength / 2), 0, 0));

            isGoingRight = true;
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
        Debug.Log(1);
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
            State = EnemyState.Idle;
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
}
