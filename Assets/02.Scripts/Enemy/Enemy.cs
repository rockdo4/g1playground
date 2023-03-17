using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;

public class Enemy : MonoBehaviour
{
    protected EnemyState state;

    protected GameObject player;
    protected Vector3 mySpawnPos;
    protected Quaternion mySpawnDir;
    protected bool isLive = true;

    public enum EnemyState
    {
        None,
        Hide,
        Spawn,
        Motion,
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
    //protected CapsuleCollider collider;

    public virtual EnemyState State
    {
        get;
        protected set;
    }

    protected virtual void Awake()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        agent = GetComponent<NavMeshAgent>();
        //collider = GetComponent<CapsuleCollider>();
        mySpawnPos = transform.position;
        mySpawnDir = transform.rotation;
    }
    protected virtual void Start()
    {
        player = GameManager.instance.player;
        GetComponent<DestructedEvent>().OnDestroyEvent = () =>
        {
            State = EnemyState.Die;
            animator.ResetTrigger("TakeDamage");
            animator.SetTrigger("Die");
            isLive = false;
        };
    }
    protected virtual void OnEnable()
    {
        transform.position = mySpawnPos;
        transform.rotation = mySpawnDir;
        isLive = true;
    }
    protected virtual void Spawn() { }
    protected virtual void IdleUpdate() { }
    protected virtual void Motion() { }
    protected virtual void PatrolUpdate() { }
    protected virtual void ChaseUpdate() { }
    protected virtual void AttackUpdate() { }
    protected virtual void Skill() { }
    protected virtual void TakeDamageUpdate() { }
    protected virtual void Groggy() { }
    protected virtual void DieUpdate() { }

    protected Vector3 GetSpawnPos() { return mySpawnPos; }
    protected bool LookAtTarget()
    {
        Vector3 dir = player.transform.position - transform.position;
        dir.y = 0;
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dir), Time.deltaTime * 10f);

        if (Quaternion.Angle(transform.rotation, Quaternion.LookRotation(dir)) < 1f)
            return true;

        return false;
    }

    public bool GetIsLive()
    {
        return isLive;
    }

    protected void LookAtFront()
    {
        var front = Quaternion.Euler(0, 180, 0);
        transform.rotation = Quaternion.Slerp(transform.rotation, front, Time.deltaTime * 10f);
    }
}