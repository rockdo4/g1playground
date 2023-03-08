using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossTemp : BossController
{
    public override BossState State
    {
        get { return State; }
        protected set
        {
            State = value;

            switch (State)
            {
                case BossState.None:
                    break;
                case BossState.Spawn:
                    break;
                case BossState.Idle:
                    break;
                case BossState.Chase:
                    break;
                case BossState.TakeDamage:
                    break;
                case BossState.Attack:
                    break;
                case BossState.Skill:
                    break;
                case BossState.Die:
                    break;
            }

        }
    }

    protected override void Awake()
    {
        base.Awake();
        State = BossState.None;
    }

    protected override void Start()
    {
        base.Start();
    }

    public void Update()
    {
        switch (State)
        {
            case BossState.Spawn:
                Spawn();
                break;
            case BossState.Idle:
                Idle();
                break;
            case BossState.Patrol:
                Patrol();
                break;
            case BossState.Chase:
                Chase();
                break;
            case BossState.Attack:
                Attack();
                break;
            case BossState.Skill:
                Skill();
                break;
            case BossState.TakeDamage:
                TakeDamage();
                break;
            case BossState.Die:
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
