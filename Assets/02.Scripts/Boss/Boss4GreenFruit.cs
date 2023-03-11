using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using static EnemyController;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class Boss4GreenFruit : BossController
{
    private BossState state;
    public GameObject mask;
    public GameObject attackBox;
    public GameObject dashBox;
    public GameObject skillPivot;
    public GameObject indicatorBox;

    public BasicAttack meleeAttack;
    public BasicAttack meleeSkill;
    public SkillAttack projectileSkill;

    public float moveSpeed;
    public float attackRange;
    public float idleTime;
    public float dashTime;
    public float projectileTime;
    private float dashCoolTime;
    private float projectileCoolTime;

    public override BossState State
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
                case BossState.None:
                    rb.isKinematic = true;
                    agent.enabled = true;
                    break;
                case BossState.Spawn:
                    rb.isKinematic = true;
                    agent.enabled = true;
                    break;
                case BossState.Motion:
                    agent.enabled = true;
                    agent.velocity = Vector3.zero;
                    agent.isStopped = true;
                    rb.isKinematic = true;
                    break;
                case BossState.Idle:
                    rb.isKinematic = true;
                    agent.velocity = Vector3.zero;
                    agent.enabled = true;
                    agent.isStopped = true;
                    break;
                case BossState.Chase:
                    agent.enabled = true;
                    agent.isStopped = false;
                    rb.isKinematic = true;
                    break;
                case BossState.TakeDamage:
                    agent.enabled = true;
                    rb.isKinematic = true;
                    break;
                case BossState.Attack:
                    agent.isStopped = true;
                    rb.isKinematic = false;
                    agent.enabled = false;
                    break;
                case BossState.Skill:
                    agent.isStopped = true;
                    agent.velocity = Vector3.zero;
                    rb.isKinematic = false;
                    agent.enabled = false;
                    break;
                case BossState.Die:
                    animator.SetTrigger("Die");
                    break;
            }
        }
    }

    protected override void Awake()
    {
        base.Awake();
        Debug.Log(gameObject.name);
        mask = GameObject.Find(gameObject.name + "/Mask");
        attackBox = GameObject.Find(gameObject.name + "/AttackBox");
        attackBox.SetActive(false);
        dashBox = GameObject.Find(gameObject.name + "/DashBox");
        dashBox.SetActive(false);
        skillPivot = GameObject.Find(gameObject.name + "/SkillPivot");
        indicatorBox = GameObject.Find(gameObject.name + "/IndicatorBoxPivot");
        indicatorBox.SetActive(false);
        State = BossState.None;
    }

    protected override void Start()
    {
        agent.speed = moveSpeed;
        GetComponent<DestructedEvent>().OnDestroyEvent = () => State = BossState.Die;
        base.Start();
    }

    protected void OnEnable()
    {
        attackBox.SetActive(false);
        dashBox.SetActive(false);
        indicatorBox.SetActive(false);

        State = BossState.None;
    }

    public void Update()
    {
        if (State == BossState.None)
        {
            if (Vector3.Distance(player.transform.position, transform.position) <= 20f)
            {
                State = BossState.Spawn;
            }
        }

        dashCoolTime += Time.deltaTime;
        projectileCoolTime += Time.deltaTime;

        switch (State)
        {
            case BossState.Spawn:
                Spawn();
                break;
            case BossState.Idle:
                Idle();
                break;
            case BossState.Motion:
                Motion();
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
        animator.SetFloat("Move", agent.velocity.magnitude);
        //Debug.Log(State);
    }
    protected override void Spawn()
    {
        animator.SetTrigger("Spawn");
    }
    protected override void Motion()
    {
        LookAtFront();
    }
    private float idleCool = 0f;
    protected override void Idle()
    {

        idleCool += Time.deltaTime;

        //if (LookAtTarget())
        if (idleCool >= idleTime)
        {
            idleCool = 0f;
            State = BossState.Motion;
            animator.SetTrigger("Motion");
        }
    }

    private bool isSkillType = false;
    protected override void Chase()
    {
        if (LookAtTarget())
            agent.SetDestination(player.transform.position);

        if (Vector3.Distance(transform.position, player.transform.position) <= attackRange)
        {
            if (!LookAtTarget())
                return;

            if (GetIsSkillOn())
            {
                if (dashCoolTime >= dashTime)
                {
                    animator.SetTrigger("DashSkill");
                    isSkillType = false;
                    dashCoolTime = 0f;
                }
                else if (projectileCoolTime >= projectileTime)
                {
                    animator.SetTrigger("ProjectileSkill");
                    isSkillType = true;
                    projectileCoolTime = 0f;
                }
                State = BossState.Skill;
            }
            else
            {
                State = BossState.Attack;
                RandomAttack();
            }
        }
    }

    protected override void Attack()
    {
        LookAtTarget();
    }

    protected override void Skill()
    {
        if (isSkillType)
        {
        }
        else
        {
        }
    }

    protected override void TakeDamage()
    {

    }

    protected override void Die()
    {

    }

    private void SpawnDone()
    {
        mask.SetActive(false);
        State = BossState.Motion;
        animator.SetTrigger("Motion");

    }

    private void DieDone()
    {
        gameObject.SetActive(false);
    }

    private int motionCount = 0;
    private int maxMotionCount = 1;
    private void MotionDone()
    {
        if (motionCount != maxMotionCount)
        {
            motionCount++;
            return;
        }

        if (motionCount == maxMotionCount)
        {
            State = BossState.Chase;
            animator.SetTrigger("Chase");
            motionCount = 0;
        }
    }
    private void AttackDone()
    {
        if (!ChaeckAttackCount())
            State = BossState.Idle;

        //attackBox.GetComponent<BoxCollider>().enabled = false;
        attackBox.SetActive(false);
    }
    private void DashSkillDone()
    {
        State = BossState.Idle;
        dashBox.SetActive(false);

    }

    private void Bite()
    {
        attackBox.SetActive(true);
        //attackBox.GetComponent<BoxCollider>().enabled = true;

    }

    private float dashStopTime = 1f;
    IEnumerator CorStopDash()
    {
        animator.speed = 0f;
        indicatorBox.SetActive(true);
        SetIndicatorBoxScale();

        var rot = transform.eulerAngles;

        if (rot.y > 180f && rot.y < 360f)
            rot.y = 270f;
        else
            rot.y = 90f;
        transform.rotation = Quaternion.Euler(rot);

        yield return new WaitForSeconds(dashStopTime);

        animator.speed = 1f;
        indicatorBox.SetActive(false);
        dashBox.SetActive(true);
        rb.AddForce(transform.forward * 20f, ForceMode.Impulse);
    }

    private void ProjectileSkill()
    {
        ((EnemyStraightSpell)projectileSkill).Fire(gameObject, skillPivot.transform.position, transform.forward);
    }

    private void ProjectileSkillDone()
    {
        State = BossState.Idle;
    }

    private int attackCount = 0;
    private void RandomAttack()
    {
        attackCount = Random.Range(1, 3);

        ChaeckAttackCount();
    }
    private bool ChaeckAttackCount()
    {
        if (attackCount == 0)
            return false;

        animator.SetTrigger("Attack");
        --attackCount;
        return true;
    }


    protected bool GetIsSkillOn()
    {
        if (dashCoolTime >= dashTime || projectileCoolTime >= projectileTime)
            return true;
        else
            return false;
    }
    protected void SetIndicatorBoxScale()
    {
        var centerPos = transform.position;
        centerPos.y += 1f;
        var temp = transform;
        transform.position = centerPos;
        Ray ray = new Ray(centerPos, transform.TransformDirection(Vector3.forward));
        RaycastHit hit;

        var currentScale = indicatorBox.transform.localScale;
        var layermask = LayerMask.GetMask("Ground");
        if (Physics.Raycast(ray, out hit, 7f, layermask))
        {
            currentScale.z = (hit.distance - 1f) * 0.5f;
        }
        else
        {
            currentScale.z = 6f * 0.5f;
        }

        indicatorBox.transform.localScale = currentScale;
    }

    private void OnTriggerEnter(Collider collider)
    {

        if (indicatorBox.activeSelf)
            return;

        if (collider.gameObject.CompareTag("Player") && collider.gameObject.GetComponent<ObjectMass>() != null)
        {
            Debug.Log(State);

            if (State == BossState.Attack)
                meleeAttack.ExecuteAttack(gameObject, player.gameObject, transform.position);
            else if (State == BossState.Skill)
                meleeAttack.ExecuteAttack(gameObject, player.gameObject, transform.position);
        }
    }
}