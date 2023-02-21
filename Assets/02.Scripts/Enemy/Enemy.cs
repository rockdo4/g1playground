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

    private float distance;
    private Transform player;

    [Header("<Only Idle and Patrol>")]
    [SerializeField]
    public List<EnemyStateData> EnemyStatePattern = new List<EnemyStateData>();

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        state = EnemyState.Idle;

    }

    private void Start()
    {
        player = GameObject.FindWithTag("Player").transform;
    }

    private void Update()
    {
        distance = Vector3.Distance(transform.position, player.position);

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
        //Debug.Log(state);
    }

    private void IdleUpdate()
    {
        if (distance < searchRange)
        {
            state = EnemyState.Chase;
        }
    }

    private void ChaseUpdate()
    {
        if (distance < attackRange)
        {
            state = EnemyState.Attack;
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
        }
    }

    private void AttackUpdate()
    {
        if (distance > attackRange)
        {
            state = EnemyState.Chase;
        }
    }
}
