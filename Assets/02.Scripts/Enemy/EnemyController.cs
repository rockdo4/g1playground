using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class EnemyController : MonoBehaviour
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

    private int curCountPattern;
    private int countPattern;
    private bool isPattern;
    private bool isAttack;

    private Transform player;
    Transform agentTransform;
    Vector3 lookDirection;

    private float floorLength;
    private Vector3 startPos;
    private Vector3 endPos;
    private bool isGoingRight;

    public AttackDefinition attackDef;

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

            if (prevState == state)
                return;

            switch (State)
            {
                case EnemyState.Idle:
                    agent.isStopped = true;
                    break;
                case EnemyState.Patrol:
                    agent.isStopped = false;
                    if (isGoingRight)
                    {
                        agent.SetDestination(endPos);
                        if (Vector3.Distance(transform.position, endPos) < 3f)
                        {
                            isGoingRight = false;
                        }
                    }
                    else
                    {
                        agent.SetDestination(startPos);
                        if (Vector3.Distance(transform.position, startPos) < 3f)
                        {
                            isGoingRight = true;
                        }
                    }
                    break;
                case EnemyState.Chase:
                    agent.speed = moveSpeed;
                    agent.isStopped = false;
                    break;
                case EnemyState.Attack:
                    agent.isStopped = true;
                    break;

            }
        }
    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        agentTransform = agent.transform;
        agent.stoppingDistance = attackRange;

    }

    void Start()
    {
        player = GameObject.FindWithTag("Player").transform;
        State = EnemyStatePattern[0].state;
        countPattern = EnemyStatePattern.Count - 1;
        curCountPattern = 0;
        isPattern = true;
        SaveFloorLength();
    }

    void Update()
    {

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
        }

        Debug.Log(state);
        animator.SetFloat("Move", agent.velocity.magnitude);
    }

    private void IdleUpdate()
    {
        if (Vector3.Distance(transform.position, player.position) < searchRange)
        {
            State = EnemyState.Chase;
            return;
        }
    }

    private void PatrolUpdate()
    {
        if (Vector3.Distance(transform.position, player.position) < searchRange)
        {
            State = EnemyState.Chase;
            return;
        }
    }

    private void ChaseUpdate()
    {
        if (Vector3.Distance(transform.position, player.position) < attackRange)
        {
            State = EnemyState.Attack;
            ResetPattern();
            lookDirection = (player.position - agentTransform.position).normalized;
            lookDirection.y = 0f;
            agentTransform.forward = lookDirection;
            return;
        }
        else if (Vector3.Distance(transform.position, player.position) > searchRange)
        {
            ResetPattern();
            isPattern = true;
            return;
        }
        agent.isStopped = false;
        agent.SetDestination(player.position);
    }
    private void AttackUpdate()
    {
        if (Vector3.Distance(transform.position, player.position) > attackRange)
        {
            animator.SetBool("Attack", false);
            State = EnemyState.Chase;
        }
        isAttack = true;
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
        StartCoroutine(PatternDelay(EnemyStatePattern[curCountPattern].second));
        isPattern = false;
    }

    IEnumerator PatternDelay(float delayTime)
    {
        yield return new WaitForSeconds(delayTime);

        //if (isAttack)
        //    yield break;

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

    private void ResetPattern()
    {
        curCountPattern = 0;
        isPattern = false;
        isAttack = false;
    }
}
