using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMeleeFixed : Enemy, IAttackable
{
    GameObject attackBox;
    public GameObject mask;
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
        mask = GameObject.Find(gameObject.name + "/Mask");
        mask.SetActive(false);
        agent = GetComponent<NavMeshAgent>();
        agent.isStopped = true;

    }
    protected override void Start()
    {
        animator.ResetTrigger("TakeDamage");
        base.Start();
    }

    protected override void OnEnable()
    {
        base.OnEnable();

        //mask.SetActive(true);
        attackBox.SetActive(false);
        State = EnemyState.None;
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
                mask.SetActive(false);
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

    void Attack()
    {
        switch (meleeAttack)
        {
            case EnemyMeleeAttack:
                {
                    attackBox.SetActive(true);
                }
                break;
        }
    }
    void AttackDone()
    {
        attackBox.SetActive(false);
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

    private void OnTriggerEnter(Collider collider)
    {
        if (!attackBox.activeSelf)
            return;

        if (collider.gameObject.CompareTag("Player") && collider.gameObject.GetComponent<ObjectMass>() != null)
        {
#if UNITY_EDITOR
            Debug.Log(State);
#endif
            meleeAttack.ExecuteAttack(gameObject, player.gameObject, transform.position);
            attackBox.SetActive(false);
        }
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

    public void OnAttack(GameObject attacker, Attack attack, Vector3 attackPos)
    {
        if (State == EnemyState.Die)
            return;

        State = EnemyState.TakeDamage;
        animator.SetTrigger("TakeDamage");
    }
}
