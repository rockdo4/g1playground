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
                IdleUpdate();
                break;
            case EnemyState.Patrol:
                PatrolUpdate();
                break;
            case EnemyState.Chase:
                ChaseUpdate();
                break;
            case EnemyState.Attack:
                AttackUpdate();
                break;
            case EnemyState.Skill:
                Skill();
                break;
            case EnemyState.TakeDamage:
                TakeDamageUpdate();
                break;
            case EnemyState.Die:
                DieUpdate();
                break;
        }
    }
    protected override void Spawn()
    {

    }

    protected override void IdleUpdate()
    {

    }

    protected override void ChaseUpdate()
    {
    }

    protected override void AttackUpdate()
    {
    }

    protected override void Skill()
    {

    }

    protected override void TakeDamageUpdate()
    {

    }

    protected override void DieUpdate()
    {

    }

}
