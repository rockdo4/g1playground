using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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

    public float moveSpeed;
    public float attackRange;
    public float searchRange;

    private int curCountPattern;
    private int countPattern;
    private bool isPattern;

    private float distance;
    private Transform player;

    [Header("<Only Idle and Patrol>")]
    [SerializeField]
    public List<EnemyStateData> EnemyStatePattern = new List<EnemyStateData>();

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();


    }

    private void Start()
    {
        player = GameObject.FindWithTag("Player").transform;
        countPattern = EnemyStatePattern.Count - 1;
        curCountPattern = 0;
        state = EnemyStatePattern[0].state;
        isPattern = true;
    }

    private void Update()
    {
        distance = Vector3.Distance(transform.position, player.position);

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
    }

    private void IdleUpdate()
    {
        if (distance < searchRange)
        {
            state = EnemyState.Chase;
            ResetPattern();
        }
    }

    private void ChaseUpdate()
    {
        if (distance < attackRange)
        {
            state = EnemyState.Attack;
            ResetPattern();
            return;
        }

        Vector3 direction = (player.position - transform.position).normalized;
        rb.velocity = direction * moveSpeed;
    }

    private void PatrolUpdate()
    {
        if (distance < searchRange)
        {
            state = EnemyState.Chase;
            ResetPattern();
        }
    }

    private void AttackUpdate()
    {
        if (distance > attackRange)
        {
            state = EnemyState.Chase;
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
    }
}