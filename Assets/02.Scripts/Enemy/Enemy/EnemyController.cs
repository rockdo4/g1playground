using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class EnemyController : Enemy
{
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
                    agent.isStopped = true;
                    rb.isKinematic = true;
                    break;
                case EnemyState.Idle:
                    agent.isStopped = true;
                    rb.isKinematic = true;
                    break;
                case EnemyState.Patrol:
                    agent.isStopped = false;
                    rb.isKinematic = true;
                    break;
                case EnemyState.Chase:
                    break;
                case EnemyState.TakeDamage:
                    break;
                case EnemyState.Attack:
                    break;
                case EnemyState.Die:
                    break;
            }

            Debug.Log(State);
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
            case EnemyState.None:
                None();
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
            case EnemyState.TakeDamage:
                TakeDamage();
                break;
            case EnemyState.Die:
                Die();
                break;
        }

        animator.SetFloat("Move", agent.velocity.magnitude);
    }
    //protected override void Spawn()
    //{
    //}
    //protected override void Motion()
    //{
    //}
    protected void None()
    {
        SaveFloorLength();
        State = EnemyStatePattern[0].state;
    }
    protected override void Idle()
    {

        ChangePattern();


    }
    public bool isGoingRight;
    protected override void Patrol()
    {
        ChangePattern();

        if (isGoingRight)
        {
            if (Vector3.Distance(transform.position, endPos) < 0.1f)
            {
                agent.velocity = Vector3.zero;

                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(startPos - transform.position).normalized, Time.deltaTime * 10f);
                Vector3 right = new Vector3(1, 0, 0);
                //transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(right), Time.deltaTime * 10f);
                if (Quaternion.Angle(transform.rotation, Quaternion.LookRotation(startPos - transform.position).normalized) <= 1f)
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


                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(endPos - transform.position).normalized, Time.deltaTime * 10f);
                Vector3 left = new Vector3(-1, 0, 0);
                // transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(Vector3.left).normalized, Time.deltaTime * 10f);
                if (Quaternion.Angle(transform.rotation, Quaternion.LookRotation(endPos - transform.position).normalized) <= 1f)
                    isGoingRight = true;

                return;
            }
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(startPos - transform.position).normalized, Time.deltaTime * 10f);
            agent.SetDestination(startPos);
        }
        Debug.Log(isGoingRight);
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
            startPos = collider.bounds.center - new Vector3((floorLength / 2), -0.5f, 0);
            endPos = collider.bounds.center + new Vector3((floorLength / 2), 0.5f, 0);
        }
        else
        {
#if UNITY_EDITOR
            Debug.Log("Arrangement Fail");
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

}
