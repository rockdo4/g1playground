using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMeleeFixed : Enemy
{
    GameObject attackBox;
    private CapsuleCollider mainColl;
    public BasicAttack meleeAttack;

    public bool isGoingRight;
    public float searchRange;
    public float attackRange;
    public float attackCool;
    private float attackTime;
    public override EnemyState State
    {
        get { return state; }
        protected set
        {
            var prevState = state;
            if (EnemyState.Die == prevState)
                return;

            state = value;
            if (prevState == state)
                return;

            switch (State)
            {
                case EnemyState.None:
                    break;
                case EnemyState.Hide:
                    break;
                case EnemyState.Idle:
                    break;
                case EnemyState.Attack:
                    break;
                case EnemyState.TakeDamage:
                    break;
                case EnemyState.Die:
                    mainColl.enabled = false;
                    break;
            }

            Debug.Log(State);
        }
    }

    protected override void Awake()
    {
        animator = GetComponent<Animator>();
        mainColl = GetComponent<CapsuleCollider>();
        attackBox = GameObject.Find(gameObject.name + "/AttackBox");
        attackBox.SetActive(false);
    }
    protected override void Start()
    {
        animator.ResetTrigger("TakeDamage");
        base.Start();
    }

    void Update()
    {
        attackTime += Time.deltaTime;

        switch (State)
        {
            case EnemyState.None:
                None();
                break;
            case EnemyState.Hide:
                HideUpdate();
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
            case EnemyState.TakeDamage:
                TakeDamageUpdate();
                break;
            case EnemyState.Die:
                DieUpdate();
                break;
        }
    }

    protected void None()
    {
        if (player.transform.position.x - transform.position.x > 0)
            isGoingRight = true;
        else
            isGoingRight = false;
        if (LookAtTarget())
        {
            if (RayShooter(searchRange))
            {
                State = EnemyState.Idle;
                animator.SetTrigger("Idle");
            }
        }
    }
    protected void HideUpdate()
    {

    }
    protected override void IdleUpdate()
    {
        if (Vector3.Distance(transform.position, player.transform.position) >= searchRange * 2f)
        {
            State = EnemyState.None;
            animator.SetTrigger("Hide");
        }

        if (player.transform.position.x - transform.position.x > 0)
            isGoingRight = true;
        else
            isGoingRight = false;

        if (LookAtTarget() && RayShooter(attackRange))
        {
            if (attackTime >= attackCool)
            {
                State = EnemyState.Attack;
                animator.SetTrigger("Attack");
                attackTime = 0f;
                return;
            }
        }
    }

    protected override void AttackUpdate()
    {

    }

    public void TakeDamageUpdate()
    {

    }

    void AttackDone()
    {
        State = EnemyState.Idle;
    }
    private void TakeDamageDone()
    {
        if (State == EnemyState.Die)
            return;

        State = EnemyState.Idle;
    }
    private void DieDone()
    {
        gameObject.SetActive(false);
    }

    private bool RayShooter(float range)
    {
        Vector3 rayOrigin;
        Ray ray;
        rayOrigin = transform.position + new Vector3(0, 0.5f, 0);

        if (isGoingRight)
        {
            ray = new Ray(rayOrigin, Vector3.right);
        }
        else
        {
            ray = new Ray(rayOrigin, Vector3.left);
        }
#if UNITY_EDITOR
        Debug.DrawRay(ray.origin, ray.direction * range, Color.red);
#endif

        if (Physics.Raycast(ray, out RaycastHit hit, range))
        {
            if (hit.collider.tag == "Player")
            {
                if (State == EnemyState.None)
                {
                    State = EnemyState.Idle;
                    return true;
                }

                if (State == EnemyState.Idle)
                {
                    return true;
                }
            }
        }
        return false;
    }
}
