using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BossController : MonoBehaviour
{
    protected GameObject player;

    public enum BossState
    {
        None,
        Spawn,
        Idle,
        Patrol,
        Chase,
        Attack,
        Skill,
        TakeDamage,
        Groggy,
        Die,
    }

    protected Animator animator;
    protected Rigidbody rb;
    protected NavMeshAgent agent;
    protected CapsuleCollider collider;

    public virtual BossState State
    {
        get;
        protected set;
    }

    protected virtual void Awake()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        agent = GetComponent<NavMeshAgent>();
        collider = GetComponent<CapsuleCollider>();
    }
    protected virtual void Start()
    {
        player = GameManager.instance.player;
    }
    protected virtual void Spawn() { }
    protected virtual void Idle() { }
    protected virtual void Patrol() { }
    protected virtual void Chase() { }
    protected virtual void Attack() { }
    protected virtual void Skill() { }
    protected virtual void TakeDamage() { }
    protected virtual void Groggy() { }
    protected virtual void Die() { }

    protected bool LookAtTarget()
    {
        Vector3 dir = player.transform.position - transform.position;
        dir.y = 0;
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dir), Time.deltaTime * 10f);

        if (Quaternion.Angle(transform.rotation, Quaternion.LookRotation(dir)) < 1f)
            return true;

        return false;
    }
}