using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using static UnityEngine.Rendering.DebugUI;
using static UnityEngine.RuleTile.TilingRuleOutput;


public class EnemyController : MonoBehaviour, IAttackable
{
    [System.Serializable]
    public class EnemyStateData
    {
        public EnemyState state;
        public float second;
    }

    [System.Serializable]
    public enum EnemyState
    {
        None,
        Idle,
        Chase,
        Patrol,
        Attack,
        TakeDamage,
        Stun,
        Die,
    }

    private Rigidbody rb;
    private NavMeshAgent agent;
    private Animator animator;
    private CapsuleCollider capsuleCollider;

    private EnemyState state;

    public GameObject attackPivot;

    public float chaseSpeed;
    public float patrolSpeed;
    public float attackRange;
    public float searchRange;
    public float attackCool;
    private float time = 0f;

    private int curCountPattern;
    private int countPattern;
    private bool isPattern;

    private GameObject player;


    private float floorLength;
    private Vector3 startPos;
    private Vector3 endPos;
    private bool isGoingRight;

    public BasicAttack basicAttack;
    public Status status;
    private EnemyStateIcon icon;

    [Header("<Only Idle and Patrol>")]
    [SerializeField]
    public List<EnemyStateData> EnemyStatePattern = new List<EnemyStateData>();

    private bool die = false;

    public EnemyState State
    {
        get { return state; }
        private set
        {
            if (die)
                return;


            var prevState = state;
            state = value;

            if (prevState != EnemyState.Idle && prevState != EnemyState.Patrol)
            {
                if (value == EnemyState.Idle || value == EnemyState.Patrol)
                {
                    SaveFloorLength();
                    isPattern = true;
                }
            }
            if (State != EnemyState.TakeDamage)
            {
                agent.enabled = true;
            }

            if (prevState == state && state == EnemyState.TakeDamage)
                return;

            icon.ChangeIcon(State);

            switch (State)
            {
                case EnemyState.Idle:
                    agent.isStopped = true;
                    rb.isKinematic = true;
                    break;
                case EnemyState.Patrol:
                    SaveFloorLength();
                    agent.speed = patrolSpeed;
                    agent.isStopped = false;
                    rb.isKinematic = true;
                    break;
                case EnemyState.Chase:
                    agent.speed = chaseSpeed;
                    agent.isStopped = false;
                    rb.isKinematic = true;
                    break;
                case EnemyState.Attack:
                    agent.velocity = Vector3.zero;
                    agent.isStopped = true;
                    rb.isKinematic = true;
                    break;
                case EnemyState.TakeDamage:
                    agent.isStopped = true;
                    rb.isKinematic = false;
                    agent.enabled = false;
                    takeDamageCoolTime = 0f;
                    break;
                case EnemyState.Die:
                    agent.velocity = Vector3.zero;
                    agent.isStopped = true;
                    agent.enabled = false;
                    rb.isKinematic = true;
                    capsuleCollider.enabled = false;
                    die = true;
                    break;

            }
        }
    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        status = GetComponent<Status>();
        capsuleCollider = GetComponent<CapsuleCollider>();
        agent.speed = chaseSpeed;
        //agent.stoppingDistance = attackRange;
        GetComponent<DestructedEvent>().OnDestroyEvent = OnDestroyObj;
    }

    void Start()
    {
        player = GameManager.instance.player;
        icon = GetComponentInChildren<EnemyStateIcon>();
        State = EnemyStatePattern[0].state;
        countPattern = EnemyStatePattern.Count - 1;
        curCountPattern = 0;
        isPattern = true;

        SaveFloorLength();
    }

    void OnEnable()
    {
        player = GameManager.instance.player;
        icon = GetComponentInChildren<EnemyStateIcon>();
        State = EnemyStatePattern[0].state;
        countPattern = EnemyStatePattern.Count - 1;
        curCountPattern = 0;
        isPattern = true;
        StartCoroutine(CSaveFloorLength());
    }

    IEnumerator CSaveFloorLength()
    {
        yield return null;
        SaveFloorLength();


    }


    void Update()
    {

       
        time += Time.deltaTime;

        if (isPattern)
        {
            ChangePatteurn();
        }

        switch (state)
        {
            case EnemyState.Idle:
                IdleUpdate();
                break;
            case EnemyState.Chase:
                ChaseUpdate();
                break;
            case EnemyState.Patrol:
                PatrolUpdate();
                break;
            case EnemyState.Attack:
                AttackUpdate();
                break;
            case EnemyState.TakeDamage:
                TakeDamageUpdate();
                break;
            case EnemyState.Die:
                break;
        }

        //Debug.Log(state);
        animator.SetFloat("Move", agent.velocity.magnitude);
    }

    private void IdleUpdate()
    {
        var pos = GetComponent<CapsuleCollider>();

        Ray ray = new Ray(pos.bounds.max, pos.transform.forward);
        RaycastHit hit;
        //Debug.DrawRay(ray.origin, ray.direction * searchRange, Color.red);
        if (Physics.Raycast(ray, out hit, searchRange))
        {
            if (hit.collider.tag == "Player")
            {
                State = EnemyState.Chase;
                return;
            }
        }
        if (isGoingRight)
        {
            if (Vector3.Distance(transform.position, endPos) < 2f)
            {
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(startPos - transform.position).normalized, Time.deltaTime * 10f);
                if (Quaternion.Angle(transform.rotation, Quaternion.LookRotation(startPos - transform.position).normalized) < 10f)
                    isGoingRight = false;

                return;
            }
        }
        else
        {
            if (Vector3.Distance(transform.position, startPos) < 2f)
            {
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(endPos - transform.position).normalized, Time.deltaTime * 10f);
                if (Quaternion.Angle(transform.rotation, Quaternion.LookRotation(endPos - transform.position).normalized) < 10f)
                    isGoingRight = true;

                return;
            }
        }
    }

    private void PatrolUpdate()
    {
        var pos = GetComponent<CapsuleCollider>();

        Ray ray = new Ray(pos.bounds.max, pos.transform.forward);
        RaycastHit hit;
        //Debug.DrawRay(ray.origin, ray.direction * searchRange, Color.red);
        if (Physics.Raycast(ray, out hit, searchRange))
        {
            if (hit.collider.tag == "Player")
            {
                State = EnemyState.Chase;
                return;
            }
        }

        if (isGoingRight)
        {
            if (Vector3.Distance(transform.position, endPos) < 1f)
            {
                agent.velocity = Vector3.zero;

                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(startPos - transform.position).normalized, Time.deltaTime * 10f);
                if (Quaternion.Angle(transform.rotation, Quaternion.LookRotation(startPos - transform.position).normalized) < 10f)
                    isGoingRight = false;

                return;
            }
            agent.SetDestination(endPos);
        }
        else
        {
            if (Vector3.Distance(transform.position, startPos) < 1f)
            {
                agent.velocity = Vector3.zero;


                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(endPos - transform.position).normalized, Time.deltaTime * 10f);

                if (Quaternion.Angle(transform.rotation, Quaternion.LookRotation(endPos - transform.position).normalized) < 10f)
                    isGoingRight = true;

                return;
            }
            agent.SetDestination(startPos);
        }
    }

    private void ChaseUpdate()
    {
        //if (Vector3.Distance(transform.position, player.transform.position) <= attackRange + 0.5f)
        //{
        //    State = EnemyState.Attack;
        //    return;
        //}

        if (Vector3.Distance(transform.position, player.transform.position) >= searchRange * 3)
        {
            State = EnemyState.Idle;
            return;
        }
        var isGround = player.GetComponent<PlayerController>().isGrounded;

        if (!isGround) { return; }

        Vector3 dir = player.transform.position - transform.position;
        dir.y = 0;
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dir), Time.deltaTime * 10f);

        if (Quaternion.Angle(transform.rotation, Quaternion.LookRotation(dir)) < 1f)
            agent.SetDestination(player.transform.position);
    }
    private void AttackUpdate()
    {
        if (Vector3.Distance(transform.position, player.transform.position) >= attackRange + 0.5f)
        {
            State = EnemyState.Chase;
        }

        var lookDirection = (player.transform.position - transform.position).normalized;
        lookDirection.y = 0f;
        transform.forward = lookDirection;

        if (time > attackCool)
        {
            animator.SetTrigger("Attack");
            time = 0f;
        }
    }

    private void SaveFloorLength()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, -Vector3.up, out hit))
        {
            Collider collider = hit.collider;
            floorLength = collider.bounds.size.x;

            startPos = collider.bounds.center - (new Vector3((floorLength / 2), 0, 0));
            endPos = collider.bounds.center + (new Vector3((floorLength / 2), 0, 0));

            //isGoingRight = true;
        }
        else
        {
#if UNITY_EDITOR
            Debug.Log("NULL!!");

#endif
        }
    }

    void ChangePatteurn()
    {
        StartCoroutine(CoPatternDelay(EnemyStatePattern[curCountPattern].second));
        isPattern = false;
    }

    IEnumerator CoPatternDelay(float delayTime)
    {
        yield return new WaitForSeconds(delayTime);

        if (State != EnemyState.Idle && State != EnemyState.Patrol)
        {
            isPattern = false;
            curCountPattern = 0;

            yield break;
        }

        if (countPattern == curCountPattern)
        {
            curCountPattern = 0;
        }
        else
        {
            ++curCountPattern;
        }

        isPattern = true;
        State = EnemyStatePattern[curCountPattern].state;
    }

    public void Attack()
    {
        if (die)
            return;

        switch (basicAttack)
        {
            case EnemyMeleeAttack:
                {
                    if (Vector3.Distance(transform.position, player.transform.position) <= attackRange + 0.5f)
                    {
                        basicAttack.ExecuteAttack(gameObject, player.gameObject, transform.position);
                        return;
                    }
                }
                break;

            case EnemyProjectileAttack:
                {
                    ((EnemyProjectileAttack)basicAttack).Fire(gameObject, attackPivot.transform.position, transform.forward);
                    return;
                }
                break;
        }
    }

    private float takeDamageCool = 0.8f;
    private float takeDamageCoolTime = 0f;
    public void TakeDamageUpdate()
    {
        takeDamageCoolTime += Time.deltaTime;

        if (takeDamageCool < takeDamageCoolTime)
        {
            State = EnemyState.Chase;
        }
    }

    public void OnAttack(GameObject attacker, Attack attack, Vector3 attackPos)
    {
        if (State == EnemyState.Die)
            return;

        State = EnemyState.TakeDamage;
        animator.SetTrigger("TakeDamage");
    }

    public void OnDestroyObj()
    {
        if (State == EnemyState.Die)
            return;

        State = EnemyState.Die;
        animator.SetTrigger("Die");
    }

    private void EnemyDie()
    {
        gameObject.SetActive(false);
    }

    public void GetAttackBoxCollStay(Collider collider, Collider attackBoxColl)
    {
        if (state == EnemyState.TakeDamage)
            return;
        if (state == EnemyState.Attack)
            return;
        if (attackBoxColl.tag == "AttackBox")
        {
            if (collider.tag == "Player")
            {
                State = EnemyState.Attack;
                return;
            }
        }
    }
    //public void GetAttackBoxCollExit(Collider collider, Collider attackBoxColl)
    //{
    //    if (state == EnemyState.Chase)
    //        return;

    //    if (attackBoxColl.tag == "AttackBox")
    //    {
    //        if (collider.tag == "Player")
    //        {
    //            State = EnemyState.Chase;
    //            return;
    //        }
    //    }
    //}

}