using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using static Enemy;

[System.Serializable]
public class EnemyStateData
{
    public EnemyState state;
    public float second;
}
public class Enemy : MonoBehaviour
{
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

    //private float distance;
    private Transform player;
    Transform agentTransform;
    Vector3 lookDirection;

    [Header("<Only Idle and Patrol>")]
    [SerializeField]
    public List<EnemyStateData> EnemyStatePattern = new List<EnemyStateData>();

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        agentTransform = agent.transform;
        agent.speed = moveSpeed;
        agent.stoppingDistance = attackRange;

    }

    private void Start()
    {
        player = GameObject.FindWithTag("Player").transform;
        state = EnemyStatePattern[0].state;
        countPattern = EnemyStatePattern.Count - 1;
        curCountPattern = 0;
        isPattern = true;
        isAttack = false;

        SaveFloorLength();
    }

    private void Update()
    {
        //distance = Vector3.Distance(transform.position, player.position);

        if (!isAttack)
        {
            if (Vector3.Distance(transform.position, player.position) < searchRange)
            {
                state = EnemyState.Chase;
                ResetPattern();
                isAttack = true;
            }
        }

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
    }

    private void IdleUpdate()
    {
        //if (distance < searchRange)
        //{
        //    state = EnemyState.Chase;
        //    ResetPattern();
        //    return;
        //}
        agent.isStopped = true;
        animator.SetBool("Run", false);
        animator.SetBool("Idle", true);
    }

    private void PatrolUpdate()
    {
        //if (distance < searchRange)
        //{
        //    state = EnemyState.Chase;
        //    ResetPattern();
        //    return;
        //}

        agent.isStopped = false;

        animator.SetBool("Idle", false);
        animator.SetBool("Run", true);

        if (isGoingRight)
        {
            //Debug.Log("��");
            agent.SetDestination(endPos);

            if (Vector3.Distance(transform.position, endPos) < 3f)
            {
                isGoingRight = false;
                agent.isStopped = true;
            }
        }
        else
        {
            //Debug.Log("��");
            agent.SetDestination(startPos);

            if (Vector3.Distance(transform.position, startPos) < 3f)
            {
                isGoingRight = true;
                agent.isStopped = true;

            }
        }
        //Debug.Log(agent.destination);
    }

    private void ChaseUpdate()
    {
        if (Vector3.Distance(transform.position, player.position) < attackRange)
        {
            state = EnemyState.Attack;
            animator.SetBool("Run", false);
            animator.SetBool("Idle", false);
            animator.SetBool("Attack", true);
            agent.isStopped = true;
            ResetPattern();
            lookDirection = (player.position - agentTransform.position).normalized;
            lookDirection.y = 0f;
            //lookDirection = new Vector3(0f, 0f, z).normalized;
            agentTransform.forward = lookDirection;
            return;
        }
        else if (Vector3.Distance(transform.position, player.position) > searchRange)
        {
            ResetPattern();
            animator.SetBool("Run", false);
            isPattern = true;
            return;
        }

        agent.isStopped = false;
        animator.SetBool("Run", true);
        agent.SetDestination(player.position);

    }
    private void AttackUpdate()
    {

        if (Vector3.Distance(transform.position, player.position) > attackRange)
        {
            animator.SetBool("Attack", false);
            state = EnemyState.Chase;
        }
        isAttack = true;
    }

    private float floorLength;
    private Vector3 startPos;
    private Vector3 endPos;
    private bool isGoingRight;

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

        if (isAttack)
            yield break;

        if (countPattern == curCountPattern)
        {
            curCountPattern = 0;
        }
        else
        {
            ++curCountPattern;
        }

        isPattern = true;
        state = EnemyStatePattern[curCountPattern].state;
    }

    private void ResetPattern()
    {
        //countPattern = 0;
        curCountPattern = 0;
        isPattern = false;
        isAttack = false;
    }
}