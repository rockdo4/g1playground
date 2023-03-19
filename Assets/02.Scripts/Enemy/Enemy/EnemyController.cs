using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class EnemyController : Enemy, IAttackable
{
    GameObject attackBox;
    private CapsuleCollider mainColl;
    public BasicAttack meleeAttack;

    [System.Serializable]
    public class EnemyStateData
    {
        public EnemyState state;
        public float second;
    }

    [Header("<Only Idle and Patrol>")]
    [SerializeField]
    public List<EnemyStateData> EnemyStatePattern = new List<EnemyStateData>();
    private int curCountPattern;
    private float patternTime = 0f;
    public bool isGoingRight;

    public float patrolSpeed;
    public float chaseSpeed;
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
                    agent.enabled = true;
                    agent.isStopped = true;
                    agent.velocity = Vector3.zero;
                    rb.isKinematic = true;
                    mainColl.enabled = true;
                    break;
                case EnemyState.Idle:
                    agent.isStopped = true;
                    rb.isKinematic = true;
                    break;
                case EnemyState.Patrol:
                    agent.isStopped = false;
                    agent.speed = patrolSpeed;
                    rb.isKinematic = true;
                    break;
                case EnemyState.Chase:
                    agent.speed = chaseSpeed;
                    agent.enabled = true;
                    agent.isStopped = false;
                    rb.isKinematic = true;
                    break;
                case EnemyState.Attack:
                    agent.isStopped = true;
                    agent.velocity = Vector3.zero;
                    rb.isKinematic = true;
                    break;
                case EnemyState.TakeDamage:
                    agent.isStopped = true;
                    //agent.velocity = Vector3.zero;
                    agent.enabled = false;
                    rb.isKinematic = false;
                    break;
                case EnemyState.Die:
                    agent.velocity = Vector3.zero;
                    agent.enabled = true;
                    rb.isKinematic = true;
                    mainColl.enabled = false;
                    break;
            }

            //Debug.Log(State);
        }
    }


    public bool preGoingRight;

    protected override void Awake()
    {
        //StartCoroutine(RestorePosition());

        base.Awake();
        mainColl = GetComponent<CapsuleCollider>();
        attackBox = GameObject.Find(gameObject.name + "/AttackBox");
        attackBox.SetActive(false);
        preGoingRight = isGoingRight;
    }
    private IEnumerator RestorePosition()
    {
        yield return new WaitForEndOfFrame();

    }
    protected override void Start()
    {
        animator.ResetTrigger("TakeDamage");
        base.Start();
    }
    protected override void OnEnable()
    {
        base.OnEnable();

        State = EnemyState.None;
        ResetPattern();
    }

    

    protected void Update()
    {
        attackTime += Time.deltaTime;

        switch (State)
        {
            case EnemyState.None:
                None();
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

        animator.SetFloat("Move", agent.velocity.magnitude);
    }
    protected void None()
    {
        SaveFloorLength();
        State = EnemyStatePattern[0].state;
    }
    protected override void IdleUpdate()
    {
        ChangePattern();
        RayShooter(searchRange);
    }
    protected override void PatrolUpdate()
    {
        ChangePattern();
        RayShooter(searchRange);

        if (isGoingRight)
        {
            if (Vector3.Distance(transform.position, endPos) < 0.1f)
            {
                agent.velocity = Vector3.zero;

                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(Vector3.left), Time.deltaTime * 10f);
                if (Quaternion.Angle(transform.rotation, Quaternion.LookRotation(startPos - transform.position).normalized) <= 5f)
                    isGoingRight = false;

                return;
            }
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(endPos - transform.position).normalized, Time.deltaTime * 10f);
            agent.SetDestination(endPos);
        }
        else
        {
            if (Vector3.Distance(transform.position, startPos) < 0.1f)
            {
                agent.velocity = Vector3.zero;

                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(Vector3.right).normalized, Time.deltaTime * 10f);
                if (Quaternion.Angle(transform.rotation, Quaternion.LookRotation(endPos - transform.position).normalized) <= 5f)
                    isGoingRight = true;

                return;
            }
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(startPos - transform.position).normalized, Time.deltaTime * 10f);
            agent.SetDestination(startPos);
        }
    }

    private bool currGoingRight;
    protected override void ChaseUpdate()
    {

        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(player.transform.position - transform.position).normalized, Time.deltaTime * 10f);

        if (player.transform.position.x - transform.position.x > 0)
            isGoingRight = true;
        else
            isGoingRight = false;

        if (currGoingRight != isGoingRight)
        {
            agent.velocity = Vector3.zero;
            currGoingRight = isGoingRight;
        }

        if (Quaternion.Angle(transform.rotation, Quaternion.LookRotation(player.transform.position - transform.position).normalized) <= 10f)
        {
            agent.isStopped = false;
            agent.SetDestination(player.transform.position);
        }
        else
        {
            agent.isStopped = true;
        }

        if (RayShooter(attackRange))
        {
            if (attackTime >= attackCool)
                State = EnemyState.Attack;
            else
            {
                agent.isStopped = true;
                agent.velocity = Vector3.zero;
            }
        }
        else
            agent.isStopped = false;


        if (Vector3.Distance(transform.position, player.transform.position) >= searchRange * 2f)
        {
            State = EnemyState.None;
            ResetPattern();
        }
    }

    protected override void AttackUpdate()
    {
        if (attackTime >= attackCool)
        {
            animator.SetTrigger("Attack");
            attackTime = 0f;
            return;
        }
    }

    public void TakeDamageUpdate()
    {

    }

    public void OnAttack(GameObject attacker, Attack attack, Vector3 attackPos)
    {
        if (State == EnemyState.Die)
            return;

        State = EnemyState.TakeDamage;
        animator.SetTrigger("TakeDamage");
        // takeDamageCoolTime = 0f;
    }
    protected override void DieUpdate()
    {
        //base.DieUpdate();
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
            startPos = collider.bounds.center - new Vector3((floorLength / 2) - 0.5f, -0.5f, 0);
            endPos = collider.bounds.center + new Vector3((floorLength / 2) - 0.5f, 0.5f, 0);
        }
        else
        {
#if UNITY_EDITOR
            Debug.Log("Arrangement Fail");
            Debug.Log("��ġ ����� !!");
#endif
        }
    }
    private void ChangePattern()
    {
        patternTime += Time.deltaTime;

        if (EnemyStatePattern[curCountPattern].second > patternTime)
            return;

        patternTime = 0f;

        if (curCountPattern == EnemyStatePattern.Count - 1)
        {
            curCountPattern = 0;
        }
        else
        {
            ++curCountPattern;
        }

        State = EnemyStatePattern[curCountPattern].state;
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

        if (Physics.Raycast(ray, out RaycastHit hit, range, LayerMask.GetMask("Player")))
        {
            if (hit.collider.tag == "Player")
            {
                if (State != EnemyState.Chase)
                {
                    State = EnemyState.Chase;
                    return true;
                }

                if (State == EnemyState.Chase)
                {
                    return true;
                }
            }
        }
        return false;
    }

    private void Attack()
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

    protected void ResetPattern()
    {
        patternTime = 0f;
        curCountPattern = 0;
        isGoingRight = preGoingRight;
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

    private void AttackDone()
    {
        State = EnemyState.Chase;
        attackBox.SetActive(false);
    }
    private void TakeDamageDone()
    {
        if (State == EnemyState.Die)
            return;

        State = EnemyState.Chase;
    }
    private void DieDone()
    {
        gameObject.SetActive(false);
    }
}
