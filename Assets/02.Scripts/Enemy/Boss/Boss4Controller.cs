using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;


public class Boss4Controller : Enemy
{
    [SerializeField] private GreenPlantSound plantSound;
    public GameObject mask;
    public GameObject attackBox;
    public GameObject dashBox;
    public GameObject skillPivot;
    public GameObject indicatorBox;

    private CapsuleCollider mainColl;

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

    private bool isSpawned = true;
    private bool isDied = true;

    public override EnemyState State
    {
        get { return state; }
        protected set
        {
            var prevState = state;
            state = value;
            if (EnemyState.Die == prevState && EnemyState.None != value)
                return;

            if (prevState == state)
                return;

            switch (State)
            {
                case EnemyState.None:
                    rb.isKinematic = true;
                    agent.enabled = true;
                    agent.isStopped = true;
                    break;
                case EnemyState.Spawn:
                    rb.isKinematic = true;
                    agent.enabled = true;
                    break;
                case EnemyState.Motion:
                    agent.enabled = true;
                    agent.velocity = Vector3.zero;
                    agent.isStopped = true;
                    rb.isKinematic = true;
                    break;
                case EnemyState.Idle:
                    rb.isKinematic = true;
                    agent.velocity = Vector3.zero;
                    agent.enabled = true;
                    agent.isStopped = true;
                    break;
                case EnemyState.Chase:
                    agent.enabled = true;
                    agent.isStopped = false;
                    rb.isKinematic = true;
                    break;
                case EnemyState.TakeDamage:
                    agent.enabled = true;
                    rb.isKinematic = true;
                    break;
                case EnemyState.Attack:
                    agent.isStopped = true;
                    rb.isKinematic = false;
                    agent.enabled = false;
                    break;
                case EnemyState.Skill:
                    agent.isStopped = true;
                    agent.velocity = Vector3.zero;
                    rb.isKinematic = false;
                    agent.enabled = false;
                    break;
                case EnemyState.Die:
                    agent.velocity = Vector3.zero;
                    agent.enabled = true;
                    agent.isStopped = true;
                    rb.isKinematic = true;
                    mainColl.enabled = false;
                    break;
            }
        }
    }

    protected override void Awake()
    {
        base.Awake();
        //Debug.Log(gameObject.name);
        mask = GameObject.Find(gameObject.name + "/Mask");
        attackBox = GameObject.Find(gameObject.name + "/AttackBox");
        attackBox.SetActive(false);
        dashBox = GameObject.Find(gameObject.name + "/DashBox");
        dashBox.SetActive(false);
        skillPivot = GameObject.Find(gameObject.name + "/SkillPivot");
        indicatorBox = GameObject.Find(gameObject.name + "/IndicatorBoxPivot");
        indicatorBox.SetActive(false);
        State = EnemyState.None;
        mainColl = GetComponent<CapsuleCollider>();
    }

    protected override void Start()
    {
        //base.Start();

        agent.speed = moveSpeed;

        player = GameManager.instance.player;
        GetComponent<DestructedEvent>().OnDestroyEvent = () =>
        {
            State = EnemyState.Die;
            animator.ResetTrigger("TakeDamage");
            animator.SetTrigger("Die");
            isLive = false;

            enemyBody.SetActive(false);

            player.GetComponent<PlayerLevelManager>().CurrExp += DataTableMgr.GetTable<MonsterData>().Get(status.id).exp;
        };

    }

    protected override void OnEnable()
    {
        base.OnEnable();
        mainColl.enabled = true;
        mask.SetActive(true);
        attackBox.SetActive(false);
        dashBox.SetActive(false);
        indicatorBox.SetActive(false);
        State = EnemyState.None;
        isSpawned = true;
        isDied = true;
    }

    public void Update()
    {
        if (State == EnemyState.None)
        {
            if (Vector3.Distance(player.transform.position, transform.position) <= 20f)
            {

                State = EnemyState.Spawn;
            }
        }

        dashCoolTime += Time.deltaTime;
        projectileCoolTime += Time.deltaTime;

        switch (State)
        {
            case EnemyState.Spawn:
                Spawn();
                break;
            case EnemyState.Idle:
                IdleUpdate();
                break;
            case EnemyState.Motion:
                Motion();
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
                SkillUpdate();
                break;
            case EnemyState.TakeDamage:
                TakeDamageUpdate();
                break;
            case EnemyState.Die:
                DieUpdate();
                break;
        }
        animator.SetFloat("Move", agent.velocity.magnitude);

        if (Input.GetKeyDown(KeyCode.F))
        {
            State = EnemyState.Die;
            animator.SetTrigger("Die");
            status.CurrHp -= 19000;
        }
        //Debug.Log(State);
    }
    protected override void Spawn()
    {
        if (isSpawned)
        {
            isSpawned = false;            
            var clip = plantSound.spawnClip;
            SoundManager.instance.PlayEnemyEffect(clip);
            animator.SetTrigger("Spawn");
        }
        
    }
    protected override void Motion()
    {
        LookAtFront();
    }
    private float idleCool = 0f;
    protected override void IdleUpdate()
    {
        idleCool += Time.deltaTime;

        //if (LookAtTarget())
        if (idleCool >= idleTime)
        {
            idleCool = 0f;
            State = EnemyState.Motion;
            animator.SetTrigger("Motion");
        }
    }

    private bool isSkillType = false;
    protected override void ChaseUpdate()
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
                    var clip = plantSound.dashAttackClip;
                    SoundManager.instance.PlayEnemyEffect(clip);
                    animator.SetTrigger("DashSkill");
                    isSkillType = false;
                    dashCoolTime = 0f;
                }
                else if (projectileCoolTime >= projectileTime)
                {
                    var clip = plantSound.projectileAttackClip;
                    SoundManager.instance.PlayEnemyEffect(clip);
                    animator.SetTrigger("ProjectileSkill");
                    isSkillType = true;
                    projectileCoolTime = 0f;
                }
                State = EnemyState.Skill;
            }
            else
            {
                State = EnemyState.Attack;
                RandomAttack();
            }
        }
    }

    protected override void AttackUpdate()
    {
        LookAtTarget();
    }

    protected override void SkillUpdate()
    {
        if (isSkillType)
        {
        }
        else
        {
        }
    }

    protected override void TakeDamageUpdate()
    {

    }

    protected override void DieUpdate()
    {
        if (isDied)
        {
            isDied = false;
            var clip = plantSound.dieClip;
            SoundManager.instance.PlayEnemyEffect(clip);
            base.DieUpdate();
        }
        
    }

    private void SpawnDone()
    {
        mask.SetActive(false);
        State = EnemyState.Motion;
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
            State = EnemyState.Chase;
            animator.SetTrigger("Chase");
            motionCount = 0;
        }
    }
    private void AttackDone()
    {
        if (!ChaeckAttackCount())
            State = EnemyState.Idle;

        //attackBox.GetComponent<BoxCollider>().enabled = false;
        attackBox.SetActive(false);
    }
    private void DashSkillDone()
    {
        State = EnemyState.Idle;
        dashBox.SetActive(false);

    }

    private void Bite()
    {
        var clip = plantSound.biteAttackClip;
        SoundManager.instance.PlayEnemyEffect(clip);
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
        State = EnemyState.Idle;
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

        if (State == EnemyState.None && State == EnemyState.Spawn)
            return;

        if (collider.gameObject.CompareTag("Player") && collider.gameObject.GetComponent<ObjectMass>() != null)
        {
            //Debug.Log(State);

            if (State == EnemyState.Attack)
                meleeAttack.ExecuteAttack(gameObject, player.gameObject, transform.position);
            else if (State == EnemyState.Skill)
                meleeAttack.ExecuteAttack(gameObject, player.gameObject, transform.position);
        }
    }
}