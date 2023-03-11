using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossTemp : Enemy
{
    public override EnemyState State
    {
        get { return State; }
        protected set
        {
            State = value;

            switch (State)
            {
                case EnemyState.None:
                    break;
                case EnemyState.Spawn:
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
        State = EnemyState.None;
    }

    protected override void Start()
    {
        base.Start();
    }

    public void Update()
    {
        switch (State)
        {
            case EnemyState.Spawn:
                Spawn();
                break;
            case EnemyState.Idle:
                Idle();
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
    }
    protected override void Spawn()
    {

    }

    protected override void Idle()
    {

    }

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

    }

}
