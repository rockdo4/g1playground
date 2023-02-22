using System;
using System.Collections;
using System.Collections.Generic;
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

    private float distance;
    private Transform player;

    [Header("<Only Idle and Patrol>")]
    [SerializeField]
    public List<EnemyStateData> EnemyStatePattern = new List<EnemyStateData>();

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        agent.speed = moveSpeed;
        agent.stoppingDistance = attackRange;

    }

    private void Start()
    {
        player = GameObject.FindWithTag("Player").transform;
        state = EnemyState.Idle;
        countPattern = EnemyStatePattern.Count - 1;
        curCountPattern = 0;
        state = EnemyStatePattern[0].state;
        isPattern = true;
        isAttack = false;

        SaveFloorLength();
    }

    private void FixedUpdate()
    {
        distance = Vector3.Distance(transform.position, player.position);

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


        if (isAttack)
        {
            agent.isStopped = true;
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
            //Debug.Log("©Л");
            agent.SetDestination(endPos);

            if (Vector3.Distance(transform.position, endPos) < 3f)
            {
                isGoingRight = false;
                agent.isStopped = true;

            }
        }
        else
        {
            //Debug.Log("аб");
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
        if (Vector3.Distance(transform.position, player.position) <= attackRange)
        {
            state = EnemyState.Attack;
            animator.SetBool("Attack", true);
            ResetPattern();
            return;
        }
        else if (Vector3.Distance(transform.position, player.position) > searchRange)
        {
            ResetPattern();
            animator.SetBool("Run", false);
            return;
        }

        animator.SetBool("Run", true);
        agent.SetDestination(player.position);
    }
    private void AttackUpdate()
    {
        if (distance > attackRange)
        {
            animator.SetBool("Attack", false);
            state = EnemyState.Chase;
        }
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

        if(isAttack)
            yield return null;

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
        curCountPattern = 0;
        isPattern = false;
        isAttack = false;
    }
}