using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class EnemyController : Enemy
{

    public override EnemyState State
    {
        get { return state; }
        protected set
        {
            var prevState = state;
            state = value;

            if (prevState == state)
                return;

            switch (State)
            {
                case EnemyState.None:

                    break;
                case EnemyState.Spawn:

                    break;
                case EnemyState.Motion:

                    break;
                case EnemyState.Idle:

                    break;
                case EnemyState.Chase:

                    break;
                case EnemyState.TakeDamage:

                    break;
                case EnemyState.Attack:

                    break;
                case EnemyState.Skill:

                    break;
                case EnemyState.Die:
                    break;
            }
        }
    }



    protected override void Awake()
    {
        base.Awake();
    }
    protected override void Start()
    {


    }
    protected override void OnEnable()
    {
        base.OnEnable();

        State = EnemyState.None;
    }
    protected void Update()
    {


        switch (State)
        {
            case EnemyState.Spawn:
                Spawn();
                break;
            case EnemyState.Idle:
                Idle();
                break;
            case EnemyState.Motion:
                Motion();
                break;
            case EnemyState.Patrol:
                Patrol();
                break;
            case EnemyState.Chase:
                Chase();
                break;
            case EnemyState.Attack:
                Attack();
                break;
            case EnemyState.Skill:
                Skill();
                break;
            case EnemyState.TakeDamage:
                TakeDamage();
                break;
            case EnemyState.Die:
                Die();
                break;
        }
        animator.SetFloat("Move", agent.velocity.magnitude);
    }
    protected override void Spawn()
    {
    }
    protected override void Motion()
    {
    }
    protected override void Idle()
    {

    }

    private bool isSkillType = false;
    protected override void Chase()
    {

    }

    protected override void Attack()
    {
    }

    protected override void Skill()
    {

    }

    protected override void TakeDamage()
    {

    }

    protected override void Die()
    {
        base.Die();
    }

    protected float floorLength;
    protected Vector3 startPos;
    protected Vector3 endPos;
    private void SaveFloorLength()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, -Vector3.up, out hit))
        {
            Collider collider = hit.collider;
            floorLength = collider.bounds.size.x;
            startPos = collider.bounds.center - new Vector3((floorLength / 2), 0, 0);
            endPos = collider.bounds.center + new Vector3((floorLength / 2), 0, 0);
        }
        else
        {
#if UNITY_EDITOR
            Debug.Log("NULL!!");
#endif
        }
    }
}
