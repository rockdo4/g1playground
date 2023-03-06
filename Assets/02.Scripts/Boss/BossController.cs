using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using static EnemyController;

public class BossController : MonoBehaviour
{
    public enum BossState
    {
        None,
        Idle,
        Chase,
        Attack,
        Skill,
        TakeDamage,
        Groggy,
        Die,
    }
    private Animator animator;
    private Rigidbody rb;
    private NavMeshAgent agent;
    private CapsuleCollider collider;
    private Boss boss;
    private void Awake()
    {
        //animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        agent = GetComponent<NavMeshAgent>();
        collider = GetComponent<CapsuleCollider>();
        boss = GetComponent<Boss4GreenFruit>();
    }
    void Start()
    {

    }

    private void Update()
    {

        boss.Idle();

    }
}
